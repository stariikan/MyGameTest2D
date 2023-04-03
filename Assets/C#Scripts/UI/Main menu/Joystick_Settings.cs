using UnityEngine;
using UnityEngine.UI;//для UI

public class Joystick_Settings : MonoBehaviour
{
    bool joystick_settings; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero

    // Start is called before the first frame update
    void Start()
    {
        //SaveSerial.Instance.LoadGame();
        //joystick_settings = SaveSerial.Instance.joystick_settings;
        //GetComponent<Text>().text = "Joystick " + $"{joystick_settings}";
    }
    private void Update()
    {
        //joystick_settings = SaveSerial.Instance.joystick_settings;
       // GetComponent<Text>().text = "Joystick " + $"{joystick_settings}";
    }
}
