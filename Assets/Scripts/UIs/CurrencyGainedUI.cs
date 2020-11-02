using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyGainedUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_currencyGainedText = null;


    private void OnEnable()
    {
        CurrencyManager.OnCurrencyGainedBroadcasted += UpdateCurrencyGainedText;
    }

    private void OnDisable()
    {
        CurrencyManager.OnCurrencyGainedBroadcasted -= UpdateCurrencyGainedText;
    }

    void Start()
    {
        if (m_currencyGainedText == null)
            Debug.LogError("The tmp text component is missing!", gameObject);
    }

    private void UpdateCurrencyGainedText(int currencyGained)
    {
        m_currencyGainedText.text = "+" + currencyGained.ToString();
    }
}
