using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCircles : MonoBehaviour, IPoolable
{
    [SerializeField, Tooltip("Reference of the line renderer.")]
    private LineRenderer m_lightLineRenderer = null;

    [SerializeField, Tooltip("The number of points in the line renderer. High value = detailed line.")]
    private int m_lineRendererPointCount = 20;


    private Vector3 m_vertexPositionBuffer;     //variable used to set up the position of the points of the line renderer
    private Vector3 m_levelStartPosition;
    private float m_angleStep;                  //variable calculated to determine the angle separating each point of the line renderer
    private float m_levelRadius;
    private float m_lightCircleSpeed;


    private void OnEnable()
    {
        LevelManager.OnLighCircleBuilt += SetUpLineRenderer;
    }


    private void OnDisable()
    {
        LevelManager.OnLighCircleBuilt -= SetUpLineRenderer;
    }


    void Start()
    {
        if (m_lightLineRenderer == null)
            Debug.LogError("The line renderer component is missing!", gameObject);

        m_vertexPositionBuffer = Vector3.zero;
    }


    private void Update()
    {
        MoveLightCircle();

        if (transform.position.z < GameManager.instance.Z_POSITION_POOL_RETURNING_CONDITION)
            ReturnToPool();

        UpdateLineRendererPointsPosition();
    }


    private void MoveLightCircle()
    {
        transform.Translate(-Vector3.forward * m_lightCircleSpeed * Time.deltaTime);
    }


    private void UpdateLineRendererPointsPosition()
    {
        for (int i = 0; i < m_lineRendererPointCount; i++)
        {
            m_vertexPositionBuffer = m_lightLineRenderer.GetPosition(i);
            m_vertexPositionBuffer.z += -m_lightCircleSpeed * Time.deltaTime;
            m_lightLineRenderer.SetPosition(i, m_vertexPositionBuffer);
        }
    }


    /// <summary>
    /// Method called when the game starts. It sets up some light circles to give the sensation that the player was already in the level.
    /// </summary>
    /// <param name="lineRendererReference"></param>
    /// <param name="startPosition"></param>
    /// <param name="lightCircleSpeed"></param>
    /// <param name="levelRadius"></param>
    private void SetUpLineRenderer(GameObject lineRendererReference, Vector3 startPosition, float lightCircleSpeed, float levelRadius)
    {
        if (lineRendererReference == gameObject)
        {
            m_lightCircleSpeed = lightCircleSpeed;
            m_levelStartPosition = startPosition;
            m_levelRadius = levelRadius;
            m_lightLineRenderer.positionCount = m_lineRendererPointCount;
            m_angleStep = 360f / (m_lineRendererPointCount - 1);

            for (int i = 0; i < m_lineRendererPointCount; i++)
            {
                m_vertexPositionBuffer.x = m_levelRadius * Mathf.Sin(i * m_angleStep * Mathf.Deg2Rad) + m_levelStartPosition.x;
                m_vertexPositionBuffer.y = m_levelRadius * Mathf.Cos(i * m_angleStep * Mathf.Deg2Rad) + m_levelStartPosition.y;
                m_vertexPositionBuffer.z = m_levelStartPosition.z;
                m_lightLineRenderer.SetPosition(i, m_vertexPositionBuffer);
            }
        }
    }


    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
