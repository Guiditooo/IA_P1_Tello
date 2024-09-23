using FlyEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace FlyEngine
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameObject mountainPrefab;
        [SerializeField] private GameObject streamPrefab;
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private GameObject grassPrefab;

        [SerializeField] private int mapWidth = 10;
        [SerializeField] private int mapHeight = 10;

        private const int MAX_OBJS_PER_DRAWCALL = 1000;

        private Mesh quadMesh;
        private Material tileMaterial;

        private List<uint> tileEntities;

        private Dictionary<TileType, SpriteRenderer> tileSprites;

        private int tileCount = 0;
        private Vector3 tileScale;

        private bool active = false;
        private bool createdMap = false;

        void Start()
        {
            // Iniciar el sistema ECS si es necesario
            ECSManager.Init();
            tileEntities = new List<uint>();

            // Inicializar el diccionario de sprites para los diferentes tipos de tile
            tileSprites = new Dictionary<TileType, SpriteRenderer>()
            {
                { TileType.Mountain, mountainPrefab.GetComponent<SpriteRenderer>() },
                { TileType.Stream, streamPrefab.GetComponent < SpriteRenderer >() },
                { TileType.Road, roadPrefab.GetComponent < SpriteRenderer >() },
                { TileType.Grass, grassPrefab.GetComponent<SpriteRenderer>() }
            };


            quadMesh = GenerateQuad(); // Generar un quad para representar cada tile
            tileMaterial = Resources.Load<Material>("Materials/InstancedTileMaterial");

        }

        void LateUpdate()
        {
            if (!active || !createdMap)
                return;

            List<Matrix4x4[]> tileDrawMatrix = new List<Matrix4x4[]>();

            FillDrawMatrix(tileDrawMatrix, tileCount);
            SetTRS(tileDrawMatrix, tileScale, 0, tileCount);

            DrawTiles(quadMesh, tileMaterial, tileDrawMatrix);
        }

        public void SetActive(bool isActive)
        {
            active = isActive;
            Debug.Log(active ? "Map is on" : "Map is off");

        }

        public void SetUp(int width, int height, float tileWidth, float tileHeight)
        {
            if (createdMap)
                return;

            tileScale = new Vector3(tileWidth, tileHeight, 1.0f);
            //tileScale = new Vector3(tileWidth / width, tileHeight / height, 1.0f);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileType tileType = (TileType)Random.Range(0, 4);

                    CreateTile(tileType, new Vector3(0.5f + x * tileScale.x, 0.5f + y * tileScale.y, 0), x, y); // Asigna tipo de tile según prefieras
                }
            }

            createdMap = true;
            Debug.Log("Map Has Been Created!");


        }

        private void CreateTile(TileType type, Vector3 position, int gridX, int gridY)
        {
            uint entityID = ECSManager.CreateEntity();

            ECSManager.AddComponent<PositionComponent>(entityID, new PositionComponent(position.x, position.y, 0));
            ECSManager.AddComponent<TileTypeComponent>(entityID, new TileTypeComponent(type));
            ECSManager.AddComponent<ColorComponent>(entityID, new ColorComponent(GetColorComponent(type)));
            ECSManager.AddComponent<GridComponet>(entityID, new GridComponet(gridX, gridY));

            tileEntities.Add(entityID);
            tileCount++;

            Debug.Log($"Tile Created: Type={type}, Position=({position.x}, {position.y}), Size=({tileScale.x}, {tileScale.y})");
        }


        private void FillDrawMatrix(List<Matrix4x4[]> drawMatrix, int entityCount)
        {
            for (int i = 0; i < entityCount; i += MAX_OBJS_PER_DRAWCALL)
            {
                drawMatrix.Add(new Matrix4x4[entityCount > MAX_OBJS_PER_DRAWCALL ? MAX_OBJS_PER_DRAWCALL : entityCount]);
                entityCount -= MAX_OBJS_PER_DRAWCALL;
            }

            /*
            Debug.Log($"drawMatrix Count: {drawMatrix.Count}");
            foreach (var matrixArray in drawMatrix)
            {
                Debug.Log($"Matrix Array Length: {matrixArray.Length}");

                foreach (var matrix in matrixArray)
                {
                    Debug.Log($"Matrix Position: {matrix.GetColumn(3)}"); // Muestra la posición
                }
            }
            */

        }


        private void SetTRS(List<Matrix4x4[]> drawMatrix, Vector3 scale, int entityStartIndex, int entityCount)
        {
            Parallel.For(entityStartIndex, entityStartIndex + entityCount, i =>
            {
                PositionComponent position = ECSManager.GetComponent<PositionComponent>(tileEntities[i]);

                int matrixIndex = (i - entityStartIndex) / MAX_OBJS_PER_DRAWCALL;
                int elementIndex = (i - entityStartIndex) % MAX_OBJS_PER_DRAWCALL;

                if (matrixIndex < drawMatrix.Count && elementIndex < drawMatrix[matrixIndex].Length)
                {
                    var tileMatrix = new Matrix4x4();
                    tileMatrix.SetTRS(new Vector3(position.X, position.Y, position.Z), Quaternion.identity, scale);

                    drawMatrix[matrixIndex][elementIndex] = tileMatrix;

                    // Log para verificar la matriz
                    //Debug.Log($"Matrix for Tile {i}: Position=({position.X}, {position.Y}), Scale=({scale.x}, {scale.y})");
                }
            });
        }

        private void DrawTiles(Mesh quad, Material material, List<Matrix4x4[]> drawMatrix)
        {

            for (int i = 0; i < drawMatrix.Count; i++)
            {
                for (int j = 0; j < drawMatrix[i].Length; j++)
                {
                    int tileIndex = (i * MAX_OBJS_PER_DRAWCALL) + j;

                    MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
                    // Obtén el color correspondiente al tile
                    ColorComponent color = ECSManager.GetComponent<ColorComponent>(tileEntities[tileIndex]);
                    Color col = GetColor(color);
                    TileType type = ECSManager.GetComponent<TileTypeComponent>(tileEntities[tileIndex]).tileType;
                    // Asegúrate de que estás estableciendo el color correctamente
                    propBlock.SetColor("_Color", col);

                    // Dibujar el tile con el material y el color establecido
                    //Graphics.DrawMeshInstanced(quad, 0, material, drawMatrix[i], drawMatrix[i].Length, propBlock);

                    Graphics.DrawMesh(quad, drawMatrix[i][j], material, 0, null, 0, propBlock);

                    Debug.Log($"Dibujando tile {tileIndex}, de tipo {type} con color {col}");
                }
            }
        }

        private Mesh GenerateQuad()
        {
            // Crear un quad simple que actuará como contenedor para los sprites
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[]
            {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0)
            };
            mesh.uv = new Vector2[]
            {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
            };
            mesh.triangles = new int[]
            {
            0, 1, 2,
            2, 3, 0
            };
            return mesh;
        }

        private ColorComponent GetColorComponent(TileType tileType)
        {
            tileSprites.TryGetValue(tileType, out SpriteRenderer sr);
            return new ColorComponent(sr.color.r, sr.color.g, sr.color.b, sr.color.a);
        }
        private ColorComponent GetColorComponent(Color color)
        {
            return new ColorComponent(color.r, color.g, color.b, color.a);
        }

        private Color GetColor(TileType tileType)
        {
            tileSprites.TryGetValue(tileType, out SpriteRenderer sr);
            return new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a);
        }
        private Color GetColor(ColorComponent color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }

        /*void OnDrawGizmos()
        {
            if (!createdMap) return;

            foreach (var entityID in tileEntities)
            {
                PositionComponent pos = ECSManager.GetComponent<PositionComponent>(entityID);
                Gizmos.color = Color.red; // Cambia el color si lo deseas
                Gizmos.DrawCube(new Vector3(pos.X, pos.Y, 0), new Vector3(tileScale.x, tileScale.y, 0.1f));
            }
        }
        */
    }

}