using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickMagicShot : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0) // check if there's a touch input
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) // check if touch phase is Began
            {
                EmulateMouseButton1Click();
            }
        }
    }

    void EmulateMouseButton1Click()
    {
        // Emulate a mouse button 1 click
        if (Input.GetMouseButtonDown(1))
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended)
                {
                    Input.GetMouseButtonDown(1);
                }
            }
        }
    }
}
