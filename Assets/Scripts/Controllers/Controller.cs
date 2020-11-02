using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Controller : MonoBehaviour, IController
{
    #region EVENTS
    public delegate void ControllerEvent(Vector3 cursorPosition);

    public static ControllerEvent OnTap;
    public static ControllerEvent OnTapBegin;
    public static ControllerEvent OnSwipe;
    public static ControllerEvent OnHold;
    public static ControllerEvent OnRelease;
    #endregion


    abstract protected void UpdateInputs();


    void Update()
    {
        UpdateInputs();
    }


    protected bool IsInputOverUI(int cursorID)
    {
        if (EventSystem.current != null)
        {
            if (EventSystem.current.IsPointerOverGameObject(cursorID))
                return true;
        }

        return false;
    }


    virtual public void Tap(Vector3 cursorPosition)
    {
        if (OnTap != null)
            OnTap(cursorPosition);
    }

    virtual public void TapBegin(Vector3 cursorPosition)
    {
        if (OnTapBegin != null)
            OnTapBegin(cursorPosition);
    }

    virtual public void Swipe(Vector3 cursorPosition)
    {
        if (OnSwipe != null)
            OnSwipe(cursorPosition);
    }

    virtual public void Hold(Vector3 cursorPosition)
    {
        if (OnHold != null)
            OnHold(cursorPosition);
    }

    virtual public void Release(Vector3 cursorPosition)
    {
        if (OnRelease != null)
            OnRelease(cursorPosition);
    }
}
