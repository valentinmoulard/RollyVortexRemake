using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the text mesh pro component.")]
    private TMP_Text m_currencyText = null;

    private void OnEnable()
    {
        CurrencyManager.OnCurrencyBroadcasted += UpdateCurrencyUI;
    }

    private void OnDisable()
    {
        CurrencyManager.OnCurrencyBroadcasted -= UpdateCurrencyUI;
    }

    private void Start()
    {
        if (m_currencyText == null)
            Debug.LogError("TMP text component reference is missing!", gameObject);
    }

    private void UpdateCurrencyUI(int value)
    {
        m_currencyText.text = value.ToString();
    }
}
