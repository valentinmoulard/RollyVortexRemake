using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action<GameObject, Vector3, float, float> OnObstacleBuilt;
    public static Action<GameObject, Vector3, float, float> OnLighCircleBuilt;
    public static Action<GameObject, float> OnBonusSpawned;

    [Header("Collectible Parameters")]
    [SerializeField]
    private GameObject m_collectibleBonusPrefab = null;
    [SerializeField, Range(0.0f, 0.2f)]
    private float m_bonusSpawnChance = 0.05f;
    [SerializeField]
    private GameObject m_collectibleCurrencyPrefab = null;
    [SerializeField, Range(0.0f, 0.2f)]
    private float m_currencySpawnChance = 0.05f;

    [Header("Obstacles Parameters")]
    [SerializeField]
    private GameObject m_levelProceduralObstaclePrefabReference = null;
    [SerializeField]
    private GameObject m_levelBarObstaclePrefabReference = null;
    [SerializeField, Range(0.0f, 0.4f)]
    private float m_barObstacleSpawnChance = 0.2f;

    [Header("Spawning Parameters")]
    [SerializeField]
    private AnimationCurve m_obstacleSpawnFrequencyCurve = null;
    [SerializeField]
    private AnimationCurve m_obstacleSpeedCurve = null;
    [SerializeField]
    private Vector3 m_spawnOffsetSpawnPosition = Vector3.zero;

    [Header("Light Circle Parameters")]
    [SerializeField]
    private GameObject m_lightCirclePrefab = null;
    [SerializeField]
    private float m_lightCircleSpeed = 10f;
    [SerializeField]
    private int m_lightCircleSpawnRate = 2;
    [SerializeField]
    private float m_lightCircleRadiusOffset = 0.5f;

    private GameObject m_spawnedLevelObstacleBuffer;
    private GameObject m_spawnedBonusBuffer;
    private GameObject m_spawnedLightCircleBuffer;
    private Vector3 m_startPosition;
    private Vector3 m_bonusSpawnPosition;
    private Vector3 m_lightCirclePositionBuffer;
    private float m_levelRadius;
    private float m_spawnAngleBuffer;
    private float m_randomValueBuffer;
    private bool m_canSpawn;

    private Coroutine m_obstacleSpawningCoroutine;
    private Coroutine m_lightCircleSpawningCoroutine;


    private void OnEnable()
    {
        GameManager.OnStartGame += StartSpawning;
        GameManager.OnPlayerRevived += StartSpawning;
        GameManager.OnRetry += StopSpawning;
        GameManager.OnGameOver += StopSpawning;
        GameManager.OnLevelRadiusSent += GetLevelRadius;
        GameManager.OnStartPositionSent += GetStartPosition;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= StartSpawning;
        GameManager.OnPlayerRevived -= StartSpawning;
        GameManager.OnRetry -= StopSpawning;
        GameManager.OnGameOver -= StopSpawning;
        GameManager.OnLevelRadiusSent -= GetLevelRadius;
        GameManager.OnStartPositionSent += GetStartPosition;
    }

    private void Start()
    {
        if (m_levelProceduralObstaclePrefabReference == null)
            Debug.LogError("The procedural obstacle prefab is missing!", gameObject);
        if (m_levelBarObstaclePrefabReference == null)
            Debug.LogError("The bar obstacle prefab is missing!", gameObject);

        if (m_collectibleBonusPrefab == null)
            Debug.LogError("The collectible bonus prefab is missing!", gameObject);
        if (m_collectibleCurrencyPrefab == null)
            Debug.LogError("The collectible currency prefab is missing!", gameObject);

        if (m_lightCirclePrefab == null)
            Debug.LogError("The light circle prefab is missing!", gameObject);


        InitLevel();
    }


    private void StartSpawning()
    {
        m_canSpawn = true;
        m_obstacleSpawningCoroutine = StartCoroutine(SpawnObstacleCoroutine());

    }

    private void StopSpawning()
    {
        m_canSpawn = false;
        if (m_obstacleSpawningCoroutine != null)
            StopCoroutine(m_obstacleSpawningCoroutine);
    }

    /// <summary>
    /// Method that spawns light circles before the game starts so the player is already in the tube level.
    /// </summary>
    private void InitLevel()
    {
        m_canSpawn = false;

        for (int i = 0; i < 50; i++)
        {
            m_lightCirclePositionBuffer = m_startPosition + m_spawnOffsetSpawnPosition - Vector3.forward * (m_lightCircleSpeed / (float)m_lightCircleSpawnRate) * i;
            m_spawnedLightCircleBuffer = PoolManager.instance.SpawnPooledObject(m_lightCirclePrefab, m_lightCirclePositionBuffer, Quaternion.identity);
            BroadcastOnLighCircleBuilt(m_spawnedLightCircleBuffer, m_lightCirclePositionBuffer);
        }

        m_lightCircleSpawningCoroutine = StartCoroutine(SpawnLightCircleCoroutine());
    }

    /// <summary>
    /// Coroutine that spawns light circles once every second
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnLightCircleCoroutine()
    {
        while (Application.isPlaying)
        {
            SpawnLightCircle();
            yield return new WaitForSeconds(1.0f/ m_lightCircleSpawnRate);
        }
    }


    /// <summary>
    /// Spawn a light circle to give the sensation to be in a neon tube
    /// </summary>
    private void SpawnLightCircle()
    {
        m_lightCirclePositionBuffer = m_startPosition + m_spawnOffsetSpawnPosition;
        m_spawnedLightCircleBuffer = PoolManager.instance.SpawnPooledObject(m_lightCirclePrefab, m_lightCirclePositionBuffer, Quaternion.identity);
        BroadcastOnLighCircleBuilt(m_spawnedLightCircleBuffer, m_lightCirclePositionBuffer);
    }

    /// <summary>
    /// Coroutine that spawns obstacles and collectibles radomly according to the parameters set
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnObstacleCoroutine()
    {
        while (m_canSpawn)
        {
            m_randomValueBuffer = UnityEngine.Random.Range(0.0f, 1.0f);
            if (m_randomValueBuffer < m_bonusSpawnChance)
            {
                SpawnBonusCollectible();
            }
            else if (m_randomValueBuffer < m_bonusSpawnChance + m_currencySpawnChance)
            {
                SpawnCurrencyCollectible();
            }
            else if (m_randomValueBuffer < m_bonusSpawnChance + m_currencySpawnChance + m_barObstacleSpawnChance)
            {
                SpawnBarObstacle();
            }
            else
            {
                SpawnLevelProceduralObstacle();
            }

            yield return new WaitForSeconds(m_obstacleSpawnFrequencyCurve.Evaluate(TimeManager.instance.GameSessionTime));
        }
    }

    private void SpawnBonusCollectible()
    {
        CalculateCollectibleSpawnPosition();
        m_spawnedBonusBuffer = PoolManager.instance.SpawnPooledObject(m_collectibleBonusPrefab, m_bonusSpawnPosition, Quaternion.identity);
        BroadcastOnBonusSpawned(m_spawnedBonusBuffer);
    }

    private void SpawnCurrencyCollectible()
    {
        CalculateCollectibleSpawnPosition();
        m_spawnedBonusBuffer = PoolManager.instance.SpawnPooledObject(m_collectibleCurrencyPrefab, m_bonusSpawnPosition, Quaternion.identity);
        BroadcastOnBonusSpawned(m_spawnedBonusBuffer);
    }

    private void CalculateCollectibleSpawnPosition()
    {
        m_spawnAngleBuffer = UnityEngine.Random.Range(0.0f, 360f);
        m_bonusSpawnPosition.x = m_levelRadius * Mathf.Sin(m_spawnAngleBuffer * Mathf.Deg2Rad);
        m_bonusSpawnPosition.y = m_levelRadius * Mathf.Cos(m_spawnAngleBuffer * Mathf.Deg2Rad);
        m_bonusSpawnPosition.z = transform.position.z;
        m_bonusSpawnPosition += m_spawnOffsetSpawnPosition + m_startPosition;
    }


    /// <summary>
    /// Method that spawns a bar obstacle
    /// </summary>
    private void SpawnBarObstacle()
    {
        m_spawnedLevelObstacleBuffer = PoolManager.instance.SpawnPooledObject(m_levelBarObstaclePrefabReference, m_startPosition + m_spawnOffsetSpawnPosition, Quaternion.identity);
        m_spawnedLevelObstacleBuffer.transform.rotation = Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 180f));
        BroadcastOnObstacleBuilt(m_spawnedLevelObstacleBuffer, m_obstacleSpeedCurve.Evaluate(TimeManager.instance.GameSessionTime), m_levelRadius);
    }

    /// <summary>
    /// Method that spawns a procedural obstacle
    /// </summary>
    private void SpawnLevelProceduralObstacle()
    {
        m_spawnedLevelObstacleBuffer = PoolManager.instance.SpawnPooledObject(m_levelProceduralObstaclePrefabReference, m_startPosition + m_spawnOffsetSpawnPosition, Quaternion.identity);
        BroadcastOnObstacleBuilt(m_spawnedLevelObstacleBuffer, m_obstacleSpeedCurve.Evaluate(TimeManager.instance.GameSessionTime), m_levelRadius);
    }

    private void GetLevelRadius(float levelRadius)
    {
        m_levelRadius = levelRadius;
    }

    private void GetStartPosition(Vector3 startPosition)
    {
        m_startPosition = startPosition;
    }

    #region Broadcaster
    private void BroadcastOnObstacleBuilt(GameObject spawnedObstacle, float obstacleSpeed, float levelRadius)
    {
        if (OnObstacleBuilt != null)
            OnObstacleBuilt(spawnedObstacle, m_startPosition, obstacleSpeed, m_levelRadius);
    }

    private void BroadcastOnLighCircleBuilt(GameObject spawnedLightCircle, Vector3 position)
    {
        if (OnLighCircleBuilt != null)
            OnLighCircleBuilt(spawnedLightCircle, position, m_lightCircleSpeed, m_levelRadius + m_lightCircleRadiusOffset);
    }

    private void BroadcastOnBonusSpawned(GameObject spawnedBonus)
    {
        if (OnBonusSpawned != null)
            OnBonusSpawned(spawnedBonus, m_obstacleSpeedCurve.Evaluate(TimeManager.instance.GameSessionTime));
    }
    #endregion
}
