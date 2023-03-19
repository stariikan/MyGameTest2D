using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; set; }

    private void Start()
    {
        Instance = this;
    }
    public void Settings_menu_ON()
    {
        this.gameObject.SetActive(true);
    }
    public void Settings_menu_OFF()
    {
        this.gameObject.SetActive(false);
    }
}
