using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPosition : MonoBehaviour
{
    private Vector3 originalPos;
    public static JoystickPosition Instance { get; set; }

    private void Start()
    {
        Instance = this;
        originalPos = transform.position;
    }
    public void ChangeJoystickPosition()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < Screen.width / 2 && Input.mousePosition.y < Screen.height / 2)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //transform.position = originalPos;
        }
    }
    // Update is called once per frame
    void Update()
    {
        ChangeJoystickPosition();
    }
}
