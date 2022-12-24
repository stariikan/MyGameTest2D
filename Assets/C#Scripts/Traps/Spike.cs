using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int trapDmg = 30;//Урон ловушки
    private float cooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown.
    private float AttackCooldown = 1.5f;//кулдаун Атаки
    private bool canAttack = false;
    private void OnCollisionEnter2D(Collision2D collision) //Если происходит соприкосновение тел
    {
        if (collision.gameObject == Hero.Instance.gameObject)//Если ловушка соприкасается именно с героем (тут получается ссылка на скрипт Hero и оттуда берется gameObject)
        {
            canAttack = true;
        }
        
    }
    void OnCollisionExit2D(Collision2D collision) //Передается, когда коллайдер другого объекта перестает соприкасаться с коллайдером этого объекта (только 2D физика).
    {
        canAttack = false;
    }
    private void trapAttack()//Метод для атаки ловушки
    {
        if (cooldownTimer > AttackCooldown & canAttack) //тут нужен кулдаун у ловушки, чтобы оно не убивало персонажа за 1 сек
        {
            cooldownTimer = 0;
            Hero.Instance.GetDamage(trapDmg); // Атака ловушки
        }
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime; //прибавление время к таймеру
        trapAttack();
    }
}
