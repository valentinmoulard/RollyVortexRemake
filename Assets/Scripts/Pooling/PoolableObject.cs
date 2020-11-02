using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour, IPoolable
{
    #region INTERN CLASS FOR SUB POOLABLE OBJECTS
    public class SubPoolableObject
    {
        private GameObject m_gameObject;
        private Vector3 m_initialPosition;

        public SubPoolableObject(GameObject gameObject, Vector3 initialPosition)
        {
            m_gameObject = gameObject;
            m_initialPosition = initialPosition;
        }

        public void ResetPosition()
        {
            m_gameObject.transform.localPosition = m_initialPosition;
        }

        public void Enable()
        {
            m_gameObject.SetActive(true);
        }

        public bool IsActiveInHierarchy()
        {
            return m_gameObject.activeInHierarchy;
        }
    }
    #endregion


    public event Action OnAllSubPoolableReturned; 

    private const float UPDATE_TIME = 0.1f;

    private float m_updateTimer;
    private List<SubPoolableObject> m_listOfPoolableChild;


    private bool HasAnyPoolableChild()
    {
        if (m_listOfPoolableChild == null)
            return false;

        return m_listOfPoolableChild.Count > 0;
    }

    void Awake()
    {
        if (this.transform.childCount > 0)
        {
            m_updateTimer = 0.0f;
            RetreiveSubPoolableObjects();
        }
    }


    void OnEnable()
    {
        if (HasAnyPoolableChild())
            EnableSubPoolables();
    }


    private void Update()
    {
        if (HasAnyPoolableChild())
            TimedUpdate();
    }


    /// <summary>
    /// Update elements only after some time.
    /// </summary>
    private void TimedUpdate()
    {
        if (m_updateTimer < UPDATE_TIME)
            m_updateTimer += Time.deltaTime;
        else
        {
            UpdateSubPoolableObjects();
            m_updateTimer = 0.0f;
        }
    }


    private void UpdateSubPoolableObjects()
    {
        int numberOfActiveSubPoolables = 0;

        for (int i = 0; i < m_listOfPoolableChild.Count; i++)
        {
            if (m_listOfPoolableChild[i].IsActiveInHierarchy())
                numberOfActiveSubPoolables++;
        }

        // Return to pool if all children have returned to pool
        if (numberOfActiveSubPoolables == 0)
        {
            if (OnAllSubPoolableReturned != null)
                OnAllSubPoolableReturned();

            ReturnToPool();
        }

    }


    private void RetreiveSubPoolableObjects()
    {
        m_listOfPoolableChild = new List<SubPoolableObject>();

        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<IPoolable>() != null)
            {
                SubPoolableObject sub = new SubPoolableObject(child.gameObject, child.transform.position);
                m_listOfPoolableChild.Add(sub);
            }
        }
    }


    private void EnableSubPoolables()
    {
        for (int i = 0; i < m_listOfPoolableChild.Count; i++)
            m_listOfPoolableChild[i].Enable();
    }


    private void ResetSubPoolablesPosition()
    {
        for (int i = 0; i < m_listOfPoolableChild.Count; i++)
            m_listOfPoolableChild[i].ResetPosition();
    }


    public void ReturnToPool()
    {
        if (HasAnyPoolableChild())
            ResetSubPoolablesPosition();

        this.gameObject.SetActive(false);
    }
}
