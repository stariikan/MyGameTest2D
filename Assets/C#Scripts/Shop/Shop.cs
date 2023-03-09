using UnityEngine;
using UnityEngine.SceneManagement; //для управления сценами



public class Shop : MonoBehaviour
{
    public void PlusHP()
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
    public void PlusMP()
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
    public void PlusStamina()
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
    public void PlusMeleeDMG()
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
    public void PlusMageDMG()
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
    public void Continue()
    {
        SaveSerial.Instance.SaveGame();
        SaveSerial.Instance.SaveLastGame();
        SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
    }
}

