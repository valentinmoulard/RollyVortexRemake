using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinButtonDisplayerUI : MonoBehaviour
{
    [SerializeField, Tooltip("List of the skin selection button references.")]
    private List<GameObject> m_skinButtons = null;
    [SerializeField, Tooltip("In pixels. Value that indicates how far from the center the buttons will appear.")]
    private float m_radiusDisplay = 100.0f;

    private Vector3 m_buttonPositionBuffer;
    private float m_angleStep;

    private void Start()
    {
        if (m_skinButtons == null || m_skinButtons.Count == 0)
            Debug.LogError("The skin list is empty or null!", gameObject);

        DisplaySkinButtons();
    }

    /// <summary>
    /// Method that displays the skin selection buttons on a circle.
    /// </summary>
    private void DisplaySkinButtons()
    {
        m_angleStep = 360f / m_skinButtons.Count;

        for (int i = 0; i < m_skinButtons.Count; i++)
        {
            m_buttonPositionBuffer.x = m_radiusDisplay * Mathf.Sin(i * m_angleStep * Mathf.Deg2Rad);
            m_buttonPositionBuffer.y = m_radiusDisplay * Mathf.Cos(i * m_angleStep * Mathf.Deg2Rad);
            m_buttonPositionBuffer.z = 0;

            m_skinButtons[i].GetComponent<RectTransform>().anchoredPosition = m_buttonPositionBuffer;
        }
    }
}
