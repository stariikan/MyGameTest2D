using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public static Buttons Instance { get; set; }

    private void Start()
    {
        Instance = this;
    }
    public void Buttons_menu_ON()
    {
        this.gameObject.SetActive(true);
    }
    public void Buttons_menu_OFF()
    {
        this.gameObject.SetActive(false);
    }
}
