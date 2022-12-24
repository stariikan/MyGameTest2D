using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp_counter : MonoBehaviour
{
    int hp_ui; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero

    void Update() //Обновление значения происходит при обновлении каждого кадра
    {
        hp_ui = Hero.Instance.hp;//тут переменная hp_ui начинает быть равной тому обьекту
                                          //который надейт медот FindObjectOfType в скрипте hero в переменной hp
        GetComponent<Text>().text = $"{hp_ui}"; //тут мы значение компонента text в game.Object к которому у нас принадлежит этот скрипт
                                              //и меняем текст на переменную val которая преобразуется в string
    }
}
