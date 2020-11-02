using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScoreUI : MonoBehaviour
{
    [SerializeField, Tooltip("Bump duration in seconds.")]
    private float m_bumpDuration = 1.0f;
    [SerializeField, Tooltip("The bumped scale of the UI.")]
    private float m_desiredScaleFactor = 1.2f;

    private Coroutine m_bumpCoroutineReference;
    private float m_timer;

    private void OnEnable()
    {
        ScoreManager.OnBonusThresholdTriggered += BumpEffectUI;
    }

    private void OnDisable()
    {
        ScoreManager.OnBonusThresholdTriggered -= BumpEffectUI;
    }

    private void BumpEffectUI()
    {
        if (m_bumpCoroutineReference != null)
            StopCoroutine(m_bumpCoroutineReference);

        m_bumpCoroutineReference = StartCoroutine(BumpCoroutine());
    }

    private IEnumerator BumpCoroutine()
    {
        while(m_timer < m_bumpDuration)
        {
            m_timer += Time.deltaTime;

            if (m_timer < m_bumpDuration/2)
            {
                transform.localScale += Vector3.one * m_desiredScaleFactor * Time.deltaTime;
            }
            else
            {
                transform.localScale -= Vector3.one * m_desiredScaleFactor * Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }

        transform.localScale = Vector3.one;

    }
}
