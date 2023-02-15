using UnityEngine;
using UnityEngine.SceneManagement; //для управления сценами



public class Shop : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 5f, Screen.width / 2.5f, Screen.height / 11.5f), "plus 20 HP (Price = 20 coins)"))
        {
            if (SaveSerial.Instance.playerCoin >= 20)
            {
                SaveSerial.Instance.IncreaseMaxHP();
            }
            else
            {
                return;
            }
        }
            
        if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 3.33f, Screen.width / 2.5f, Screen.height / 11.5f), "plus 20 MP(Price = 20 coins)"))
        {
            if (SaveSerial.Instance.playerCoin >= 20)
            {
                SaveSerial.Instance.IncreaseMaxMP();
            }
            else
            {
                return;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 2.5f, Screen.width / 2.5f, Screen.height / 11.5f), "plus 10 Melee DMG (Price = 20 coins)"))
        {
            
            if (SaveSerial.Instance.playerCoin >= 20)
            {
                SaveSerial.Instance.IncreaseAttackDamage();
            }
            else
            {
                return;
            }
        }


        if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 2f, Screen.width / 2.5f, Screen.height / 11.5f), "plus 10 Mage DMG (Price = 20 coins)"))
        {
            
            if (SaveSerial.Instance.playerCoin >= 20)
            {
                SaveSerial.Instance.IncreaseMageDamage();
            }
            else
            {
                return;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 1.66f, Screen.width / 2.5f, Screen.height / 11.5f), "plus 20 Stamina (Price = 20 coins)"))
        {
            if (SaveSerial.Instance.playerCoin >= 20)
            {
                SaveSerial.Instance.IncreaseStamina();
            }
            else
            {
                return;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 1.428f, Screen.width / 2.5f, Screen.height / 11.5f), "Save and Go to the next level"))
        {
            SaveSerial.Instance.SaveGame();
            SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
        }


        //GUI.Label(new Rect(425, 65, 125, 50), "plus 20 HP (Price = 20 coins)");
        //GUI.Label(new Rect(425, 165, 125, 50), "plus 20 MP(Price = 20 coins)");
        //GUI.Label(new Rect(425, 265, 125, 50), "plus 10 Melee DMG (Price = 20 coins)");
        //GUI.Label(new Rect(425, 365, 125, 50), "plus 10 Mage DMG (Price = 20 coins)");
        //GUI.Label(new Rect(425, 465, 125, 50), "Save and Go to the next lvl");

    }
}

