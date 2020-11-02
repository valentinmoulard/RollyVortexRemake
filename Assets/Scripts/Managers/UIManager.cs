using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static Action OnMainMenuUI;
    public static Action OnInGameUI;
    public static Action OnGameOverUI;
    public static Action OnRetryUI;
    public static Action OnSkinSelectionUI;
    public static Action OnOptionUI;

    [SerializeField]
    private GameObject m_mainMenuUI = null;

    [SerializeField]
    private GameObject m_inGameUI = null;

    [SerializeField]
    private GameObject m_gameOverUI = null;

    [SerializeField]
    private GameObject m_retryUI = null;

    [SerializeField]
    private GameObject m_skinSelectionUI = null;

    [SerializeField]
    private GameObject m_optionsUI = null;

    [SerializeField]
    private GameObject m_dailyRewardUI = null;

    private void OnEnable()
    {
        GameManager.OnStartGame += ShowInGameUI;
        GameManager.OnGameOver += ShowGameOverUI;
        GameManager.OnRetry += ShowRetryUI;
        GameManager.OnPlayerRevived += ShowInGameUI;
        GameManager.OnTitleScreen += ShowMainMenuUI;
        GameManager.OnSkinScreen += ShowSkinSelectionUI;
        GameManager.OnOptions += ShowOptionUI;

        DailyRewardManager.OnShowDailyRewardUI += ShowDailyRewardUI;
        DailyRewardManager.OnHideDailyRewardUI += HideDailyRewardUI;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= ShowInGameUI;
        GameManager.OnGameOver -= ShowGameOverUI;
        GameManager.OnRetry -= ShowRetryUI;
        GameManager.OnPlayerRevived -= ShowInGameUI;
        GameManager.OnTitleScreen -= ShowMainMenuUI;
        GameManager.OnSkinScreen -= ShowSkinSelectionUI;
        GameManager.OnOptions -= ShowOptionUI;

        DailyRewardManager.OnShowDailyRewardUI -= ShowDailyRewardUI;
        DailyRewardManager.OnHideDailyRewardUI -= HideDailyRewardUI;
    }

    private void Start()
    {
        if (m_mainMenuUI == null)
            Debug.LogError("Main menu UI gameobject reference missing!", gameObject);
        if (m_inGameUI == null)
            Debug.LogError("In game UI gameobject reference missing!", gameObject);
        if (m_gameOverUI == null)
            Debug.LogError("Game over UI gameobject reference missing!", gameObject);
        if (m_retryUI == null)
            Debug.LogError("Retry UI gameobject reference missing!", gameObject);
        if (m_skinSelectionUI == null)
            Debug.LogError("Skin selection UI gameobject reference missing!", gameObject);
        if (m_optionsUI == null)
            Debug.LogError("Option UI gameobject reference missing!", gameObject);
        if (m_dailyRewardUI == null)
            Debug.LogError("Daily reawrd UI gameobject reference missing!", gameObject);

    }

    private void HideDailyRewardUI()
    {
        m_dailyRewardUI.SetActive(false);
    }

    private void ShowDailyRewardUI()
    {
        m_dailyRewardUI.SetActive(true);
    }

    private void ShowSkinSelectionUI()
    {
        m_mainMenuUI.SetActive(false);
        m_skinSelectionUI.SetActive(true);
        if (OnSkinSelectionUI != null)
            OnSkinSelectionUI();
    }

    private void ShowMainMenuUI()
    {
        m_mainMenuUI.SetActive(true);
        m_inGameUI.SetActive(false);
        m_gameOverUI.SetActive(false);
        m_retryUI.SetActive(false);
        m_skinSelectionUI.SetActive(false);
        m_optionsUI.SetActive(false);
        if (OnMainMenuUI != null)
            OnMainMenuUI();
    }

    private void ShowInGameUI()
    {
        m_mainMenuUI.SetActive(false);
        m_retryUI.SetActive(false);
        m_inGameUI.SetActive(true);
        if (OnInGameUI != null)
            OnInGameUI();
    }

    private void ShowGameOverUI()
    {
        m_inGameUI.SetActive(false);
        m_retryUI.SetActive(false);
        m_gameOverUI.SetActive(true);
        if (OnGameOverUI != null)
            OnGameOverUI();
    }

    private void ShowRetryUI()
    {
        m_inGameUI.SetActive(false);
        m_retryUI.SetActive(true);
        if (OnRetryUI != null)
            OnRetryUI();
    }

    private void ShowOptionUI()
    {
        m_optionsUI.SetActive(true);
        if (OnOptionUI != null)
            OnOptionUI();
    }
}
