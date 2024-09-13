#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class QoL : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Por ejemplo, presionar 'Escape' para detener la simulación
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
