using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the text mesh pro component.")]
    private TMP_Text m_highScoreText = null;

    private void OnEnable()
    {
        ScoreManager.OnHighScoreValueSent += UpdateHighScoreText;
    }

    private void OnDisable()
    {
        ScoreManager.OnHighScoreValueSent -= UpdateHighScoreText;
    }

    private void Start()
    {
        if (m_highScoreText == null)
            Debug.LogError("The text component is missing in the inspector!", gameObject);
    }

    private void UpdateHighScoreText(int highScoreValue)
    {
        m_highScoreText.text = highScoreValue.ToString();
    }
}
