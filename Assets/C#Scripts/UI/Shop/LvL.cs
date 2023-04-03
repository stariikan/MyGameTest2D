using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class LvL : MonoBehaviour
{
    int level_ui; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        level_ui = SaveSerial.Instance.passedLvl;
        GetComponent<Text>().text = $"{level_ui}";
    }

    private void Update()
    {
        GetComponent<Text>().text = $"{level_ui}";
    }
}
