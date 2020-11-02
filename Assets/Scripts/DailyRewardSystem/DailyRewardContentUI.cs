using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewardContentUI : MonoBehaviour {

    [SerializeField, Tooltip("The size of this list must be 7! (the 7 days of a week). The references of gameobjects containing sprites. The one corresponding to the current daily reward will be activated, while the other aren't.")]
    private List<GameObject> m_daysHighlightersList = null;

    [SerializeField, Tooltip("The size of this list must be 7! (the 7 days of a week). The reference of all the text fiels that displays the number of coin the player can get for each days.")]
    private List<TMP_Text> m_dailyRewardCurrencyText = null;

    private void OnEnable()
    {
        DailyRewardManager.OnCurrentDaySent += HighlightCurrentDailyReward;
        DailyRewardManager.OnDailyRewardSOSent += UpdateDailyRewardCurrencyTexts;
    }

    private void OnDisable()
    {
        DailyRewardManager.OnCurrentDaySent -= HighlightCurrentDailyReward;
        DailyRewardManager.OnDailyRewardSOSent -= UpdateDailyRewardCurrencyTexts;
    }

	private void Start ()
    {
        //refenrece check
        if (m_daysHighlightersList == null || m_daysHighlightersList.Count != 7)
            Debug.LogError("the days highlighters list is null or has missing elements!", gameObject);

        if (m_dailyRewardCurrencyText == null || m_dailyRewardCurrencyText.Count !=7)
            Debug.LogError("the daily reward currency texts list is null or has missing elements!", gameObject);
    }
	
    private void HighlightCurrentDailyReward(int currentDay)
    {
        for (int i = 0; i < m_daysHighlightersList.Count; i++)
        {
            m_daysHighlightersList[i].SetActive(false);
        }
        m_daysHighlightersList[currentDay].SetActive(true);
    }

    private void UpdateDailyRewardCurrencyTexts(DailyRewardScriptableObject dailyRewardsSO)
    {
        for (int i = 0; i < m_dailyRewardCurrencyText.Count; i++)
        {
            m_dailyRewardCurrencyText[i].text = dailyRewardsSO.m_dailyRewardList[i].ToString();
        }
    }

}
