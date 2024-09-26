using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FlyEngine
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private GameObject caravanPrefab;
        [SerializeField] private GameObject villagerPrefab;

        [SerializeField] private int initialCaravanCount = 1;
        [SerializeField] private int initialVillagerCount = 2;

        [SerializeField] private float caravanVelocity = 10.0f;
        [SerializeField] private float villagerVelocity = 10.0f;

        private const int MAX_OBJS_PER_DRAWCALL = 1000;
        private const float DEFAULT_VELOCITY = 0.1f;

        private Mesh caravanMesh;
        private Mesh villagerMesh;

        private Material caravanMaterial;
        private Material villagerMaterial;

        private Vector3 caravanScale;
        private Vector3 villagerScale;

        private List<uint> entities;

        private int caravanCount = 0;
        private int villagerCount = 0;

        void Start()
        {
            entities = new List<uint>();
            for (int i = 0; i < initialCaravanCount; i++)
            {
                CreateCaravan(caravanVelocity);
            }

            for (int i = 0; i < initialVillagerCount; i++)
            {
                CreateVillager(villagerVelocity);
            }
         
            villagerMesh = villagerPrefab.GetComponent<MeshFilter>().sharedMesh;
            villagerMaterial = villagerPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            villagerScale = villagerPrefab.transform.localScale;

            caravanMesh = caravanPrefab.GetComponent<MeshFilter>().sharedMesh;
            caravanMaterial = caravanPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            caravanScale = caravanPrefab.transform.localScale;

        }

        void Update()
        {
            ECSManager.Tick(Time.deltaTime);
        }

        void LateUpdate()
        {

            foreach (uint entity in entities)
            {
                PositionComponent position = ECSManager.GetComponent<PositionComponent>(entity);
                position.X += 1 * Time.deltaTime * Random.Range(0.1f, 2.0f);
            }

            List<Matrix4x4[]> caravanDrawMatrix = new List<Matrix4x4[]>();
            List<Matrix4x4[]> villagerDrawMatrix = new List<Matrix4x4[]>();

            FillDrawMatrix(caravanDrawMatrix, caravanCount);
            FillDrawMatrix(villagerDrawMatrix, villagerCount);

            SetTRS(caravanDrawMatrix, caravanScale, 0, caravanCount);
            SetTRS(villagerDrawMatrix, villagerScale, caravanCount, villagerCount);

            DrawMeshes(caravanCount, caravanMesh, caravanMaterial, caravanDrawMatrix);
            DrawMeshes(villagerCount, villagerMesh, villagerMaterial, villagerDrawMatrix);

        }

        private void CreateAgent()
        {
            CreateAgent(DEFAULT_VELOCITY, Random.insideUnitCircle);
        }

        private void CreateAgent(float velocity, Vector3 newPos = default(Vector3))
        {
            uint entityID = ECSManager.CreateEntity();

            ECSManager.AddComponent<PositionComponent>(entityID, new PositionComponent(newPos.x, newPos.y, 0));
            ECSManager.AddComponent<VelocityComponent>(entityID, new VelocityComponent(velocity, Vector3.right.x, Vector3.right.y, Vector3.right.z));
            entities.Add(entityID);
        }

        public void CreateCaravan(float velocity = DEFAULT_VELOCITY, Vector3 newPos = default(Vector3))
        {
            CreateAgent(velocity, newPos);
            caravanCount++;
        }
        public void CreateVillager(float velocity = DEFAULT_VELOCITY, Vector3 newPos = default(Vector3))
        {
            CreateAgent(velocity, newPos);
            villagerCount++;
        }

        private void FillDrawMatrix(List<Matrix4x4[]> drawMatrix, int entityCount)
        {
            for (int i = 0; i < entityCount; i += MAX_OBJS_PER_DRAWCALL)
            {
                drawMatrix.Add(new Matrix4x4[entityCount > MAX_OBJS_PER_DRAWCALL ? MAX_OBJS_PER_DRAWCALL : entityCount]);
                entityCount -= MAX_OBJS_PER_DRAWCALL;
            }
        }

        private void SetTRS(List<Matrix4x4[]> drawMatrix, Vector3 scale, int entityStartIndex, int entityCount)
        {
            Parallel.For(entityStartIndex, entityStartIndex + entityCount, i =>
            {
                PositionComponent position = ECSManager.GetComponent<PositionComponent>(entities[i]);

                int matrixIndex = (i - entityStartIndex) / MAX_OBJS_PER_DRAWCALL;
                int elementIndex = (i - entityStartIndex) % MAX_OBJS_PER_DRAWCALL;

                if (matrixIndex < drawMatrix.Count && elementIndex < drawMatrix[matrixIndex].Length)
                {
                    drawMatrix[matrixIndex][elementIndex].SetTRS(new Vector3(position.X, position.Y, position.Z), Quaternion.identity, scale);
                }
            });
        }

        private void DrawMeshes(int meshCount, Mesh mesh, Material material, List<Matrix4x4[]> drawMatrix, int exclusiveThreshold = 0)
        {
            for (int i = 0; i < meshCount && i + exclusiveThreshold < drawMatrix.Count; i++)
            {   //Con esa condición me aseguro de no pasarme de lo que tiene drawMatrix adentro

                Graphics.DrawMeshInstanced(mesh, 0, material, drawMatrix[i + exclusiveThreshold]);
            }
        }
    }
}