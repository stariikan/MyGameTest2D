using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class Stamina : MonoBehaviour
{
    float stamina_ui; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        stamina_ui = SaveSerial.Instance.playerStamina;
        GetComponent<Text>().text = $"{stamina_ui}";
    }
    private void Update()
    {
        stamina_ui = SaveSerial.Instance.playerStamina;
        GetComponent<Text>().text = $"{stamina_ui}";
    }
}
