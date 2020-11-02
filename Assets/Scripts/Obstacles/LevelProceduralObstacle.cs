using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProceduralObstacle : MonoBehaviour, IPoolable
{
    public static Action<GameObject,float> OnObstacleSpeedSent;


    private const int MAX_OBSTACLE_ELEMENT = 15; //the maximum amout of blocks on a circle for each spawned obstacle.

    [SerializeField, Tooltip("The reference to the obstacle element prefab to spawn when building the obstacle.")]
    private GameObject m_obstaclePrefabReference = null;
    [SerializeField, Tooltip("The minimum amount of blocks to spawn for each obstacle.")]
    private int m_numberOfMinimumObstacleElement = 10;
    [SerializeField, Range(1,9), Tooltip("This value indicates that there is a minimum of 3 gaps (so the player can pass an obstacle) for each spawned obstacles.")]
    private int m_minGapsNumberInObstacle = 3;

    private GameObject m_spawnedObstacleElement;
    private Vector3 m_desiredPosition;
    private float m_angleStepBuffer;
    private int m_obstacleElementCountBuffer;

    private void OnEnable()
    {
        LevelManager.OnObstacleBuilt += BuildObstacle;

        Check();
    }

    private void OnDisable()
    {
        LevelManager.OnObstacleBuilt -= BuildObstacle;
    }

    private void Check()
    {
        if (m_obstaclePrefabReference == null)
            Debug.LogError("The obstacle prefab reference is missing!", gameObject);
        if (m_numberOfMinimumObstacleElement <= 0)
        {
            m_numberOfMinimumObstacleElement = 5;
        }
        else if (m_numberOfMinimumObstacleElement > MAX_OBSTACLE_ELEMENT - m_minGapsNumberInObstacle)
        {
            m_numberOfMinimumObstacleElement = MAX_OBSTACLE_ELEMENT - m_minGapsNumberInObstacle;
        }
        m_angleStepBuffer = 360f / (float)MAX_OBSTACLE_ELEMENT;
    }

    /// <summary>
    /// Method that calculates a random number of blocks to put in the circle obstacle. Then displays the blocks randomly on the circle.
    /// </summary>
    /// <param name="spawnedObstacle"></param>
    /// <param name="startPosition"></param>
    /// <param name="obstacleSpeed"></param>
    /// <param name="levelRadius"></param>
    private void BuildObstacle(GameObject spawnedObstacle, Vector3 startPosition, float obstacleSpeed, float levelRadius)
    {
        if (spawnedObstacle == gameObject)
        {
            m_obstacleElementCountBuffer = UnityEngine.Random.Range(m_numberOfMinimumObstacleElement, MAX_OBSTACLE_ELEMENT - m_minGapsNumberInObstacle);

            for (int i = 0; i < MAX_OBSTACLE_ELEMENT; i++)
            {
                if (m_obstacleElementCountBuffer > 0)
                {
                    if (UnityEngine.Random.Range(0.0f, 1.0f) < (float)m_obstacleElementCountBuffer / (float)(MAX_OBSTACLE_ELEMENT - i))
                    {
                        m_desiredPosition.x = levelRadius * Mathf.Sin(i * m_angleStepBuffer * Mathf.Deg2Rad) + startPosition.x;
                        m_desiredPosition.y = levelRadius * Mathf.Cos(i * m_angleStepBuffer * Mathf.Deg2Rad) + startPosition.y;
                        m_desiredPosition.z = transform.position.z + startPosition.z;

                        m_spawnedObstacleElement = PoolManager.instance.SpawnPooledObject(m_obstaclePrefabReference, m_desiredPosition, Quaternion.Euler(0, 0, i * m_angleStepBuffer * -1));
                        BroadcastOnObstacleSpeedSent(m_spawnedObstacleElement, obstacleSpeed);
                        m_obstacleElementCountBuffer--;
                    }
                }
            }
        }
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    private void BroadcastOnObstacleSpeedSent(GameObject obstacleElement, float obstacleSpeed)
    {
        if (OnObstacleSpeedSent != null)
            OnObstacleSpeedSent(obstacleElement, obstacleSpeed);
    }
}
