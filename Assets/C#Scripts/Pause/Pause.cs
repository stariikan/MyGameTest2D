using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //для управления сценами
using System.Collections;
public class Pause : MonoBehaviour
{
    public float timer;
    public bool ispause;
    public bool guipuse;
    public Texture BoxTexture; //текустуру устаналиваем сами в эдиторе (это для бокса с параметрами
    GUIContent contentPlayerAttackDamage;
    GUIContent contentPlayerMageDamage;
    GUIContent contentPassedLvl;
    GUIContent contentEnemyHP;
    GUIContent contentEnemyDamage;
    GUIContent contentEnemySpeed;

    // Характеристики указанные в BOX
    private int infoPlayerAttackDamage;
    private int infoPlayerMageDamage;
    private int infoPassedLvl;
    private int infoEnemyHP;
    private int infoEnemyDamage;
    private float infoEnemySpeed;

    void Update()
    {
        infoPlayerAttackDamage = SaveSerial.Instance.playerAttackDamage;
        infoPlayerMageDamage = SaveSerial.Instance.playerMageDamage;
        infoPassedLvl = SaveSerial.Instance.passedLvl;
        infoEnemyHP = SaveSerial.Instance.enemyHP;
        infoEnemyDamage = SaveSerial.Instance.enemyDamage;
        infoEnemySpeed = SaveSerial.Instance.enemySpeed;

        contentPlayerAttackDamage = new GUIContent("Player Attack Damage = " + $"{infoPlayerAttackDamage}", BoxTexture, "This is a tooltip");
        contentPlayerMageDamage = new GUIContent("Player Mage Damage = " + $"{infoPlayerMageDamage}", BoxTexture, "This is a tooltip");
        contentPassedLvl = new GUIContent("Passed LvL = " + $"{infoPassedLvl}", BoxTexture, "This is a tooltip");
        contentEnemyHP = new GUIContent("Enemy HP = " + $"{infoEnemyHP}", BoxTexture, "This is a tooltip");
        contentEnemyDamage = new GUIContent("Enemy Attack Damage = " + $"{infoEnemyDamage}", BoxTexture, "This is a tooltip");
        contentEnemySpeed = new GUIContent("Enemy Movement Speed = " + $"{infoEnemySpeed}", BoxTexture, "This is a tooltip");

        Time.timeScale = timer; 
        if (Input.GetKeyDown(KeyCode.Escape) && ispause == false) 
        {
            ispause = true; 
        } 
        else if (Input.GetKeyDown(KeyCode.Escape) && ispause == true) 
        {
            ispause = false; 
        }
        if (ispause == true) 
        { 
            timer = 0; guipuse = true; 
        }
        else if (ispause == false)
        {
            timer = 1f;
            guipuse = false;
        }
    }
    public void OnGUI()
    {
        if (guipuse == true)
        {
            Cursor.visible = true;// включаем отображение курсора
                                  
            if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 5f, Screen.width / 2.5f, Screen.height / 11.5f), "Продолжить")) 
            {
                ispause = false;
                timer = 0;
                Cursor.visible = false;
            } 
            if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 3.33f, Screen.width / 2.5f, Screen.height / 11.5f), "Сохранить")) 
            {
            } 
            if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 2.5f, Screen.width / 2.5f, Screen.height / 11.5f), "В Меню"))
            { 
            }
            GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 2f, Screen.width / 2.5f, Screen.height / 25f), contentPlayerAttackDamage);
            GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.81f, Screen.width / 2.5f, Screen.height / 25f), contentPlayerMageDamage);
            GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.66f, Screen.width / 2.5f, Screen.height / 25f), contentPassedLvl);
            GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.538f, Screen.width / 2.5f, Screen.height / 25f), contentEnemyHP);
            GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.428f, Screen.width / 2.5f, Screen.height / 25f), contentEnemyDamage);
            GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.33f, Screen.width / 2.5f, Screen.height / 25f), contentEnemySpeed);
        } 
    } 
}