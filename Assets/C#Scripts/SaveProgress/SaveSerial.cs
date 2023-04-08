using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement; // for scene management

public class SaveSerial : MonoBehaviour
{
    public int playerCoin;
    public float playerHP;
    public float playerMP;
    public float playerStamina;
    public float playerSpeed;
    public float playerAttackDamage;
    public float playerMageDamage;

    public int passedLvl;

    public float mushroomHP;
    public float mushroomDamage;
    public float mushroomSpeed;
    public int mushroomReward;

    public float flyingEyeHP;
    public float flyingEyeDamage;
    public float flyingEyeSpeed;
    public int flyingEyeReward;

    public float skeletonHP;
    public float skeletonDamage;
    public float skeletonSpeed;
    public int skeletonReward;

    public float goblinHP;
    public float goblinDamage;
    public float goblinSpeed;
    public int goblinReward;

    public float wizardHP;
    public float wizardDamage;
    public float wizardSpeed;
    public int wizardReward;

    public float martialHP;
    public float martialDamage;
    public float martialSpeed;
    public int martialReward;

    //Settings
    public bool joystick_settings = false; //Joystick or buttons
    public bool localization; //Eng/Ru
    public bool sound = true; //The sound is switched on
    public bool music = true; //The music is on
    public int enemyCheat; //cheat to generate enemies
    public static SaveSerial Instance { get; set; } // To collect and send data from this script

    private void Awake()
    {
        Instance = this;
        LoadlSetting(); //loading data from previously set game settings
    }
    //Create a new serializable SaveData class to contain the data to be saved
    [Serializable]
    class SaveData
    {
        public int playerCoin;
        public float playerHP;
        public float playerMP;
        public float playerStamina;
        public float playerSpeed;
        public float playerAttackDamage;
        public float playerMageDamage;

        public int passedLvl;

        public float mushroomHP;
        public float mushroomDamage;
        public float mushroomSpeed;
        public int mushroomReward;

        public float flyingEyeHP;
        public float flyingEyeDamage;
        public float flyingEyeSpeed;
        public int flyingEyeReward;

        public float skeletonHP;
        public float skeletonDamage;
        public float skeletonSpeed;
        public int skeletonReward;

        public float goblinHP;
        public float goblinDamage;
        public float goblinSpeed;
        public int goblinReward;

        public float wizardHP;
        public float wizardDamage;
        public float wizardSpeed;
        public int wizardReward;

        public float martialHP;
        public float martialDamage;
        public float martialSpeed;
        public int martialReward;
    }
    //notice that the three variables in the SaveData class correspond to variables from the SaveSerial class.
    //We will pass values from SaveSerial to SaveData and then serialise the latter.
    [Serializable]
    class SaveSettings
    {
        //Settings
        public bool joystick_settings; //Joystick or buttons
        public bool localization; //Eng/Ru
        public bool sound; //The sound is switched on
        public bool music; //The music is on
        public int enemyCheat; //chit to generate enemies
    }


