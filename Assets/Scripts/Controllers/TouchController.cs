using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : Controller
{
    #region CONSTANT VARIABLES
    private const int FIRST_TOUCH_ID = 0;

    private const float MAX_SWIPE_TIME = 1f;
    //in pixels
    private const float MIN_SWIPE_DISTANCE = 100f;
    private const float SWIPE_UP_ANGLE_THRESHOLD = 0.9f;
    #endregion


    private Vector2 m_touchStartWorldPosition = Vector2.zero;
    private float m_startTime = 0f;


    protected override void UpdateInputs()
    {
        if (IsInputOverUI(FIRST_TOUCH_ID))
            return;

        if (Input.touchCount > 0)
        {
            UpdateTouchPhases();
        }
    }


    private void UpdateTouchPhases()
    {
        Touch touch = Input.GetTouch(FIRST_TOUCH_ID); // Note: does not manage yet multiple touches

        if (touch.phase == TouchPhase.Began)
        {
            m_touchStartWorldPosition = new Vector2(touch.position.x, touch.position.y);
            m_startTime = Time.time;
            TapBegin(m_touchStartWorldPosition);
        }


        if (touch.phase == TouchPhase.Stationary)
        {
            Hold(touch.position);
        }


        if (touch.phase == TouchPhase.Moved)
        {
            Hold(touch.position);
        }


        if (touch.phase == TouchPhase.Ended)
        {
            float swipeDuration = Time.time - m_startTime;
            Vector2 touchEndWorldPosition = new Vector2(touch.position.x, touch.position.y);

            Release(touchEndWorldPosition);

            if (swipeDuration < MAX_SWIPE_TIME)
            {
                if (Vector2.Distance(m_touchStartWorldPosition, touchEndWorldPosition) < MIN_SWIPE_DISTANCE)
                {
                    Tap(m_touchStartWorldPosition);
                }
                else
                {
                    Vector2 direction = touchEndWorldPosition - m_touchStartWorldPosition;
                    Swipe(direction);
                }
            }
        }
    }
}
