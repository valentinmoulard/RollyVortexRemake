using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBarElement : ObstacleElement
{
    private Vector3 m_scaleBuffer;

    protected override void OnEnable()
    {
        m_scaleBuffer = Vector3.one;
        base.OnEnable();
        LevelManager.OnObstacleBuilt += SetObstacleScale;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LevelManager.OnObstacleBuilt -= SetObstacleScale;
    }

    /// <summary>
    /// Method that sets up the scale of the bar depending on the radius of the level.
    /// </summary>
    /// <param name="spawnedObstacleReference"></param>
    /// <param name="startPosition"></param>
    /// <param name="obstacleSpeed"></param>
    /// <param name="levelRadius"></param>
    private void SetObstacleScale(GameObject spawnedObstacleReference, Vector3 startPosition, float obstacleSpeed, float levelRadius)
    {
        m_scaleBuffer.y = levelRadius * 2;
        transform.localScale = m_scaleBuffer;
        m_speed = obstacleSpeed;
    }
}
