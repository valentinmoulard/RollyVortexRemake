using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_timerText = null;

    private void Start()
    {
        if (m_timerText == null)
            Debug.LogError("The text component is missing in the inspector!", gameObject);
    }

    private void Update()
    {
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        m_timerText.text = TimeManager.instance.GameSessionTime.ToString("F2");
    }
}
