using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPoolable))]
public class PoolableLifetime : MonoBehaviour
{
    [SerializeField]
    private float m_lifeTime = 5.0f;

    private float m_currentTimer;
    private IPoolable m_poolable;


    private void Start()
    {
        m_currentTimer = 0.0f;
        m_poolable = this.GetComponent<IPoolable>();

        if (m_poolable == null)
            Debug.LogError("No poolable object implementing the poolable interface has been found!", this.gameObject);

        if (m_lifeTime < 0)
            m_lifeTime = Mathf.Abs(m_lifeTime);
    }


    // Update is called once per frame
    void Update()
    {
        if (m_currentTimer < m_lifeTime)
            m_currentTimer += Time.deltaTime;
        else
            ReturnToPool();
    }


    private void ReturnToPool()
    {
        m_currentTimer = 0.0f;
        m_poolable.ReturnToPool();
    }
}
