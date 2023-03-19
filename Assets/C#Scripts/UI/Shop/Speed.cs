using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class Speed : MonoBehaviour
{
    float speed_ui; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        speed_ui = SaveSerial.Instance.playerSpeed;
        GetComponent<Text>().text = $"{speed_ui}";
    }
    private void Update()
    {
        speed_ui = SaveSerial.Instance.playerSpeed;
        GetComponent<Text>().text = $"{speed_ui}";
    }
}
