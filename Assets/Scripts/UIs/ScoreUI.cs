using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the text mesh pro component.")]
    private TMP_Text m_scoreText = null;

    private void OnEnable()
    {
        ScoreManager.OnScoreValueSent += UpdateScoreText;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreValueSent -= UpdateScoreText;
    }

    private void Start()
    {
        if (m_scoreText == null)
            Debug.LogError("The text component is missing in the inspector!", gameObject);
    }

    private void UpdateScoreText(int scoreValue)
    {
        m_scoreText.text = scoreValue.ToString();
    }
}
