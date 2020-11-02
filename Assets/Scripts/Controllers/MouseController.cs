using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : Controller
{
    #region CONSTANT VARIABLES
    private const int MOUSE_ID = -1;

    private const float MIN_SWIPE_DISTANCE = 1.0f;
    private const float SWIPE_UP_ANGLE_THRESHOLD = 0.9f;
    private const float MIN_SWIPE_TIME = 0.4f;
    private const float MIN_HOLD_TIME = 0.3f;
    private const float MAX_SWIPE_TIME = 1f;
    #endregion

    #region PRIVATE VARIABLES
    private Vector2 m_clickPosition;
    private Vector2 m_clickReleasePosition;
    private Vector2 m_currentMousePosition;
    private Vector2 m_swipeDirection;
    private Vector2 m_previousMousePosition;

    private float m_startTime = 0f;
    #endregion


    protected override void UpdateInputs()
    {
        if (IsInputOverUI(MOUSE_ID))
            return;

        if (Input.GetMouseButtonDown(0))
        {
            m_clickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            m_startTime = Time.time;
            TapBegin(m_clickPosition);
        }


        if (Input.GetMouseButton(0))
        {
            m_currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            Hold(m_currentMousePosition);
            m_previousMousePosition = m_currentMousePosition;
        }


        if (Input.GetMouseButtonUp(0))
        {
            m_clickReleasePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Release(m_clickReleasePosition);


            if (Vector2.Distance(m_clickPosition, m_clickReleasePosition) < MIN_SWIPE_DISTANCE && (Time.time - m_startTime) < MIN_SWIPE_TIME)
            {
                Tap(m_clickPosition);
            }
            else if (Vector2.Distance(m_clickPosition, m_clickReleasePosition) > MIN_SWIPE_DISTANCE)
            {
                Vector2 direction = m_clickReleasePosition - m_clickPosition;
                Swipe(direction);
            }
        }
    }
}
