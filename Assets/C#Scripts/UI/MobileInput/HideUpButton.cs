using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUpButton : MonoBehaviour
{
    public static HideUpButton Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void Start()
    {
        Instance = this;
    }
    public void Buttons_ON() 
    {
        this.gameObject.SetActive(true);
    }
    public void Buttons_OFF()
    {
        this.gameObject.SetActive(false);
    }
}
