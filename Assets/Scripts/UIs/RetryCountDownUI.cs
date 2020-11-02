using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RetryCountDownUI : MonoBehaviour
{
    public static Action OnCountdownOver;

    [SerializeField, Tooltip("Reference to the text mesh pro component.")]
    private TMP_Text m_countdownText = null;

    private Coroutine m_countdownCoroutine;
    private float m_timerBuffer;

    private void OnEnable()
    {
        StartCountdown();
        GameManager.OnPlayerRevived += StopCountdown;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerRevived -= StopCountdown;
    }

    private void Start()
    {
        if (m_countdownText == null)
            Debug.LogError("The TMP text component is missing!", gameObject);
    }

    /// <summary>
    /// Method called when the players die. 
    /// Starts the countdown to let the player the choice to continue the game session or not. 
    /// At the end of the timer, brings the player to the gameover screen.
    /// </summary>
    private void StartCountdown()
    {
        if (m_countdownCoroutine != null)
            StopCoroutine(m_countdownCoroutine);

        m_countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    /// <summary>
    /// Method called when the player decides to continue the game session after one player's death.
    /// </summary>
    private void StopCountdown()
    {
        if (m_countdownCoroutine != null)
            StopCoroutine(m_countdownCoroutine);
    }

    /// <summary>
    /// Coroutine updating the countdown text.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountdownCoroutine()
    {
        m_timerBuffer = 5.0f;
        while (m_timerBuffer > 0.0f)
        {
            m_timerBuffer -= Time.deltaTime;
            UpdateText(m_timerBuffer);
            yield return new WaitForEndOfFrame();
        }
        BroadcastOnCountdownOver();
    }

    private void UpdateText(float countdown)
    {
        m_countdownText.text = countdown.ToString("F0");

    }

    private void BroadcastOnCountdownOver()
    {
        if (OnCountdownOver != null)
            OnCountdownOver();
    }
}