    //Add the SaveGame method to the SaveSerial class:
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); //The BinaryFormatter object is designed for serialization and deserialization.
                                                    //In serialisation it is responsible for converting the information into a stream of binary data (zeros and ones).

        FileStream file = File.Create(Application.persistentDataPath //FileStream and File are needed to create a file with a .dat extension.
                                                                     //The Application.persistentDataPath constant contains the path to the project files: C:\Users\[user]\AppData\LocalLow\[company name].
          + "/SessionData.dat");
        SaveData data = new SaveData(); //The SaveGame method creates a new instance of the SaveData class. The current data from SaveSerial to be saved is written into it.
                                        //BinaryFormatter serializes this data and writes it to the file created by FileStream. The file is then closed and a successful save message is displayed in the console.

        if (SceneManager.GetActiveScene().name == "startLevel")
        {
            playerCoin = LvLGeneration.Instance.coin;
            playerHP = Hero.Instance.maxHP;
            playerMP = Hero.Instance.maxMP;
            playerStamina = Hero.Instance.stamina;
            playerSpeed = Hero.Instance.m_speed;
            playerAttackDamage = MeleeWeapon.Instance.AttackDamage;
            playerMageDamage = Hero.Instance.mageAttackDamage;
            passedLvl = LvLGeneration.Instance.Level;

            mushroomHP = Enemy_Behavior.Instance.mushroomMaxHP;
            mushroomDamage = Enemy_Behavior.Instance.mushroomAttackDamage;
            mushroomSpeed = Enemy_Behavior.Instance.moushroomSpeed;
            mushroomReward = Enemy_Behavior.Instance.mushroomReward;

            flyingEyeHP = Enemy_Behavior.Instance.flyingEyeMaxHP;
            flyingEyeDamage = Enemy_Behavior.Instance.flyingEyeAttackDamage;
            flyingEyeSpeed = Enemy_Behavior.Instance.flyingEyeSpeed;
            flyingEyeReward = Enemy_Behavior.Instance.flyingEyeReward;

            skeletonHP = Enemy_Behavior.Instance.skeletonMaxHP;
            skeletonDamage = Enemy_Behavior.Instance.skeletonAttackDamage;
            skeletonSpeed = Enemy_Behavior.Instance.skeletonSpeed;
            skeletonReward = Enemy_Behavior.Instance.skeletonReward;

            goblinHP = Enemy_Behavior.Instance.goblinMaxHP;
            goblinDamage = Enemy_Behavior.Instance.goblinAttackDamage;
            goblinSpeed = Enemy_Behavior.Instance.goblinSpeed;
            goblinReward = Enemy_Behavior.Instance.goblinReward;

            wizardHP = Enemy_Behavior.Instance.wizardMaxHP;
            wizardDamage = Enemy_Behavior.Instance.wizardAttackDamage;
            wizardSpeed = Enemy_Behavior.Instance.wizardSpeed;
            wizardReward = Enemy_Behavior.Instance.wizardReward;

            martialHP = Enemy_Behavior.Instance.martialMaxHP;
            martialDamage = Enemy_Behavior.Instance.martialAttackDamage;
            martialSpeed = Enemy_Behavior.Instance.martialSpeed;
            martialReward = Enemy_Behavior.Instance.martialReward;
        }
        
        data.playerCoin = playerCoin;
        data.playerHP = playerHP;
        data.playerMP = playerMP;
        data.playerStamina = playerStamina;
        data.playerSpeed = playerSpeed;
        data.playerAttackDamage = playerAttackDamage;
        data.playerMageDamage = playerMageDamage;
        
        data.passedLvl = passedLvl;
                
        data.mushroomHP = mushroomHP;
        data.mushroomDamage = mushroomDamage;
        data.mushroomSpeed = mushroomSpeed;
        data.mushroomReward = mushroomReward;

        data.flyingEyeHP = flyingEyeHP;
        data.flyingEyeDamage = flyingEyeDamage;
        data.flyingEyeSpeed = flyingEyeSpeed;
        data.flyingEyeReward = flyingEyeReward;

        data.skeletonHP = skeletonHP;
        data.skeletonDamage = skeletonDamage;
        data.skeletonSpeed = skeletonSpeed;
        data.skeletonReward = skeletonReward;

        data.goblinHP = goblinHP;
        data.goblinDamage = goblinDamage;
        data.goblinSpeed = goblinSpeed;
        data.goblinReward = goblinReward;

        data.wizardHP = wizardHP;
        data.wizardDamage = wizardDamage;
        data.wizardSpeed = wizardSpeed;
        data.wizardReward = wizardReward;

        data.martialHP = martialHP;
        data.martialDamage = martialDamage;
        data.martialSpeed = martialSpeed;
        data.martialReward = martialReward;


        //data.savedBool = boolToSave;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }
    public void SaveLastGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); //The BinaryFormatter object is intended for serialisation and deserialisation.
                                                    //In serialisation it is responsible for converting the information into a stream of binary data (zeros and ones).

        FileStream file = File.Create(Application.persistentDataPath //FileStream and File are needed to create a file with a .dat extension.
                                                                     //The Application.persistentDataPath constant contains the path to the project files: C:\Users\[user]\AppData\LocalLow\[company name].
          + "/LastSessionData.dat");
        SaveData data = new SaveData(); //The SaveGame method creates a new instance of the SaveData class. The current data from SaveSerial to be saved is written into it.
                                        //BinaryFormatter serializes this data and writes it to the file created by FileStream. The file is then closed and a successful save message is displayed in the console.

        if (SceneManager.GetActiveScene().name == "startLevel")
        {
            playerCoin = LvLGeneration.Instance.coin;
            playerHP = Hero.Instance.maxHP;
            playerMP = Hero.Instance.maxMP;
            playerStamina = Hero.Instance.stamina;
            playerSpeed = Hero.Instance.m_curentSpeed;
            playerAttackDamage = MeleeWeapon.Instance.AttackDamage;
            playerMageDamage = Hero.Instance.mageAttackDamage;
            passedLvl = LvLGeneration.Instance.Level;

            mushroomHP = Enemy_Behavior.Instance.mushroomMaxHP;
            mushroomDamage = Enemy_Behavior.Instance.mushroomAttackDamage;
            mushroomSpeed = Enemy_Behavior.Instance.moushroomSpeed;
            mushroomReward = Enemy_Behavior.Instance.mushroomReward;

            flyingEyeHP = Enemy_Behavior.Instance.flyingEyeMaxHP;
            flyingEyeDamage = Enemy_Behavior.Instance.flyingEyeAttackDamage;
            flyingEyeSpeed = Enemy_Behavior.Instance.flyingEyeSpeed;
            flyingEyeReward = Enemy_Behavior.Instance.flyingEyeReward;

            skeletonHP = Enemy_Behavior.Instance.skeletonMaxHP;
            skeletonDamage = Enemy_Behavior.Instance.skeletonAttackDamage;
            skeletonSpeed = Enemy_Behavior.Instance.skeletonSpeed;
            skeletonReward = Enemy_Behavior.Instance.skeletonReward;

            goblinHP = Enemy_Behavior.Instance.goblinMaxHP;
            goblinDamage = Enemy_Behavior.Instance.goblinAttackDamage;
            goblinSpeed = Enemy_Behavior.Instance.goblinSpeed;
            goblinReward = Enemy_Behavior.Instance.goblinReward;

            wizardHP = Enemy_Behavior.Instance.wizardMaxHP;
            wizardDamage = Enemy_Behavior.Instance.wizardAttackDamage;
            wizardSpeed = Enemy_Behavior.Instance.wizardSpeed;
            wizardReward = Enemy_Behavior.Instance.wizardReward;

            martialHP = Enemy_Behavior.Instance.martialMaxHP;
            martialDamage = Enemy_Behavior.Instance.martialAttackDamage;
            martialSpeed = Enemy_Behavior.Instance.martialSpeed;
            martialReward = Enemy_Behavior.Instance.martialReward;
        }

        data.playerCoin = playerCoin;
        data.playerHP = playerHP;
        data.playerMP = playerMP;
        data.playerStamina = playerStamina;
        data.playerSpeed = playerSpeed;
        data.playerAttackDamage = playerAttackDamage;
        data.playerMageDamage = playerMageDamage;

        data.passedLvl = passedLvl;

        data.mushroomHP = mushroomHP;
        data.mushroomDamage = mushroomDamage;
        data.mushroomSpeed = mushroomSpeed;
        data.mushroomReward = mushroomReward;

        data.flyingEyeHP = flyingEyeHP;
        data.flyingEyeDamage = flyingEyeDamage;
        data.flyingEyeSpeed = flyingEyeSpeed;
        data.flyingEyeReward = flyingEyeReward;

        data.skeletonHP = skeletonHP;
        data.skeletonDamage = skeletonDamage;
        data.skeletonSpeed = skeletonSpeed;
        data.skeletonReward = skeletonReward;

        data.goblinHP = goblinHP;
        data.goblinDamage = goblinDamage;
        data.goblinSpeed = goblinSpeed;
        data.goblinReward = goblinReward;

        data.wizardHP = wizardHP;
        data.wizardDamage = wizardDamage;
        data.wizardSpeed = wizardSpeed;
        data.wizardReward = wizardReward;

        data.martialHP = martialHP;
        data.martialDamage = martialDamage;
        data.martialSpeed = martialSpeed;
        data.martialReward = martialReward;


        //data.savedBool = boolToSave;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }
    public void SaveSetting()
    {
        BinaryFormatter bf = new BinaryFormatter(); //The BinaryFormatter object is intended for serialisation and deserialisation.
                                                    //In serialisation it is responsible for converting the information into a stream of binary data (zeros and ones).

        FileStream file = File.Create(Application.persistentDataPath //FileStream and File are needed to create a file with a .dat extension.
                                                                     //The Application.persistentDataPath constant contains the path to the project files: C:\Users\[user]\AppData\LocalLow\[company name].
          + "/SettingsData.dat");
        SaveSettings data = new SaveSettings(); //The SaveGame method creates a new instance of the SaveData class. The current data from SaveSerial to be saved is written into it.
                                                //BinaryFormatter serializes this data and writes it to the file created by FileStream. The file is then closed and a successful save message is displayed in the console.

        data.joystick_settings = joystick_settings;
        data.localization = localization;
        data.sound = sound;
        data.music = music;
        data.enemyCheat = enemyCheat;

        //data.savedBool = boolToSave;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Settings data saved!");
    }
    //The LoadGame method is, as before, SaveGame in reverse:
    public void LoadGame()
        {
        if (File.Exists(Application.persistentDataPath
          + "/SessionData.dat")) //First, look for the file with the saved data that we created in the SaveGame method.
        {
            BinaryFormatter bf = new BinaryFormatter(); //If it exists, open it and deserialise it with BinaryFormatter.
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/SessionData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file); // Передаем записанные в нем значения в переменные класса SaveSerial.
            file.Close();
            playerCoin = data.playerCoin;
            playerHP = data.playerHP;
            playerMP = data.playerMP;
            playerStamina = data.playerStamina;
            playerSpeed = data.playerSpeed;
            playerAttackDamage = data.playerAttackDamage;
            playerMageDamage = data.playerMageDamage;

            passedLvl = data.passedLvl;

            mushroomHP = data.mushroomHP;
            mushroomDamage = data.mushroomDamage;
            mushroomSpeed = data.mushroomSpeed;
            mushroomReward = data.mushroomReward;

            flyingEyeHP = data.flyingEyeHP;
            flyingEyeDamage = data.flyingEyeDamage;
            flyingEyeSpeed = data.flyingEyeSpeed;
            flyingEyeReward = data.flyingEyeReward;

            skeletonHP = data.skeletonHP;
            skeletonDamage = data.skeletonDamage;
            skeletonSpeed = data.skeletonSpeed;
            skeletonReward = data.skeletonReward;

            goblinHP = data.goblinHP;
            goblinDamage = data.goblinDamage;
            goblinSpeed = data.goblinSpeed;
            goblinReward = data.goblinReward;

            wizardHP = data.wizardHP;
            wizardDamage = data.wizardDamage;
            wizardSpeed = data.wizardSpeed;
            wizardReward = data.wizardReward;

            martialHP = data.martialHP;
            martialDamage = data.martialDamage;
            martialSpeed = data.martialSpeed;
            martialReward = data.martialReward;

            Debug.Log("Game data loaded!"); //Put a message on the debug console stating that the download was successful.
        }
        else
            Debug.LogWarning("There is no save data!"); //If the data file is not in the project folder, the console will display an error message.
    }
    public void LoadlLastGame()
    {
        if (File.Exists(Application.persistentDataPath
          + "/LastSessionData.dat")) //First, look for the file with the saved data that we created in the SaveGame method.
        {
            BinaryFormatter bf = new BinaryFormatter(); //If it exists, open it and deserialise it with BinaryFormatter.
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/LastSessionData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file); // Pass the values written in it to the SaveSerial class variables.
            file.Close();
            playerCoin = data.playerCoin;
            playerHP = data.playerHP;
            playerMP = data.playerMP;
            playerStamina = data.playerStamina;
            playerSpeed = data.playerSpeed;
            playerAttackDamage = data.playerAttackDamage;
            playerMageDamage = data.playerMageDamage;

            passedLvl = data.passedLvl;

            mushroomHP = data.mushroomHP;
            mushroomDamage = data.mushroomDamage;
            mushroomSpeed = data.mushroomSpeed;
            mushroomReward = data.mushroomReward;

            flyingEyeHP = data.flyingEyeHP;
            flyingEyeDamage = data.flyingEyeDamage;
            flyingEyeSpeed = data.flyingEyeSpeed;
            flyingEyeReward = data.flyingEyeReward;

            skeletonHP = data.skeletonHP;
            skeletonDamage = data.skeletonDamage;
            skeletonSpeed = data.skeletonSpeed;
            skeletonReward = data.skeletonReward;

            goblinHP = data.goblinHP;
            goblinDamage = data.goblinDamage;
            goblinSpeed = data.goblinSpeed;
            goblinReward = data.goblinReward;

            wizardHP = data.wizardHP;
            wizardDamage = data.wizardDamage;
            wizardSpeed = data.wizardSpeed;
            wizardReward = data.wizardReward;

            martialHP = data.martialHP;
            martialDamage = data.martialDamage;
            martialSpeed = data.martialSpeed;
            martialReward = data.martialReward;

            Debug.Log("Game data loaded!"); //Put a message on the debug console stating that the download was successful.
        }
        else
            Debug.LogWarning("There is no save data!"); //If the data file is not in the project folder, the console will display an error message.
    }
    public void LoadlSetting()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SettingsData.dat")) //First, look for the file with the saved data that we created in the SaveGame method.
        {
            BinaryFormatter bf = new BinaryFormatter(); //If it exists, open it and deserialise it with BinaryFormatter.
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/SettingsData.dat", FileMode.Open);
            SaveSettings data = (SaveSettings)bf.Deserialize(file); // Pass the values written in it to the SaveSerial class variables.
            file.Close();
            
            joystick_settings = data.joystick_settings;
            localization = data.localization;
            sound = data.sound;
            music = data.music;
            enemyCheat = data.enemyCheat;

            Debug.Log("Settings loaded!"); //Put a message on the debug console stating that the download was successful.
        }
        else
            Debug.LogWarning("There is no save data!"); //If the data file is not in the project folder, the console will display an error message.
    }
    //Reset
    //Finally, let's implement a method to reset the save.This is similar to the ResetData we wrote to clear PlayerPrefs, but includes a couple of extra steps.
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SessionData.dat")) //First, make sure that the file we want to delete exists. 
        {
            File.Delete(Application.persistentDataPath
              + "/SessionData.dat");
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogWarning("No save data to delete.");//If the file does not exist, output an error message.
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
    public void IncreaseStamina()
    {
        playerStamina += 20;
        playerCoin -= 20;
    }
    public void IncreaseSpeed()
    {
        playerSpeed += 0.25f;
        playerCoin -= 20;
    }
    public void IncreaseAttackDamage()
    {
        playerAttackDamage += 5;
        playerCoin -= 20;
    }
    public void IncreaseMageDamage()
    {
        playerMageDamage += 5;
        playerCoin -= 20;
    }
    public void ChangeJoystickSetting()
    {
        if (joystick_settings == false)
        {
            joystick_settings = true;
        }
        else
        {
            joystick_settings = false;
        }
    }
    public void ChangeLocalizationSetting()
    {
        if (localization == false)
        {
            localization = true;
        }
        else
        {
            localization = false;
        }
    }
    public void ChangeMuiscSetting()
    {
        if (music == false)
        {
            music = true;
        }
        else
        {
            music = false;
        }
    }
    public void ChangeSoundSetting()
    {
        if (sound == false)
        {
            sound = true;
        }
        else
        {
            sound = false;
        }
    }
    public void EnemyCheatSetting()
    {
        if (enemyCheat == 6)
        {
           enemyCheat = -1;
        }
        enemyCheat += 1;
    }
}
