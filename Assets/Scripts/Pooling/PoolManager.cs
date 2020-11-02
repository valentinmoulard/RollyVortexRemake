using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private List<Pool> m_poolList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        RetreivePoolsInChildren(); 
    }


    // Start is called before the first frame update
    void Start()
    {
        InitPoolList();
    }


    private void RetreivePoolsInChildren()
    {
        m_poolList = new List<Pool>();
        Pool[] tempArray = this.GetComponentsInChildren<Pool>();

        if (tempArray == null || tempArray.Length == 0)
        {
            Debug.LogError("Cannot retreive pools : no pool object exists in child!");
            return;
        }

        Debug.Log("Retreiving pool list with children in the pool manager...", this.gameObject);

        for (int i = 0; i < tempArray.Length; i++)
            m_poolList.Add(tempArray[i]);
    }


    void InitPoolList()
    {
        for(int i = 0; i < m_poolList.Count; i++)
            m_poolList[i].Populate();
    }



    public void SwitchPooledObject(GameObject oldPrefabReference, GameObject newPrefabReference)
    {
        for (int i = 0; i < m_poolList.Count; i++)
        {
            if (oldPrefabReference.Equals(m_poolList[i].PrefabReference))
                m_poolList[i].SwapReference(newPrefabReference);
        }
    }

    public GameObject SpawnPooledObject(GameObject prefabReference, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (m_poolList.Count == 0)
        {
            Debug.LogError("Cannot spawn object : no pool exists!", this.gameObject);
            return null;
        }

        for(int i = 0; i < m_poolList.Count; i++)
        {
            if (prefabReference.Equals(m_poolList[i].PrefabReference))
            {
                GameObject pooledObj = m_poolList[i].GetPooledObject();

                if (pooledObj != null)
                {
                    pooledObj.transform.position = spawnPosition;
                    pooledObj.transform.rotation = spawnRotation;
                }

                return pooledObj;
            }
        }

        Debug.LogError("Cannot spawn pooled object: no pool has been found for the following prefab :" + prefabReference, this.gameObject);
        return null;
    }


    private void ReturnAllToPool()
    {
        for (int i = 0; i < m_poolList.Count; i++)
        {
            m_poolList[i].ReturnAllToPool();
        }
    }
}
