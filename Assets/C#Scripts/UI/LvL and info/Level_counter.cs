using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//дл¤ UI

public class Level_counter : MonoBehaviour
{
    int level_ui; //тут сделали переменную чтобы она потом собирала значение переменной hp из скрипта Hero
    void Update() //ќбновление значени¤ происходит при обновлении каждого кадра
    {
        level_ui = LvLGeneration.Instance.Level;//тут переменна¤ hp_ui начинает быть равной тому обьекту
                                              //который надейт медот FindObjectOfType в скрипте hero в переменной hp
        GetComponent<Text>().text = $"{level_ui}"; //тут мы значение компонента text в game.Object к которому у нас принадлежит этот скрипт
                                                  //и мен¤ем текст на переменную котора¤ преобразуетс¤ в string
    }
}
