using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    #region INTERN CLASS POOLEDOBJECT
    private class PooledObject
    {
        public GameObject m_pooledGameObject;
        public IPoolable m_poolableReference;

        public PooledObject(GameObject pooledObject, IPoolable poolableRef)
        {
            m_pooledGameObject = pooledObject;
            m_poolableReference = poolableRef;
        }

        public void Clean()
        {
            Destroy(m_pooledGameObject);
            m_pooledGameObject = null;
            m_poolableReference = null;
        }

        public bool IsActiveInHierarchy()
        {
            return m_pooledGameObject.activeInHierarchy;
        }

        public GameObject Pick()
        {
            m_pooledGameObject.SetActive(true);
            return m_pooledGameObject;
        }

        public void ReturnToPool()
        {
            m_poolableReference.ReturnToPool();
        }
    }
    #endregion


    [SerializeField, Tooltip("The reference on the prefab to pool.")]
    private GameObject m_prefabToPool = null;

    [SerializeField, Tooltip("The amount of instances to pool.")]
    private int m_amount = 5;

    [SerializeField, Tooltip("If true, the pool will instanciate a new object if it has no more available ones.")]
    private bool m_canExpand = true;


    private List<PooledObject> m_pooledObjects;

    public GameObject PrefabReference { get => m_prefabToPool; }


    private void Awake()
    {
        if (m_prefabToPool == null)
            Debug.LogError("The prefab to pool has not been defined in the inspector!", this.gameObject);

        if (m_amount < 0)
        {
            Debug.LogError("The amount to spawn is negative. Will use absolute value.", this.gameObject);
            m_amount = Mathf.Abs(m_amount);
        }

        m_pooledObjects = new List<PooledObject>();
    }


    public void SwapReference(GameObject newPrefabReference)
    {
        if (newPrefabReference == null)
        {
            Debug.LogError("Cannot switch pooled object, the given one is null!", this.gameObject);
            return;
        }

        m_prefabToPool = newPrefabReference;
        Repopulate();
    }


    private void Clean()
    {
        for (int i = 0; i < m_pooledObjects.Count; i++)
            m_pooledObjects[i].Clean();

        m_pooledObjects.Clear();
    }


    private void Repopulate()
    {
        Clean();
        Populate();
    }


    public void Populate()
    {
        // Do not populate if pooled objects have been populated before. To empty a pool, use Repopulate
        if (m_pooledObjects.Count > 0)
            return;

        for (int i = 0; i < m_amount; i++)
            AddNewPooledInstance();
    }


    /// <summary>
    /// Adds a new pooled instance in the pool
    /// </summary>
    /// <param name="isActive">Is the instance active and used when created?</param>
    /// <returns>The added GameObject instance if further processing is needed.</returns>
    private GameObject AddNewPooledInstance(bool isActive = false)
    {
        GameObject newPooledObject = Instantiate(m_prefabToPool, this.transform);
        newPooledObject.SetActive(isActive);

        IPoolable poolable = newPooledObject.GetComponent<IPoolable>();

        if (poolable == null)
        {
            Debug.LogError("No poolable script has been found on the given gameobject when adding a new pooled instance!", this.gameObject);
            return null;
        }

        PooledObject pooledObject = new PooledObject(newPooledObject, poolable);
        m_pooledObjects.Add(pooledObject);

        return newPooledObject;
    }


    /// <summary>
    /// Returns a pooled instance in the pool. If no instance are available, it will generate a new one according to the canExpand value.
    /// </summary>
    /// <returns>The first available instance in hierachy or the new created one. Otherwise, returns null.</returns>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < m_pooledObjects.Count; i++)
        {
            if (!m_pooledObjects[i].IsActiveInHierarchy())
            {
                return m_pooledObjects[i].Pick();
            }
        }

        if (m_canExpand)
            return AddNewPooledInstance(true);
        else
            return null;
    }

    /// <summary>
    /// Orders to all pools to return their pooled object in them
    /// </summary>
    public void ReturnAllToPool()
    {
        for (int i = 0; i < m_pooledObjects.Count; i++)
        {
            m_pooledObjects[i].ReturnToPool();
        }
    }
}
