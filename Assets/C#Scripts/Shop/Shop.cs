using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //для управления сценами



public class Shop : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(275, 50, 125, 50), "Increase MaxHP"))
        {
            SaveSerial.Instance.IncreaseMaxHP();
        }
            
        if (GUI.Button(new Rect(275, 150, 125, 50), "Increase MaxMP"))
        {
            SaveSerial.Instance.IncreaseMaxMP();
        }
        if (GUI.Button(new Rect(275, 250, 125, 50), "IncreaseAttackDamage"))
        {
            SaveSerial.Instance.IncreaseAttackDamage();
        }


        if (GUI.Button(new Rect(275, 350, 125, 50), "IncreaseMageDamage"))
        {
            SaveSerial.Instance.IncreaseMageDamage();
        }

        if (GUI.Button(new Rect(275, 450, 125, 50), "Save & Go"))
        {
            SaveSerial.Instance.SaveGame();
            SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
        }


        GUI.Label(new Rect(425, 65, 125, 50), "plus 20 HP");
        GUI.Label(new Rect(425, 165, 125, 50), "plus 20 MP");
        GUI.Label(new Rect(425, 265, 125, 50), "plus 10 Melee DMG");
        GUI.Label(new Rect(425, 365, 125, 50), "plus 10 Mage DMG");
        GUI.Label(new Rect(425, 465, 125, 50), "Save and Go to the next lvl");

    }
}

