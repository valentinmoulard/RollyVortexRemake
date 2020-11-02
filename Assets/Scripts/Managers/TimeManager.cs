using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static Action<float> OnGameSessionSent;

    private float m_gameSessionTime;
    private bool m_isTimerActive;

    public static TimeManager instance = null;

    public float GameSessionTime { get => m_gameSessionTime; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += ActivateTimer;
        GameManager.OnGameOver += StopGameTimer;
        GameManager.OnRetry += StopGameTimer;
        GameManager.OnPlayerRevived += ActivateTimer;
        GameManager.OnTitleScreen += ResetTimer;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= ActivateTimer;
        GameManager.OnGameOver -= StopGameTimer;
        GameManager.OnRetry -= StopGameTimer;
        GameManager.OnPlayerRevived -= ActivateTimer;
        GameManager.OnTitleScreen -= ResetTimer;
    }

    private void Start()
    {
        m_isTimerActive = false;
    }

    private void Update()
    {
        if (m_isTimerActive)
            m_gameSessionTime += Time.deltaTime;
    }

    private void ResetTimer()
    {
        m_gameSessionTime = 0;
    }

    private void ActivateTimer()
    {
        m_isTimerActive = true;
    }

    private void StopGameTimer()
    {
        m_isTimerActive = false;
        BroadcastOnGameSessionSent();
    }

    private void BroadcastOnGameSessionSent()
    {
        if (OnGameSessionSent != null)
            OnGameSessionSent(m_gameSessionTime);
    }

}
