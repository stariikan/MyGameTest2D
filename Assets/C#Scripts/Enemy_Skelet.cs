using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skelet : Entity //наследование класса сущности (то есть методы которые используются в Entity будут применены и к этому обьекту)
{
    // Start is called before the first frame update
    [SerializeField] private int hp = 30; //жизни скелета
    [SerializeField] private float speed = 3.5f; //параметр скорости скелета
    private SpriteRenderer sprite;
    private void Start()
    {
        dir = transform.right;//начальное направление движения 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject) //Если скелет соприкасается именно с героем 
                                                              //(тут получается ссылка на скрипт Hero и оттуда берется gameObject)
        {
            Hero.Instance.GetDamage(); //Из скрипта Hero вызывается публичный метод который меняет переменную hp -= 10.
            //hp -= 10; //но при этом и у скелета тратятся 10 жизней
            Debug.Log("Скелет потерял 10 жизней, осталось" + hp);//написание в логах количества жизней у скелета
        }

        if (hp < 0)//если hp меньше или равно 0
            Die();//то смерть и уничтожение gameObject, это публичный метод из скрипта Entity  
    }
    
// Update is called once per frame
void Update()
    {
        Move();
    }
}
