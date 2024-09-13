using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    private bool alarmOn = false;
    private Button btn;
    private TMP_Text valueText = null;

    public static Action<bool> OnAlarmToggle;

    private void Awake()
    {
        btn = GetComponentInParent<Button>();
        valueText = GetComponent<TMP_Text>();
        btn.onClick.AddListener(ToggleAlarmState);
    }
    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    private void ToggleAlarmState()
    {
        alarmOn = !alarmOn;
        valueText.text = alarmOn ? "ON" : "OFF";
        OnAlarmToggle?.Invoke(alarmOn);
    }
}
