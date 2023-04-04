using UnityEngine;
using UnityEngine.SceneManagement; // for scene management
public class Pause : MonoBehaviour
{
    public float timer;
    public bool ispause;

    public bool guipuse;
    public Texture BoxTexture; //we set the texture ourselves in the editor

    public static Pause Instance { get; set; } // To collect and send data from this script
    // Characteristics specified in the BOX
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
        
    }
    void Update()
    {
        platform = Joystick.Instance.platform;
        GameInfo();
        ClickPause();
        GamePause();
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
        infoEnemyHP = SaveSerial.Instance.mushroomHP;
        if (infoEnemyHP == 0)
        {
            infoEnemyHP = 50;
        }
        infoEnemyDamage = SaveSerial.Instance.mushroomDamage;
        if (infoEnemyDamage == 0)
        {
            infoEnemyDamage = 7;
        }
        infoEnemySpeed = SaveSerial.Instance.mushroomSpeed;
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

    public void ContinuePlay()
    {
        ispause = false;
        timer = 0;
        Cursor.visible = false;
    }
    public void RestartGame()
    {
        SaveSerial.Instance.ResetData();
        SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void GamePause()
    {
        if (guipuse == true)
        {
            Cursor.visible = true;// switch on the cursor display
            Settings.Instance.Settings_menu_ON();

            if (platform == 1)
            {
                GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.66f, Screen.width / 2.5f, Screen.height / 11.5f), "WASD = Movement; CTRL = Attack;");
                GUI.Box(new Rect(Screen.width / 3.3f, Screen.height / 1.42f, Screen.width / 2.5f, Screen.height / 11.5f), "Alt = Magic Attack; Shift = Shield;");
            }
        }
        else
        {
            Settings.Instance.Settings_menu_OFF();
        }
    }
}