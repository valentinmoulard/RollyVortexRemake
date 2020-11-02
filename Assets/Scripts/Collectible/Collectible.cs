using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class of the collectibles
/// </summary>
public abstract class Collectible : MonoBehaviour, IPoolable
{
    private float m_speed;

    private void OnEnable()
    {
        LevelManager.OnBonusSpawned += GetSpeed;
    }

    private void OnDisable()
    {
        LevelManager.OnBonusSpawned -= GetSpeed;
    }

    private void Update()
    {
        transform.Translate(-Vector3.forward * m_speed * Time.deltaTime);

        if (transform.position.z < GameManager.instance.Z_POSITION_POOL_RETURNING_CONDITION)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameTags.PLAYER_TAG))
        {
            TriggerCollectibleEffect();
            ReturnToPool();
        }
    }

    private void GetSpeed(GameObject bonusReference, float speed)
    {
        if (bonusReference == gameObject)
            m_speed = speed;
    }

    /// <summary>
    /// Overriden method of child classes. The effect of each collectible can vary.
    /// </summary>
    public virtual void TriggerCollectibleEffect(){ }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
