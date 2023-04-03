using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class Mana : MonoBehaviour
{
    float mana_ui; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        mana_ui = SaveSerial.Instance.playerMP;
        GetComponent<Text>().text = $"{mana_ui}";
    }
    private void Update()
    {
        mana_ui = SaveSerial.Instance.playerMP;
        GetComponent<Text>().text = $"{mana_ui}";
    }
}
