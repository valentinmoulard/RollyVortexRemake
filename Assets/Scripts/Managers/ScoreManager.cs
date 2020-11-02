using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static Action<int> OnScoreValueSent;
    public static Action<int> OnHighScoreValueSent;
    public static Action OnBonusThresholdTriggered;

    [SerializeField, Tooltip("For each x obstacle elements avoided, grants a bonus to the player.")]
    private int m_bonusTriggerThreshold = 100;

    private int m_countBuffer;
    private int m_currentScore;
    private int m_highScore;
    private bool m_canIncreaseScore;

    private void Awake()
    {
        m_highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void OnEnable()
    {
        //gameflow
        GameManager.OnStartGame += ResetScore;
        GameManager.OnGameOver += StopScoring;

        //gameplay
        ObstacleElement.OnObstacleElementPassed += IncreaseScore;
        ObstacleElement.OnObstacleElementPassed += IncreaseCombo;

        //UI Update
        UIManager.OnInGameUI += BroadcastOnScoreValueSent;
        UIManager.OnGameOverUI += BroadcastOnScoreValueSent;
        UIManager.OnGameOverUI += BroadcastOnHighScoreValueSent;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= ResetScore;
        GameManager.OnGameOver -= StopScoring;
        ObstacleElement.OnObstacleElementPassed -= IncreaseScore;
        ObstacleElement.OnObstacleElementPassed -= IncreaseCombo;

        UIManager.OnInGameUI -= BroadcastOnScoreValueSent;
        UIManager.OnGameOverUI -= BroadcastOnScoreValueSent;
        UIManager.OnGameOverUI -= BroadcastOnHighScoreValueSent;
    }


    private void ResetScore()
    {
        m_currentScore = 0;
        m_countBuffer = 1;
        m_canIncreaseScore = true;
        BroadcastOnScoreValueSent();
    }

    private void IncreaseScore(int scoreValue)
    {
        if (m_canIncreaseScore)
        {
            m_currentScore += scoreValue;
            BroadcastOnScoreValueSent();
        }
    }

    /// <summary>
    /// The tracking of the combo is used to trigger the player's power when crossing a threshold.
    /// </summary>
    /// <param name="amount"></param>
    private void IncreaseCombo(int amount)
    {
        m_countBuffer += amount;
        if (m_countBuffer >= m_bonusTriggerThreshold)
        {
            m_countBuffer -= m_bonusTriggerThreshold;
            BroadcastOnBonusThresholdTriggered();
        }
    }

    private void StopScoring()
    {
        m_canIncreaseScore = false;
        if (m_currentScore > m_highScore)
        {
            m_highScore = m_currentScore;
            PlayerPrefs.SetInt("HighScore", m_highScore);
        }
        BroadcastOnScoreValueSent();
        BroadcastOnHighScoreValueSent();
    }

    #region Broadcasters
    private void BroadcastOnScoreValueSent()
    {
        if (OnScoreValueSent != null)
            OnScoreValueSent(m_currentScore);
    }

    private void BroadcastOnHighScoreValueSent()
    {
        if (OnHighScoreValueSent != null)
            OnHighScoreValueSent(m_highScore);
    }

    private void BroadcastOnBonusThresholdTriggered()
    {
        if (OnBonusThresholdTriggered != null)
            OnBonusThresholdTriggered();
    }
    #endregion
}
