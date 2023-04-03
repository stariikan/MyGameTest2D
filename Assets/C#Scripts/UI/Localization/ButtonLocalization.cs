using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLocalization : MonoBehaviour
{
    public GameObject buttonName; // assign target object in inspector
    public Text textUI;
    public string[] textArray;
    private string currentText = "";

    public bool joystick_settings; //�������� ��� ������
    public bool localization; //Eng/Ru
    public bool sound; //������� �� ����
    public bool music; //�������� �� ������
    public int enemyCheat; //��� �� ��������� ������
    // Start is called before the first frame update
    void Start()
    {
        joystick_settings = SaveSerial.Instance.joystick_settings;
        localization = SaveSerial.Instance.localization;
        sound = SaveSerial.Instance.sound;
        music = SaveSerial.Instance.music;
        enemyCheat = SaveSerial.Instance.enemyCheat;

        StartButton();
        SettingsButton();
        ExtraButton();
        ExitButton();
        SoundButton();
        MusicButton();
        JoystickButton();
        LanguageButton();
        ControlButton();
        BackButton();
        ContinueButton();
        RestartButton();
    }

    // Update is called once per frame
    void Update()
    {
        joystick_settings = SaveSerial.Instance.joystick_settings;
        localization = SaveSerial.Instance.localization;
        sound = SaveSerial.Instance.sound;
        music = SaveSerial.Instance.music;
        enemyCheat = SaveSerial.Instance.enemyCheat;

        StartButton();
        SettingsButton();
        ExtraButton();
        ExitButton();
        SoundButton();
        MusicButton();
        JoystickButton();
        LanguageButton();
        ControlButton();
        BackButton();
        ContinueButton();
        RestartButton();
        EnemyCheatButton();
    }

    // ������ � ����
    private void StartButton()
    {
        if (buttonName.name == "Start")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }

        }
    }
    private void SettingsButton()
    {
        if (buttonName.name == "Settings")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }
        }
    }
    private void ExtraButton()
    {
        if (buttonName.name == "Extra")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }
        }
    }
    private void ExitButton()
    {
        if (buttonName.name == "Exit")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }
        }
    }
    private void SoundButton()
    {
        if (buttonName.name == "Sound")
        {
            if (!localization)
            {
                currentText = textArray[0];
                if (sound) textUI.text = currentText + " ON";
                if (!sound) textUI.text = currentText + " OFF";
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                if (sound) textUI.text = currentText + " ���";
                if (!sound) textUI.text = currentText + " ����";
                textUI.fontSize = 20;
            }
        }
    }
    private void MusicButton()
    {
        if (buttonName.name == "Music")
        {
            if (!localization)
            {
                currentText = textArray[0];
                if (music) textUI.text = currentText + " ON";
                if (!music) textUI.text = currentText + " OFF";
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                if (music) textUI.text = currentText + " ���";
                if (!music) textUI.text = currentText + " ����";
                textUI.fontSize = 20;
            }
        }
    }
    private void JoystickButton()
    {
        if (buttonName.name == "Joystick")
        {
            if (!localization)
            {
                currentText = textArray[0];
                if (joystick_settings) textUI.text = currentText + " ON";
                if (!joystick_settings) textUI.text = currentText + " OFF";
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                if (joystick_settings) textUI.text = currentText + " ���";
                if (!joystick_settings) textUI.text = currentText + " ����";
                textUI.fontSize = 20;
            }
        }
    }
    private void LanguageButton()
    {
        if (buttonName.name == "Language")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }
        }
    }
    private void ControlButton()
    {
        if (buttonName.name == "Control")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }
        }
    }
    private void BackButton()
    {
        if (buttonName.name == "Back")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }
        }
    }
    
    //������ � ��� ����� ������ ���� � ����� ������
    private void ContinueButton()
    {
        if (buttonName.name == "Continue")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }
        }
    }
    private void RestartButton()
    {
        if (buttonName.name == "Restart")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText;
                textUI.fontSize = 20;
            }
        }
    }
    private void EnemyCheatButton()
    {
        if (buttonName.name == "EnemyCheat")
        {
            if (!localization)
            {
                currentText = textArray[0];
                textUI.text = currentText + " " + enemyCheat;
                textUI.fontSize = 25;
            }
            if (localization)
            {
                currentText = textArray[1];
                textUI.text = currentText + " " + enemyCheat;
                textUI.fontSize = 20;
            }
        }
    }
}
