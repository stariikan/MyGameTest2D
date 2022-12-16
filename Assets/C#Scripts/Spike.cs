using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) //Если происходит соприкосновение тел
    { 
       if (collision.gameObject == Hero.Instance.gameObject)//Если ловушка соприкасается именно с героем
                                                            //(тут получается ссылка на скрипт Hero и оттуда берется gameObject)
        {
            Hero.Instance.GetDamage();//Из скрипта Hero вызывается публичный метод который меняет переменную hp -= 10.
        }
    }

}
