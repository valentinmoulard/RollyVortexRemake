using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnTitleScreen;
    public static Action OnSkinScreen;
    public static Action OnOptions;
    public static Action OnStartGame;
    public static Action OnGameOver;
    public static Action OnRetry;
    public static Action OnPlayerRevived;

    public static Action<Vector3> OnStartPositionSent;
    public static Action<float> OnLevelRadiusSent;


    public const float z_POSITION_POOL_RETURNING_CONDITION = -10f;
    public float Z_POSITION_POOL_RETURNING_CONDITION => z_POSITION_POOL_RETURNING_CONDITION;
    public GameState CurrentGameState { get => m_currentGameState; }


    [Header("Level Parameters")]
    [SerializeField]
    private GameObject m_levelStartPosition = null;
    [SerializeField]
    private float m_levelRadius = 5f;


    private GameState m_currentGameState;
    private bool m_hasAlreadyRevied;

    public static GameManager instance = null;


    public enum GameState
    {
        InMainMenu,
        InSkinMenu,
        InGame,
        InGameOver
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Controller.OnTapBegin += StartGame;
        PlayerController.OnPlayerDead += Retry;
        RetryCountDownUI.OnCountdownOver += GameOver;
    }

    private void OnDisable()
    {
        Controller.OnTapBegin -= StartGame;
        PlayerController.OnPlayerDead -= Retry;
        RetryCountDownUI.OnCountdownOver -= GameOver;
    }

    private void Start()
    {
        if (m_levelStartPosition == null)
            Debug.LogError("The gameobject used for the start position is missing!", gameObject);

        m_currentGameState = GameState.InMainMenu;

        BroadcastOnLevelRadiusSent();
        ToMainMenu();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs Cleared");
        }
#endif
    }

    public void StartGame(Vector3 cursorPosition)
    {
        if (m_currentGameState == GameState.InMainMenu)
            BroadcastStartGame();
    }

    private void Retry()
    {
        if (m_hasAlreadyRevied)
        {
            GameOver();
        }
        else
        {
            BroadcastOnRetry();
        }
    }

    private void GameOver()
    {
        BroadcastOnGameOver();
    }

    public void ToMainMenu()
    {
        m_hasAlreadyRevied = false;

        //Don't change the order of the 2 following lines
        //some elements in the game need the start position information before executing the functions related to the event OnTitleScreen
        BroadcastOnStartPosition();
        BroadcastOnTitleScreen();
    }

    public void ToSkinSelction()
    {
        BroadcastOnSkinScreen();
    }

    public void ToOptions()
    {
        BroadcastOnOptions();
    }

    //called by button
    public void RevivePlayer()
    {
        m_hasAlreadyRevied = true;

        BroadcastOnPlayerRevived();
    }

    #region Broadcasters
    private void BroadcastOnTitleScreen()
    {
        m_currentGameState = GameState.InMainMenu;
        if (OnTitleScreen != null)
            OnTitleScreen();
    }

    private void BroadcastStartGame()
    {
        m_currentGameState = GameState.InGame;
        if (OnStartGame != null)
            OnStartGame();
    }

    private void BroadcastOnGameOver()
    {
        m_currentGameState = GameState.InGameOver;

        if (OnGameOver != null)
            OnGameOver();
    }

    private void BroadcastOnSkinScreen()
    {
        m_currentGameState = GameState.InSkinMenu;
        if (OnSkinScreen != null)
            OnSkinScreen();
    }

    private void BroadcastOnStartPosition()
    {
        if (OnStartPositionSent != null)
            OnStartPositionSent(m_levelStartPosition.transform.position);
    }

    private void BroadcastOnLevelRadiusSent()
    {
        if (OnLevelRadiusSent != null)
            OnLevelRadiusSent(m_levelRadius);
    }

    private void BroadcastOnPlayerRevived()
    {
        if (OnPlayerRevived != null)
            OnPlayerRevived();
    }

    private void BroadcastOnRetry()
    {
        if (OnRetry != null)
            OnRetry();
    }

    private void BroadcastOnOptions()
    {
        if (OnOptions != null)
            OnOptions();
    }

    #endregion
}
