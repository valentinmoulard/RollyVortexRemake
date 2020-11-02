using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseController)), RequireComponent(typeof(TouchController))]
public class ControllerManager : MonoBehaviour
{

    private MouseController m_mouseController;
    private TouchController m_touchController;

    void Awake()
    {
        m_mouseController = this.GetComponent<MouseController>();
        m_touchController = this.GetComponent<TouchController>();

        if (m_mouseController == null || m_touchController == null)
            Debug.LogError("Either the mouse or touch controller is missing!", this.gameObject);


# if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isRemoteConnected)
            EnableTouchControls();
        else
            EnableMouseControls();
#else
        EnableTouchControls();
#endif

    }



    private void EnableMouseControls()
    {
        Debug.Log("Enabling mouse controls");

        m_mouseController.enabled = true;
        m_touchController.enabled = false;
    }

    private void EnableTouchControls()
    {
        Debug.Log("Enabling touch controls");

        m_mouseController.enabled = false;
        m_touchController.enabled = true;
    }
}
