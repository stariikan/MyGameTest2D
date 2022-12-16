using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skelet : Entity
{
    // Start is called before the first frame update
    [SerializeField] private int hp = 30; //жизни скелета
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject) //≈сли скелет соприкасаетс€ именно с героем 
                                                              //(тут получаетс€ ссылка на скрипт Hero и оттуда беретс€ gameObject)
        {
            Hero.Instance.GetDamage(); //»з скрипта Hero вызываетс€ публичный метод который мен€ет переменную hp -= 10.
            hp -= 10; //но при этом и у скелета трат€тс€ 10 жизней
            Debug.Log("—келет потер€л 10 жизней, осталось" + hp);//написание в логах количества жизней у скелета
        }

        if (hp < 0)//если hp меньше или равно 0
            Die();//то смерть и уничтожение gameObject, это публичный метод из скрипта Entity  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
