using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRollButton : MonoBehaviour
{
    public static HideRollButton Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

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
