using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMovement : MonoBehaviour
{
    public float joystickRadius = 50.0f; //radius of the joystick
    public float deadzone = 10.0f; //deadzone value to prevent small touch inputs from registering
    private Vector2 joystickCenter; //center of the joystick
    private Vector2 touchPosition; //position of the touch input
    private float distance; //distance between touch input and joystick center
    GameObject joystick;

    private void Start()
    {
        joystickCenter = transform.position;
    }

    private void Update()
    {
        if (Input.touchCount > 0) //check if there's a touch input
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) //check if touch phase is Began
            {
                touchPosition = touch.position;
                distance = Vector2.Distance(touchPosition, joystickCenter); //calculate distance between touch and joystick center

                if (distance < joystickRadius) //check if touch is within joystick radius
                {
                    touchPosition = Camera.main.ScreenToWorldPoint(touchPosition); //convert screen space to world space
                }
            }
            else if (touch.phase == TouchPhase.Moved) //check if touch phase is Moved
            {
                touchPosition = touch.position;
                distance = Vector2.Distance(touchPosition, joystickCenter); //recalculate distance

                if (distance < joystickRadius && distance > deadzone) //check if touch is within joystick radius and outside of deadzone
                {
                    touchPosition = Camera.main.ScreenToWorldPoint(touchPosition); //convert screen space to world space

                    float x = touchPosition.x - joystickCenter.x; //calculate x direction

                    if (x > 0) //if touch is to the right of the joystick center
                    {
                        if (Input.GetKey(KeyCode.D) == false)
                            Input.GetKeyDown(KeyCode.D); //emulate press of key D
                    }
                    else //if touch is to the left of the joystick center
                    {
                        if (Input.GetKey(KeyCode.A) == false)
                            Input.GetKeyDown(KeyCode.A); //emulate press of key A
                    }
                }
            }
        }
    }
}