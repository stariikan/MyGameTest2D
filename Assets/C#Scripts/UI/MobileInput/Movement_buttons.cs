using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_buttons : MonoBehaviour
{
    public bool joystick_settings; //�������� ��� ������

    public static Movement_buttons Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void Start()
    {
        Instance = this;
    }
    // Update is called once per frame
    public void MovementButtons_ON()
    {
        this.gameObject.SetActive(true);
    }
    public void MovementButtons_OFF()
    {
        this.gameObject.SetActive(false);
    }
}

