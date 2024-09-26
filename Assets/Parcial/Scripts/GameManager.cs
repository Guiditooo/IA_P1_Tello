using UnityEngine;

namespace FlyEngine
{
    public class GameManager : MonoBehaviour
    {
        private static MapManager map;

        private static GrapfView viewer;

        private static bool isPaused;
        private static float mapWidth;
        private static float mapHeight;
        private static float nodeSeparation;

        private void Awake()
        {
            ECSManager.Init();
            viewer = GetComponent<GrapfView>();
            map = GetComponent<MapManager>();
        }

        public static void StartSimulation()
        {
            Vector2Int nodeCount = new Vector2Int();
            nodeCount.x = (int)(mapWidth / nodeSeparation) + 1; // El +1 es por el 0,0
            nodeCount.y = (int)(mapHeight / nodeSeparation) + 1;

            Vector2 tileSize = new Vector2();
            tileSize.x = mapWidth / (nodeCount.x - 1);
            tileSize.y = mapHeight / (nodeCount.y - 1);

            if (viewer != null)
            {
                viewer.Initialize(nodeCount.x, nodeCount.y, tileSize.x);
            }

            Camera.main.transform.position = new Vector3(mapWidth / 2, mapHeight / 2, -10);

            map.SetUp(nodeCount.x, nodeCount.y, tileSize.x, tileSize.y);

            ResumeGame();
        }
        public static void ResetSimulation()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            PauseGame();
        }
        public static void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0.0f;
            viewer.SetActive(false);
            map.SetActive(false);
        }
        public static void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            viewer.SetActive(true);
            map.SetActive(true);
        }

        public static void SetMapWidth(float newMapWidth) => mapWidth = newMapWidth;
        public static void SetMapHeight(float newMapHeight) => mapHeight = newMapHeight;
        public static void SetNodeSeparation(float newNodeSeparation) => nodeSeparation = newNodeSeparation;


    }

}