using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSerial : MonoBehaviour
{
    public int playerCoin;
    public int playerHP;
    public int playerMP;
    public int playerAttackDamage;
    public int playerMageDamage;

    public int passedLvl;

    public int enemyHP;
    public int enemyDamage;
    public float enemySpeed;
    public static SaveSerial Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void Awake()
    {
        Instance = this;
    }
    //�������� ����� ������������� ����� SaveData, ������� ����� ��������� ����������� ������
    [Serializable]
    class SaveData
    {
        public int playerCoin;
        public int playerHP;
        public int playerMP;
        public int playerAttackDamage;
        public int playerMageDamage;

        public int passedLvl;

        public int enemyHP;
        public int enemyDamage;
        public float enemySpeed;
        //public float savedFloat;
        //public bool savedBool;
    }
    //�������� ��������, ��� ���������� � ������ SaveData ������������� ���������� �� ������ SaveSerial.
    //��� ���������� �� ����� ���������� �������� �� SaveSerial � SaveData, � ����� ������������� ���������.

    //������� � ����� SaveSerial ����� SaveGame:
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); //������ BinaryFormatter ������������ ��� ������������ � ��������������.
                                                    //��� ������������ �� �������� �� �������������� ���������� � ����� �������� ������ (����� � ������).
      
        FileStream file = File.Create(Application.persistentDataPath //FileStream � File ����� ��� �������� ����� � ����������� .dat.
                                                                     //��������� Application.persistentDataPath �������� ���� � ������ �������: C:\Users\[user]\AppData\LocalLow\[company name].
          + "/MySaveData.dat");
        SaveData data = new SaveData(); //� ������ SaveGame ��������� ����� ��������� ������ SaveData. � ���� ������������ ������� ������ �� SaveSerial, ������� ����� ���������.
                                        //BinaryFormatter ����������� ��� ������ � ���������� �� � ����, ��������� FileStream. ����� ���� �����������, � ������� ��������� ��������� �� �������� ����������.
        
        playerCoin = LvLGeneration.Instance.coin;
        playerHP = Hero.Instance.maxHP;
        playerMP = HeroAttack.Instance.maxMP;
        playerAttackDamage = Projectile.Instance.magicAttackDamage;
        playerMageDamage = MeleeWeapon.Instance.AttackDamage;

        passedLvl = LvLGeneration.Instance.Level;

        enemyHP = GameObject.Find("EnemySkelet").GetComponent<Entity>().maxHP;
        enemyDamage = GameObject.Find("EnemySkelet").GetComponent<Entity>().enemyAttackDamage;
        enemySpeed = GameObject.Find("EnemySkelet").GetComponent<Enemy_Skelet>().speed;

        data.playerCoin = playerCoin;
        data.playerHP = playerHP;
        data.playerMP = playerMP;
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

    //����� LoadGame � ���, ��� � ������, SaveGame ��������:
       public void LoadGame()
        {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat")) //������� ���� ���� � ������������ �������, ������� �� ������� � ������ SaveGame.
        {
            BinaryFormatter bf = new BinaryFormatter(); //���� �� ����������, ��������� ��� � ������������� � ������� BinaryFormatter.
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file); // �������� ���������� � ��� �������� � ���������� ������ SaveSerial.
            file.Close();
            playerCoin = data.playerCoin;
            playerHP = data.playerHP;
            playerMP = data.playerMP;
            playerAttackDamage = data.playerAttackDamage;
            playerMageDamage = data.playerMageDamage;

            passedLvl = data.passedLvl;

            enemyHP = data.enemyHP;
            enemyDamage = data.enemyDamage;
            enemySpeed = data.enemySpeed;
            //boolToSave = data.savedBool;
            Debug.Log("Game data loaded!"); //������� � ���������� ������� ��������� �� �������� ��������.
        }
        else
            Debug.LogError("There is no save data!"); //���� ����� � ������� �� �������� � ����� �������, ������� � ������� ��������� �� ������.
        }
    //�����
    //�������, ��������� ����� ��� ������ ����������.�� ����� �� ��� ResetData, ������� �� �������� ��� ������� PlayerPrefs, �� �������� � ���� ���� �������������� �����.
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat")) //������� ����� ���������, ��� ����, ������� �� ����� �������, ����������. 
        {
            File.Delete(Application.persistentDataPath
              + "/MySaveData.dat");
            //playerCoin = 0;
            //playerHP = 100;
            //passedLvl = 1;
            //enemyHP = 100;
            //enemyDamage = 7;
            //enemySpeed = 1f;
            //boolToSave = false; //����� ��� �������� �������� �������� ���������� ������ SaveSerial �� ��������� � ������� ��������� � �������.
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");//���� ����� ���, ������� ��������� �� ������.
    }

}
