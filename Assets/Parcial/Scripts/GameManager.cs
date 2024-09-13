using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool isPaused;
    private static float mapWidth;
    private static float nodeSeparation;
    public static void ResetSimulation()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        PauseGame();
    }
    public static void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0.0f;
    }
    public static void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
    }

    public static void SetMapWidth(float newMapWidth) => mapWidth = newMapWidth;
    public static void SetMapHeight(float newMapHeight) => mapWidth = newMapHeight;
    public static void SetNodeSeparation(float newNodeSeparation) => nodeSeparation = newNodeSeparation;
}
