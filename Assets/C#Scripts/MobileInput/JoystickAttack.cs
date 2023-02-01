using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAttack : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0) //check if there's a touch input
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) //check if touch phase is Began
            {
                Vector2 touchPos = touch.position;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // Emulate left mouse button click
                    Input.GetMouseButtonDown(0);
                }
            }
        }
    }
}
