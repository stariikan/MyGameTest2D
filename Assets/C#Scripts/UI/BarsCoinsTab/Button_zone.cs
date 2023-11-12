using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_zone : MonoBehaviour
{
    public float xPercent; // X position as a percentage of screen width (0-1)
    public float yPercent; // Y position as a percentage of screen height (0-1)
    public float zPosition; // Z position in world space

    private void Start()
    {
        // Get the size of the screen in pixels
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        // Convert the percentage positions to pixel positions
        float xPos = xPercent * screenSize.x;
        float yPos = yPercent * screenSize.y;

        // Convert the pixel positions to world positions
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(xPos, yPos, zPosition));

        // Set the object's position to the calculated world position
        transform.position = worldPosition;
    }
}
