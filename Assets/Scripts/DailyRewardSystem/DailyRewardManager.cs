using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardManager : MonoBehaviour
{
    public static Action OnShowDailyRewardUI;
    public static Action OnHideDailyRewardUI;

    public static Action<int> OnRewardClaimed;
    public static Action<int> OnCurrentDaySent;
    public static Action<DailyRewardScriptableObject> OnDailyRewardSOSent;

    [SerializeField, Tooltip("A scriptable object that contains the amount of currency distributed as daily reaward for each day.")]
    private DailyRewardScriptableObject m_dailyRewardScriptableObject = null;

    private DateTime m_currentDate;
    private DateTime m_oldDateBuffer;
    private long m_oldTimeBuffer;
    private int m_currentDay;

    //debug variables
    TimeSpan m_timeSpanBuffer;
    DateTime m_dateTimeBuffer;

    private void Start()
    {
        if (m_dailyRewardScriptableObject == null)
            Debug.LogError("No daily reward scriptable object referenced in the daily reward manager!", gameObject);

        m_timeSpanBuffer = new TimeSpan(2, 0, 0, 0);
        CheckDailyRewardValidity();
    }

    private void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.V))
            TimeCheatDailyReward();
#endif
    }

    public void TimeCheatDailyReward()
    {
        m_dateTimeBuffer = DateTime.Now;
        m_dateTimeBuffer = m_dateTimeBuffer.Subtract(m_timeSpanBuffer);
        PlayerPrefs.SetString("oldTime", m_dateTimeBuffer.ToBinary().ToString());
        CheckDailyRewardValidity();
        Debug.Log("Cheat used!");
    }

    private void CheckDailyRewardValidity()
    {
        m_currentDate = DateTime.Now;

        //If it's the first time the player starts the game, the player will receive the first daily reward
        //If it's not the case, the game will check when was the last game session, and will determine if a day or more has passed
        if (PlayerPrefs.HasKey("oldTime"))
        {
            m_oldTimeBuffer = Convert.ToInt64(PlayerPrefs.GetString("oldTime"));
            m_oldDateBuffer = DateTime.FromBinary(m_oldTimeBuffer);

            TimeSpan difference = m_currentDate.Subtract(m_oldDateBuffer);
            if (difference.Days >= 1)
            {
                m_currentDay = PlayerPrefs.GetInt("CurrentDay");
                m_currentDay++;
                if (m_currentDay >= 7)
                    m_currentDay = 0;

                ShowDailyRewardView();
            }
        }
        else
        {
            m_currentDay = 0;
            PlayerPrefs.SetInt("CurrentDay", m_currentDay);
            ShowDailyRewardView();
            //PlayerPrefs.SetString("oldTime", DateTime.Now.ToBinary().ToString());
        }
    }

    private void ShowDailyRewardView()
    {
        BroadcastOnShowDailyRewardUI();
        BroadcastOnCurrentDaySent();
        BroadcastOnDailyRewardSOSent();
    }


    public void ClaimReward()
    {
        BroadcastOnRewardClaimed(m_dailyRewardScriptableObject.m_dailyRewardList[m_currentDay]);
        PlayerPrefs.SetInt("CurrentDay", m_currentDay);
        PlayerPrefs.SetString("oldTime", System.DateTime.Now.ToBinary().ToString());
        BroadcastOnHideDailyRewardUI();
    }

    #region Broadcasters
    private void BroadcastOnShowDailyRewardUI()
    {
        if (OnShowDailyRewardUI != null)
            OnShowDailyRewardUI();
    }

    private void BroadcastOnHideDailyRewardUI()
    {
        if (OnHideDailyRewardUI != null)
            OnHideDailyRewardUI();
    }

    private void BroadcastOnRewardClaimed(int softCurrencyGained)
    {
        if (OnRewardClaimed != null)
            OnRewardClaimed(softCurrencyGained);
    }

    private void BroadcastOnCurrentDaySent()
    {
        if (OnCurrentDaySent != null)
            OnCurrentDaySent(m_currentDay);
    }

    private void BroadcastOnDailyRewardSOSent()
    {
        if (OnDailyRewardSOSent != null)
            OnDailyRewardSOSent(m_dailyRewardScriptableObject);
    }
    #endregion
}
