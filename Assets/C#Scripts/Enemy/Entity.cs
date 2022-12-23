using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHP = 100;
    public static Entity Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    int currentHP;

    private Animator anim;

    private void Start()
    {
        currentHP = maxHP;
        Instance = this;
        anim = GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
                                         //к которому привязан скрипт
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        anim.SetTrigger("damage");//анимация получения демейджа
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die() //Обьявляем публичный метод Die
    {
        //anim.SetTrigger("death");//анимация смерти
        Debug.Log("Enemy Defeat");
        Destroy(this.gameObject);//уничтожить этот игровой обьект
    }

    private void Update()
    {
        //Debug.Log(currentHP);
    }

}
