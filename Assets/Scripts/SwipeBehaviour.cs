using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeBehaviour : MonoBehaviour
{
    public float swipeThreshold = 100f;
    public float swipeTime = 0.3f;
    public GameObject panelToToggleLeft;
    public GameObject panelToToggleRight;
    public GameObject currentPanel;

    private Vector2 swipeStartPosition;
    private float swipeStartTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            swipeStartPosition = Input.GetTouch(0).position;
            swipeStartTime = Time.time;
        }
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector2 swipeEndPosition = Input.GetTouch(0).position;
            float swipeDuration = Time.time - swipeStartTime;
            float swipeDistance = Vector2.Distance(swipeStartPosition, swipeEndPosition);

            if (swipeDuration < swipeTime && swipeDistance > swipeThreshold)
            {
                float swipeAngle = Mathf.Atan2(swipeEndPosition.y - swipeStartPosition.y, swipeEndPosition.x - swipeStartPosition.x) * 180 / Mathf.PI;

                if (swipeAngle < -135 || swipeAngle > 135) // Swipe Left
                {
                    if (panelToToggleRight != null)
                    {
                        panelToToggleRight.SetActive(!panelToToggleRight.activeSelf);
                    }
                }
                else if (swipeAngle > -45 && swipeAngle < 45) // Swipe Right
                {
                    if (panelToToggleLeft != null)
                    {
                        panelToToggleLeft.SetActive(!panelToToggleLeft.activeSelf);
                    }
                }

                if (currentPanel != null)
                {
                    currentPanel.SetActive(false);
                }
            }
        }
    }
}



