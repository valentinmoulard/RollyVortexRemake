using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script used on objects with rect transofrm component. Rotates the UI on the Z axis. Can be extended for X and Y axis.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class AutoRotationUI : MonoBehaviour
{
    [SerializeField]
    private float m_rotationSpeed = 50.0f;
    [SerializeField]
    private bool m_isRotationClockWise = false;

    private RectTransform m_rectTransformReference;
    private int m_rotationFactor;

    private void Start()
    {
        m_rectTransformReference = gameObject.GetComponent<RectTransform>();

        if (m_rectTransformReference == null)
            Debug.LogError("Could not get rect transform component!", gameObject);

        m_rotationFactor = m_isRotationClockWise ? 1 : -1;
    }

    private void Update()
    {
        m_rectTransformReference.Rotate(Vector3.forward * Time.deltaTime * m_rotationSpeed * m_rotationFactor);
    }
}
