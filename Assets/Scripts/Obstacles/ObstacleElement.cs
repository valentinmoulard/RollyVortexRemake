using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleElement : MonoBehaviour, IPoolable
{
    public static Action<int> OnObstacleElementPassed;

    [SerializeField, Tooltip("The reference of the rigidbody.")]
    private Rigidbody m_rigidbodyReference = null;

    [SerializeField, Tooltip("The score value of the obstacle.")]
    protected int m_obstacleScoreValue = 1;

    protected float m_speed;
    private bool m_isObstaclePassed;
    private bool m_isExploded;

    protected virtual void OnEnable()
    {
        m_isObstaclePassed = false;
        LevelProceduralObstacle.OnObstacleSpeedSent += GetSpeed;
        PlayerController.OnPowerActivated += Explode;
        GameManager.OnRetry += ReturnToPool;
        GameManager.OnGameOver += ReturnToPool;
    }

    protected virtual void OnDisable()
    {
        LevelProceduralObstacle.OnObstacleSpeedSent -= GetSpeed;
        PlayerController.OnPowerActivated -= Explode;
        GameManager.OnRetry -= ReturnToPool;
        GameManager.OnGameOver -= ReturnToPool;
    }

    private void Start()
    {
        if (m_rigidbodyReference == null)
            Debug.LogError("The rigidbody component is missing!", gameObject);
    }

    private void Update()
    {
        MoveObstacle();
    }


    private void MoveObstacle()
    {
        if (!m_isExploded)
        {
            transform.Translate(-Vector3.forward * m_speed * Time.deltaTime);

            if (transform.position.z < GameManager.instance.Z_POSITION_POOL_RETURNING_CONDITION)
                ReturnToPool();

            if (m_isObstaclePassed == false && transform.position.z < 0.0f)
            {
                m_isObstaclePassed = true;
                BroadcastOnObstacleElementPassed();
            }
        }
    }


    /// <summary>
    /// Method that starts a coroutine and allows the obstacle to fly away by adding forces on rigidbody.
    /// </summary>
    private void Explode()
    {
        StartCoroutine(ObstacleExplodeCoroutine());
    }

    private IEnumerator ObstacleExplodeCoroutine()
    {
        m_isExploded = true;
        m_rigidbodyReference.useGravity = true;

        m_rigidbodyReference.AddForce(
            Vector3.forward * UnityEngine.Random.Range(30f, 50f)
            + Vector3.up * UnityEngine.Random.Range(-50f, 50f)
            + Vector3.right * UnityEngine.Random.Range(-50f, 50f)
            , ForceMode.Impulse);

        m_rigidbodyReference.AddTorque(Vector3.forward * UnityEngine.Random.Range(30f, 50f)
            + Vector3.up * UnityEngine.Random.Range(-50f, 50f)
            + Vector3.right * UnityEngine.Random.Range(-50f, 50f)
            , ForceMode.Impulse);

        BroadcastOnObstacleElementPassed();

        yield return new WaitForSeconds(4f);
        ReturnToPool();
    }

    private void GetSpeed(GameObject obstacleElement, float speed)
    {
        if (obstacleElement == gameObject)
            m_speed = speed;
    }

    private void ResetObstacle()
    {
        m_rigidbodyReference.useGravity = false;
        m_isExploded = false;
        m_rigidbodyReference.velocity = Vector3.zero;
        m_rigidbodyReference.angularVelocity = Vector3.zero;
    }

    public void ReturnToPool()
    {
        ResetObstacle();
        gameObject.SetActive(false);
    }

    private void BroadcastOnObstacleElementPassed()
    {
        if (OnObstacleElementPassed != null)
            OnObstacleElementPassed(m_obstacleScoreValue);
    }
}
