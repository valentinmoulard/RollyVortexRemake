using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyReward", menuName = "Data/DailyReward")]
public class DailyRewardScriptableObject : ScriptableObject {

    
    public List<int> m_dailyRewardList;
}
