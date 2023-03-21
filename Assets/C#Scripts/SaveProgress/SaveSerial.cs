using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement; //��� ���������� �������

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
    public bool joystick_settings = false; //�������� ��� ������
    public bool localization; //Eng/Ru
    public bool sound = true; //������� �� ����
    public bool music = true; //�������� �� ������
    public static SaveSerial Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void Awake()
    {
        Instance = this;
        LoadlSetting(); //�������� ������ ����� ������������ �������� ����
    }
    //�������� ����� ������������� ����� SaveData, ������� ����� ��������� ����������� ������
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
    //�������� ��������, ��� ���������� � ������ SaveData ������������� ���������� �� ������ SaveSerial.
    //��� ���������� �� ����� ���������� �������� �� SaveSerial � SaveData, � ����� ������������� ���������.
    [Serializable]
    class SaveSettings
    {
        //Settings
        public bool joystick_settings; //�������� ��� ������
        public bool localization; //Eng/Ru
        public bool sound; //������� �� ����
        public bool music; //�������� �� ������
    }


    //������� � ����� SaveSerial ����� SaveGame:
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); //������ BinaryFormatter ������������ ��� ������������ � ��������������.
                                                    //��� ������������ �� �������� �� �������������� ���������� � ����� �������� ������ (����� � ������).
      
        FileStream file = File.Create(Application.persistentDataPath //FileStream � File ����� ��� �������� ����� � ����������� .dat.
                                                                     //��������� Application.persistentDataPath �������� ���� � ������ �������: C:\Users\[user]\AppData\LocalLow\[company name].
          + "/SessionData.dat");
        SaveData data = new SaveData(); //� ������ SaveGame ��������� ����� ��������� ������ SaveData. � ���� ������������ ������� ������ �� SaveSerial, ������� ����� ���������.
                                        //BinaryFormatter ����������� ��� ������ � ���������� �� � ����, ��������� FileStream. ����� ���� �����������, � ������� ��������� ��������� �� �������� ����������.
        
        if (SceneManager.GetActiveScene().name == "startLevel")
        {
            playerCoin = LvLGeneration.Instance.coin;
            playerHP = Hero.Instance.maxHP;
            playerMP = HeroAttack.Instance.maxMP;
            playerStamina = HeroAttack.Instance.stamina;
            playerSpeed = Hero.Instance.m_curentSpeed;
            playerAttackDamage = MeleeWeapon.Instance.AttackDamage;
            playerMageDamage = Hero.Instance.mageAttackDamage;
            passedLvl = LvLGeneration.Instance.Level;

            mushroomHP = Entity_Enemy.Instance.mushroomMaxHP;
            mushroomDamage = Entity_Enemy.Instance.mushroomAttackDamage;
            mushroomSpeed = Enemy_Behavior.Instance.moushroomSpeed;
            mushroomReward = Entity_Enemy.Instance.mushroomReward;

            flyingEyeHP = Entity_Enemy.Instance.flyingEyeMaxHP;
            flyingEyeDamage = Entity_Enemy.Instance.flyingEyeAttackDamage;
            flyingEyeSpeed = Enemy_Behavior.Instance.flyingEyeSpeed;
            flyingEyeReward = Entity_Enemy.Instance.flyingEyeReward;

            skeletonHP = Entity_Enemy.Instance.skeletonMaxHP;
            skeletonDamage = Entity_Enemy.Instance.skeletonAttackDamage;
            skeletonSpeed = Enemy_Behavior.Instance.skeletonSpeed;
            skeletonReward = Entity_Enemy.Instance.skeletonReward;

            goblinHP = Entity_Enemy.Instance.goblinMaxHP;
            goblinDamage = Entity_Enemy.Instance.goblinAttackDamage;
            goblinSpeed = Enemy_Behavior.Instance.goblinSpeed;
            goblinReward = Entity_Enemy.Instance.goblinReward;

            wizardHP = Entity_Enemy.Instance.wizardMaxHP;
            wizardDamage = Entity_Enemy.Instance.wizardAttackDamage;
            wizardSpeed = Enemy_Behavior.Instance.wizardSpeed;
            wizardReward = Entity_Enemy.Instance.wizardReward;

            martialHP = Entity_Enemy.Instance.martialMaxHP;
            martialDamage = Entity_Enemy.Instance.martialAttackDamage;
            martialSpeed = Enemy_Behavior.Instance.martialSpeed;
            martialReward = Entity_Enemy.Instance.martialReward;
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
        BinaryFormatter bf = new BinaryFormatter(); //������ BinaryFormatter ������������ ��� ������������ � ��������������.
                                                    //��� ������������ �� �������� �� �������������� ���������� � ����� �������� ������ (����� � ������).

        FileStream file = File.Create(Application.persistentDataPath //FileStream � File ����� ��� �������� ����� � ����������� .dat.
                                                                     //��������� Application.persistentDataPath �������� ���� � ������ �������: C:\Users\[user]\AppData\LocalLow\[company name].
          + "/LastSessionData.dat");
        SaveData data = new SaveData(); //� ������ SaveGame ��������� ����� ��������� ������ SaveData. � ���� ������������ ������� ������ �� SaveSerial, ������� ����� ���������.
                                        //BinaryFormatter ����������� ��� ������ � ���������� �� � ����, ��������� FileStream. ����� ���� �����������, � ������� ��������� ��������� �� �������� ����������.

        if (SceneManager.GetActiveScene().name == "startLevel")
        {
            playerCoin = LvLGeneration.Instance.coin;
            playerHP = Hero.Instance.maxHP;
            playerMP = HeroAttack.Instance.maxMP;
            playerStamina = HeroAttack.Instance.stamina;
            playerSpeed = Hero.Instance.m_curentSpeed;
            playerAttackDamage = MeleeWeapon.Instance.AttackDamage;
            playerMageDamage = Hero.Instance.mageAttackDamage;
            passedLvl = LvLGeneration.Instance.Level;

            mushroomHP = Entity_Enemy.Instance.mushroomMaxHP;
            mushroomDamage = Entity_Enemy.Instance.mushroomAttackDamage;
            mushroomSpeed = Enemy_Behavior.Instance.moushroomSpeed;
            mushroomReward = Entity_Enemy.Instance.mushroomReward;

            flyingEyeHP = Entity_Enemy.Instance.flyingEyeMaxHP;
            flyingEyeDamage = Entity_Enemy.Instance.flyingEyeAttackDamage;
            flyingEyeSpeed = Enemy_Behavior.Instance.flyingEyeSpeed;
            flyingEyeReward = Entity_Enemy.Instance.flyingEyeReward;

            skeletonHP = Entity_Enemy.Instance.skeletonMaxHP;
            skeletonDamage = Entity_Enemy.Instance.skeletonAttackDamage;
            skeletonSpeed = Enemy_Behavior.Instance.skeletonSpeed;
            skeletonReward = Entity_Enemy.Instance.skeletonReward;

            goblinHP = Entity_Enemy.Instance.goblinMaxHP;
            goblinDamage = Entity_Enemy.Instance.goblinAttackDamage;
            goblinSpeed = Enemy_Behavior.Instance.goblinSpeed;
            goblinReward = Entity_Enemy.Instance.goblinReward;

            wizardHP = Entity_Enemy.Instance.wizardMaxHP;
            wizardDamage = Entity_Enemy.Instance.wizardAttackDamage;
            wizardSpeed = Enemy_Behavior.Instance.wizardSpeed;
            wizardReward = Entity_Enemy.Instance.wizardReward;

            martialHP = Entity_Enemy.Instance.martialMaxHP;
            martialDamage = Entity_Enemy.Instance.martialAttackDamage;
            martialSpeed = Enemy_Behavior.Instance.martialSpeed;
            martialReward = Entity_Enemy.Instance.martialReward;
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
        BinaryFormatter bf = new BinaryFormatter(); //������ BinaryFormatter ������������ ��� ������������ � ��������������.
                                                    //��� ������������ �� �������� �� �������������� ���������� � ����� �������� ������ (����� � ������).

        FileStream file = File.Create(Application.persistentDataPath //FileStream � File ����� ��� �������� ����� � ����������� .dat.
                                                                     //��������� Application.persistentDataPath �������� ���� � ������ �������: C:\Users\[user]\AppData\LocalLow\[company name].
          + "/SettingsData.dat");
        SaveSettings data = new SaveSettings(); //� ������ SaveGame ��������� ����� ��������� ������ SaveData. � ���� ������������ ������� ������ �� SaveSerial, ������� ����� ���������.
                                        //BinaryFormatter ����������� ��� ������ � ���������� �� � ����, ��������� FileStream. ����� ���� �����������, � ������� ��������� ��������� �� �������� ����������.

        data.joystick_settings = joystick_settings;
        data.localization = localization;
        data.sound = sound;
        data.music = music;

        //data.savedBool = boolToSave;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Settings data saved!");
    }
    //����� LoadGame � ���, ��� � ������, SaveGame ��������:
    public void LoadGame()
        {
        if (File.Exists(Application.persistentDataPath
          + "/SessionData.dat")) //������� ���� ���� � ������������ �������, ������� �� ������� � ������ SaveGame.
        {
            BinaryFormatter bf = new BinaryFormatter(); //���� �� ����������, ��������� ��� � ������������� � ������� BinaryFormatter.
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/SessionData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file); // �������� ���������� � ��� �������� � ���������� ������ SaveSerial.
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

            Debug.Log("Game data loaded!"); //������� � ���������� ������� ��������� �� �������� ��������.
        }
        else
            Debug.LogWarning("There is no save data!"); //���� ����� � ������� �� �������� � ����� �������, ������� � ������� ��������� �� ������.
        }
    public void LoadlLastGame()
    {
        if (File.Exists(Application.persistentDataPath
          + "/LastSessionData.dat")) //������� ���� ���� � ������������ �������, ������� �� ������� � ������ SaveGame.
        {
            BinaryFormatter bf = new BinaryFormatter(); //���� �� ����������, ��������� ��� � ������������� � ������� BinaryFormatter.
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/LastSessionData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file); // �������� ���������� � ��� �������� � ���������� ������ SaveSerial.
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

            Debug.Log("Game data loaded!"); //������� � ���������� ������� ��������� �� �������� ��������.
        }
        else
            Debug.LogWarning("There is no save data!"); //���� ����� � ������� �� �������� � ����� �������, ������� � ������� ��������� �� ������.
    }
    public void LoadlSetting()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SettingsData.dat")) //������� ���� ���� � ������������ �������, ������� �� ������� � ������ SaveGame.
        {
            BinaryFormatter bf = new BinaryFormatter(); //���� �� ����������, ��������� ��� � ������������� � ������� BinaryFormatter.
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/SettingsData.dat", FileMode.Open);
            SaveSettings data = (SaveSettings)bf.Deserialize(file); // �������� ���������� � ��� �������� � ���������� ������ SaveSerial.
            file.Close();
            
            joystick_settings = data.joystick_settings;
            localization = data.localization;
            sound = data.sound;
            music = data.music;


            Debug.Log("Settings loaded!"); //������� � ���������� ������� ��������� �� �������� ��������.
        }
        else
            Debug.LogWarning("There is no save data!"); //���� ����� � ������� �� �������� � ����� �������, ������� � ������� ��������� �� ������.
    }
    //�����
    //�������, ��������� ����� ��� ������ ����������.�� ����� �� ��� ResetData, ������� �� �������� ��� ������� PlayerPrefs, �� �������� � ���� ���� �������������� �����.
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SessionData.dat")) //������� ����� ���������, ��� ����, ������� �� ����� �������, ����������. 
        {
            File.Delete(Application.persistentDataPath
              + "/SessionData.dat");
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogWarning("No save data to delete.");//���� ����� ���, ������� ��������� �� ������.
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
}
