using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button btn_SaveConfiguration;
    [SerializeField] private Button btn_StartSimulation;
    [SerializeField] private Button btn_RestartSimulation;
    [SerializeField] private Button btn_Pause;

    [SerializeField] private Slider slider_MapWidth;
    [SerializeField] private Slider slider_MapHeight;
    [SerializeField] private Slider slider_NodeSeparation;

    [SerializeField] private TMP_InputField txt_MapWidth;
    [SerializeField] private TMP_InputField txt_MapHeight;
    [SerializeField] private TMP_InputField txt_NodeSeparation;

    [SerializeField] private CanvasGroup panel_Configuration;
    [SerializeField] private CanvasGroup panel_Overlay;
    [SerializeField] private CanvasGroup panel_Pause;

    private const float INITIAL_DIM = 10;
    private const float INITIAL_SEP = 1;

    private const float MIN_DIM = 5;
    private const float MAX_DIM = 20;

    private const float MIN_SEP = 1;
    private const float MAX_SEP = 3;

    private float mapWidthValue = INITIAL_DIM;
    private float mapHeightValue = INITIAL_DIM;
    private float nodeSeparationValue = INITIAL_SEP;
    private void Awake()
    {
        btn_SaveConfiguration.onClick.AddListener(SaveConfiguration);
        btn_StartSimulation.onClick.AddListener(StartSimulation);
        btn_RestartSimulation.onClick.AddListener(ResetSimulation);
        btn_Pause.onClick.AddListener(PauseSimulation);

        slider_MapWidth.onValueChanged.AddListener(SetMapWidth);
        slider_MapHeight.onValueChanged.AddListener(SetMapHeight);
        slider_NodeSeparation.onValueChanged.AddListener(SetNodeSeparation);

        txt_MapHeight.onValueChanged.AddListener(CheckHeightInput);
        txt_MapWidth.onValueChanged.AddListener(CheckWidthInput);
        txt_NodeSeparation.onValueChanged.AddListener(CheckNodeSeparationInput);

        txt_MapWidth.text = mapWidthValue.ToString();
        txt_MapHeight.text = mapHeightValue.ToString();
        txt_NodeSeparation.text = nodeSeparationValue.ToString();
    }

    private void Start()
    {
        HidePanel(panel_Overlay);
        HidePanel(panel_Pause);
        ShowPanel(panel_Configuration);
    }
    private void OnDestroy()
    {
        btn_SaveConfiguration.onClick.RemoveAllListeners();
        btn_StartSimulation.onClick.RemoveAllListeners();
        btn_RestartSimulation.onClick.RemoveAllListeners();
        btn_Pause.onClick.RemoveAllListeners();

        slider_MapWidth.onValueChanged.RemoveAllListeners();
        slider_MapHeight.onValueChanged.RemoveAllListeners();
        slider_NodeSeparation.onValueChanged.RemoveAllListeners();

        txt_MapWidth.onValueChanged.RemoveAllListeners();
        txt_MapHeight.onValueChanged.RemoveAllListeners();
        txt_NodeSeparation.onValueChanged.RemoveAllListeners();
    }

    #region PANEL_REGION

    private void HidePanel(CanvasGroup panel)
    {
        panel.alpha = 0.0f;
        panel.blocksRaycasts = false;
        panel.interactable = false;
    }
    private void ShowPanel(CanvasGroup panel)
    {
        panel.alpha = 1.0f;
        panel.blocksRaycasts = true;
        panel.interactable = true;
    }

    private void SaveConfiguration()
    {
        GameManager.SetMapHeight(mapHeightValue);
        GameManager.SetMapWidth(mapWidthValue);
        GameManager.SetNodeSeparation(nodeSeparationValue);
        HidePanel(panel_Configuration);
        ShowPanel(panel_Pause);
    }

    private void StartSimulation()
    {
        GameManager.ResumeGame();
        HidePanel(panel_Pause);
        ShowPanel(panel_Overlay);
    }

    private void ResetSimulation()
    {
        GameManager.ResetSimulation();
    }

    private void PauseSimulation()
    {
        GameManager.PauseGame();
        HidePanel(panel_Overlay);
        ShowPanel(panel_Pause);
    }

    #endregion

    #region SLIDER_REGION

    private void SetMapWidth(float mapWidth)
    {
        txt_MapWidth.text = mapWidth.ToString("F3");
        mapWidthValue = mapWidth;
    }
    private void SetMapHeight(float mapHeight)
    {
        txt_MapHeight.text = mapHeight.ToString("F3");
        mapHeightValue = mapHeight;
    }
    private void SetNodeSeparation(float nodeSeparation)
    {
        txt_NodeSeparation.text = nodeSeparation.ToString("F3");
        nodeSeparationValue = nodeSeparation;
    }

    #endregion

    #region INPUT_REGION

    private void CheckWidthInput(string input)
    {
        mapWidthValue = ClampValues(float.Parse(input), MIN_DIM, MAX_DIM);
        txt_MapWidth.text = mapWidthValue.ToString("F3");
    }
    private void CheckHeightInput(string input)
    {
        mapHeightValue = ClampValues(float.Parse(input), MIN_DIM, MAX_DIM);
        txt_MapHeight.text = mapHeightValue.ToString("F3");
    }
    private void CheckNodeSeparationInput(string input)
    {
        nodeSeparationValue = ClampValues(float.Parse(input), MIN_SEP, MAX_SEP);
        txt_NodeSeparation.text = nodeSeparationValue.ToString("F3");
    }
    private float ClampValues(float value, float minValue, float maxValue)
    {
        if (value < minValue)
        {
            value = minValue;
        }
        else if (value > maxValue)
        {
            value = maxValue;
        }
        return value;
    }

    #endregion

}