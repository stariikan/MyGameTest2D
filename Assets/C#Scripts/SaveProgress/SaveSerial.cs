using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement; //для управления сценами

public class SaveSerial : MonoBehaviour
{
    public int playerCoin;
    public float playerHP;
    public float playerMP;
    public float playerStamina;
    public int playerAttackDamage;
    public int playerMageDamage;

    public int passedLvl;

    public int enemyHP;
    public int enemyDamage;
    public float enemySpeed;
    public static SaveSerial Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    private void Awake()
    {
        Instance = this;
    }
    //Создадим новый сериализуемый класс SaveData, который будет содержать сохраняемые данные
    [Serializable]
    class SaveData
    {
        public int playerCoin;
        public float playerHP;
        public float playerMP;
        public float playerStamina;
        public int playerAttackDamage;
        public int playerMageDamage = 30;

        public int passedLvl;

        public int enemyHP;
        public int enemyDamage;
        public float enemySpeed;
        //public float savedFloat;
        //public bool savedBool;
    }
    //Обратите внимание, три переменные в классе SaveData соответствуют переменным из класса SaveSerial.
    //Для сохранения мы будем передавать значения из SaveSerial в SaveData, а затем сериализовать последний.

    //Добавим в класс SaveSerial метод SaveGame:
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); //Объект BinaryFormatter предназначен для сериализации и десериализации.
                                                    //При сериализации он отвечает за преобразование информации в поток бинарных данных (нулей и единиц).
      
        FileStream file = File.Create(Application.persistentDataPath //FileStream и File нужны для создания файла с расширением .dat.
                                                                     //Константа Application.persistentDataPath содержит путь к файлам проекта: C:\Users\[user]\AppData\LocalLow\[company name].
          + "/MySaveData.dat");
        SaveData data = new SaveData(); //В методе SaveGame создается новый экземпляр класса SaveData. В него записываются текущие данные из SaveSerial, которые нужно сохранить.
                                        //BinaryFormatter сериализует эти данные и записывает их в файл, созданный FileStream. Затем файл закрывается, в консоль выводится сообщение об успешном сохранении.
        
        if (SceneManager.GetActiveScene().name == "startLevel")
        {
            playerCoin = LvLGeneration.Instance.coin;
            playerHP = Hero.Instance.maxHP;
            playerMP = HeroAttack.Instance.maxMP;
            playerStamina = HeroAttack.Instance.stamina;
            playerAttackDamage = MeleeWeapon.Instance.AttackDamage;
            playerMageDamage = Hero.Instance.mageAttackDamage;
            passedLvl = LvLGeneration.Instance.Level;

            enemyHP = GameObject.Find("EnemySkelet").GetComponent<Entity>().maxHP;
            enemyDamage = GameObject.Find("EnemySkelet").GetComponent<Entity>().enemyAttackDamage;
            enemySpeed = GameObject.Find("EnemySkelet").GetComponent<Enemy_Skelet>().speed;
        }
        
        data.playerCoin = playerCoin;
        data.playerHP = playerHP;
        data.playerMP = playerMP;
        data.playerStamina = playerStamina;
        data.playerAttackDamage = playerAttackDamage;
        data.playerMageDamage = playerMageDamage;
        
        data.passedLvl = passedLvl;
                
        data.enemyHP = enemyHP;
        data.enemyDamage = enemyDamage;
        data.enemySpeed = enemySpeed;
        

    //data.savedBool = boolToSave;
    bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    //Метод LoadGame – это, как и раньше, SaveGame наоборот:
       public void LoadGame()
        {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat")) //Сначала ищем файл с сохраненными данными, который мы создали в методе SaveGame.
        {
            BinaryFormatter bf = new BinaryFormatter(); //Если он существует, открываем его и десериализуем с помощью BinaryFormatter.
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file); // Передаем записанные в нем значения в переменные класса SaveSerial.
            file.Close();
            playerCoin = data.playerCoin;
            playerHP = data.playerHP;
            playerMP = data.playerMP;
            playerStamina = data.playerStamina;
            playerAttackDamage = data.playerAttackDamage;
            playerMageDamage = data.playerMageDamage;

            passedLvl = data.passedLvl;

            enemyHP = data.enemyHP;
            enemyDamage = data.enemyDamage;
            enemySpeed = data.enemySpeed;
            //boolToSave = data.savedBool;
            Debug.Log("Game data loaded!"); //Выводим в отладочную консоль сообщение об успешной загрузке.
        }
        else
            Debug.LogError("There is no save data!"); //Если файла с данными не окажется в папке проекта, выведем в консоль сообщение об ошибке.
        }
    //Сброс
    //Наконец, реализуем метод для сброса сохранения.Он похож на тот ResetData, который мы написали для очистки PlayerPrefs, но включает в себя пару дополнительных шагов.
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat")) //Сначала нужно убедиться, что файл, который мы хотим удалить, существует. 
        {
            File.Delete(Application.persistentDataPath
              + "/MySaveData.dat");
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");//Если файла нет, выводим сообщение об ошибке.
    }
    public void IncreaseMaxHP()
    {
        playerHP += 20;
        playerCoin -= 20;
    }
    public void IncreaseMaxMP()
    {
        playerMP += 20;
        playerCoin -= 20;
    }
    public void IncreaseAttackDamage()
    {
        playerAttackDamage += 10;
        playerCoin -= 20;
    }
    public void IncreaseMageDamage()
    {
        playerMageDamage += 10;
        playerCoin -= 20;
    }
    public void IncreaseStamina()
    {
        playerStamina += 20;
        playerCoin -= 20;
    }
}
