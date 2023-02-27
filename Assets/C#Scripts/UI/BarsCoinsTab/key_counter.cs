using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class key_counter : MonoBehaviour
{
    bool key_ui; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero
    private void ShowKey()
    {
        if (key_ui == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    void Update() //Обновление значения происходит при обновлении каждого кадра
    {
        key_ui = LvLGeneration.Instance.key;//тут переменная hp_ui начинает быть равной тому обьекту
        ShowKey();
    }
}
