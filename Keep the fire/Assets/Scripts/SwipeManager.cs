using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    [SerializeField] private int swipeDeadZone = 125;

    public bool Tap { get; private set; }
    public bool SwipeLeft { get; private set; }
    public bool SwipeRight { get; private set; }
    public bool SwipeUp { get; private set; }
    public bool SwipeDown { get; private set; }

    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;
    void Update()
    {
        Tap = SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;

        // keyboard input
        if(Input.GetMouseButtonDown(0))
        {
            Tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            Reset();
        }
        // mobile input
        if(Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                Tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || 
                     Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }
        // подсчет дистанции
        swipeDelta = Vector2.zero;
        if(isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if(Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        // check deadzone
        if (swipeDelta.magnitude > swipeDeadZone)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                if(x < 0)
                {
                    print("left");
                    SwipeLeft = true;
                }
                else
                {
                    print("right");
                    SwipeRight = true;
                }
            }
            else
            {
                if (y < 0)
                {
                    print("down");
                    SwipeDown = true;
                }
                else
                {
                    print("up");
                    SwipeUp = true;
                }
            }

            Reset();
        }

    }
    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}
