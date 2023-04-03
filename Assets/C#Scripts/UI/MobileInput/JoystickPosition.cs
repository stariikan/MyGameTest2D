using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPosition : MonoBehaviour
{
    private Vector3 originalPos;
    public bool joystick_settings; //Джойстик или кнопки

    public static JoystickPosition Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    private void Start()
    {
        Instance = this;
        originalPos = transform.position;
    }
    // Update is called once per frame

    public void Joystick_ON()
    {
        this.gameObject.SetActive(true);
    }
    public void Joystick_OFF()
    {
        this.gameObject.SetActive(false);
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

}
