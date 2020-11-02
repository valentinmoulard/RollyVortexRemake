using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static Action<int> OnCurrencyBroadcasted;
    public static Action<int> OnCurrencyGainedBroadcasted;

    private int m_currencyHeld;

    private void Awake()
    {
        FetchCurrencyFromSave();
    }

    private void OnEnable()
    {
        CollectibleCurrency.OnCollectibleCurrencyCollected += IncreaseCurrency;
        DailyRewardManager.OnRewardClaimed += IncreaseCurrency;

        UIManager.OnGameOverUI += BroadcastOnCurrencyBroadcasted;
        UIManager.OnInGameUI += BroadcastOnCurrencyBroadcasted;
        UIManager.OnMainMenuUI += BroadcastOnCurrencyBroadcasted;
        ScoreManager.OnScoreValueSent += CalculateCurrencyGained;
    }

    private void OnDisable()
    {
        CollectibleCurrency.OnCollectibleCurrencyCollected -= IncreaseCurrency;
        DailyRewardManager.OnRewardClaimed -= IncreaseCurrency;

        UIManager.OnGameOverUI -= BroadcastOnCurrencyBroadcasted;
        UIManager.OnInGameUI -= BroadcastOnCurrencyBroadcasted;
        UIManager.OnMainMenuUI -= BroadcastOnCurrencyBroadcasted;
        ScoreManager.OnScoreValueSent -= CalculateCurrencyGained;
    }

    private void CalculateCurrencyGained(int sessionScore)
    {
        IncreaseCurrency(sessionScore / 100);
        BroadcastOnCurrencyGainedBroadcasted(sessionScore / 100);
    }

    /// <summary>
    /// Curretly using player prefs to save variables
    /// </summary>
    private void FetchCurrencyFromSave()
    {
        m_currencyHeld = PlayerPrefs.GetInt("Currency", 0);
    }

    private void IncreaseCurrency(int amount)
    {
        m_currencyHeld += amount;
        PlayerPrefs.SetInt("Currency", m_currencyHeld);
        BroadcastOnCurrencyBroadcasted();
    }

    /// <summary>
    /// method used in future developement
    /// </summary>
    /// <param name="amount"></param>
    private void DecreaseCurrency(int amount)
    {
        m_currencyHeld -= amount;
        BroadcastOnCurrencyBroadcasted();
    }

    /// <summary>
    /// method used in future developement
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private bool HasEnoughCurrency(float amount)
    {
        return (m_currencyHeld >= amount);
    }

    private void BroadcastOnCurrencyBroadcasted()
    {
        if (OnCurrencyBroadcasted != null)
            OnCurrencyBroadcasted(m_currencyHeld);
    }

    private void BroadcastOnCurrencyGainedBroadcasted(int currencyGained)
    {
        if (OnCurrencyGainedBroadcasted != null)
            OnCurrencyGainedBroadcasted(currencyGained);
    }
}
