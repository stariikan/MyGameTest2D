using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsBool : MonoBehaviour
{
    private bool settingsON = false;
    private void Update()
    {
        Main_Menu_And_Settings_Controll();
    }
    private void Main_Menu_And_Settings_Controll()
    {
        if (settingsON == true)
        {
            Main_menu.Instance.Main_menu_OFF();
            Settings.Instance.Settings_menu_ON();
        }
        if (settingsON == false)
        {
            Settings.Instance.Settings_menu_OFF();
            Main_menu.Instance.Main_menu_ON();
        }
    }
    public void OpenSettings()
    {
        settingsON = true;
    }
    public void ReturnMainMenu()
    {
        settingsON = false;
    }
}
