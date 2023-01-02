using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class mp_counter : MonoBehaviour
{
    int mp_ui; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero
    void Update() //Обновление значения происходит при обновлении каждого кадра
    {
        mp_ui = HeroAttack.Instance.currentMP;//тут переменная hp_ui начинает быть равной тому обьекту
        GetComponent<Text>().text = $"{mp_ui}"; //тут мы значение компонента text в game.Object к которому у нас принадлежит этот скрипт
                                                //и меняем текст на переменную которая преобразуется в string
    }
}
