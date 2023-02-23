using UnityEngine;
using UnityEngine.SceneManagement; //для управления сценами
public class Pause : MonoBehaviour
{
    public float timer;
    public bool ispause;

    public bool guipuse;
    public Texture BoxTexture; //текустуру устаналиваем сами в эдиторе

    public static Pause Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    // Характеристики указанные в BOX
    private float infoPlayerAttackDamage;
    private float infoPlayerMageDamage;
    private float infoPassedLvl;
    private float infoEnemyHP;
    private float infoEnemyDamage;
    private float infoEnemySpeed;

    public bool joystick = false;

    private int platform;
    private void Start()
    {
        Instance = this;
        joystick = SaveSerial.Instance.joystick_settings;
        
    }
    void Update()
    {
        platform = Joystick.Instance.platform;
        GameInfo();
        ClickPause();
    }
    private void GameInfo()
    {
        infoPlayerAttackDamage = SaveSerial.Instance.playerAttackDamage;
        if (infoPlayerAttackDamage == 0)
        {
            infoPlayerAttackDamage = 20;
        }
        infoPlayerMageDamage = SaveSerial.Instance.playerMageDamage;
        if (infoPlayerMageDamage == 0)
        {
            infoPlayerMageDamage = 30;
        }
        infoPassedLvl = SaveSerial.Instance.passedLvl;
        if (infoPassedLvl == 0)
        {
            infoPassedLvl = 1;
        }
        infoEnemyHP = SaveSerial.Instance.moushroomHP;
        if (infoEnemyHP == 0)
        {
            infoEnemyHP = 50;
        }
        infoEnemyDamage = SaveSerial.Instance.moushroomDamage;
        if (infoEnemyDamage == 0)
        {
            infoEnemyDamage = 7;
        }
        infoEnemySpeed = SaveSerial.Instance.moushroomSpeed;
        if (infoEnemySpeed == 0)
        {
            infoEnemySpeed = 1f;
        }
    }
    public void PauseON()
    {
        Time.timeScale = timer;
        if (ispause == false)
        {
            ispause = true;
        }
        if (ispause == true)
        {
            timer = 0;
            guipuse = true;
        }
    }
    public void PauseOFF()
    {
        Time.timeScale = timer;
         if (ispause == true)
        {
            ispause = false;
        }
        if (ispause == false)
        {
            timer = 1f;
            guipuse = false;
        }
    }
    private void ClickPause()
    {
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
            timer = 0;
            guipuse = true;
        }
        else if (ispause == false)
        {
            timer = 1f;
            guipuse = false;
        }
    }
    private void ChangeJoystickSetting()
    {
        if (joystick == false)
        {
            joystick = true;
        }
        else
        {
            joystick = false;
        }
        
    }
    public void OnGUI()
    {
        if (guipuse == true)
        {
            Cursor.visible = true;// включаем отображение курсора
                                  
            if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 5f, Screen.width / 2.5f, Screen.height / 11.5f), "Continue")) 
            {
                ispause = false;
                timer = 0;
                Cursor.visible = false;
            } 
            if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 3.33f, Screen.width / 2.5f, Screen.height / 11.5f), "Save")) 
            {
            } 
            if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 2.5f, Screen.width / 2.5f, Screen.height / 11.5f), "Restart"))
            {
                SaveSerial.Instance.ResetData();
                SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
            }
            if (GUI.Button(new Rect(Screen.width / 3.3f, Screen.height / 2f, Screen.width / 2.5f, Screen.height / 11.5f), "Touchscreen Joystick " + joystick))
            {
                ChangeJoystickSetting();
            }
            if(platform == 1)
            {
                GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.66f, Screen.width / 2.5f, Screen.height / 11.5f), "WASD = Movement; CTRL = Attack;");
                GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.42f, Screen.width / 2.5f, Screen.height / 11.5f), "Alt = Magic Attack; Shift = Shield;");
            }
        } 
    } 
}