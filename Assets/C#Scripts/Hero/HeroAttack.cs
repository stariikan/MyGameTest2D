using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : Hero
{
    [SerializeField] private float attackCooldown;//кулдаун запуска снаряда (магии)
    [SerializeField] private Transform firePoint; //Позиция из которых будет выпущены снаряди
    [SerializeField] private GameObject[] magicProjectile; //Массив наших снарядов

    private float cooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown.
                                                  //Поэтому мы поставим тут бесконечность или можно поставить любое большое число
    private Animator magicAnim;
    //private PlayerMovement playerMovement;

    private void magicAttack()
    {
        magicAnim.SetTrigger("magicAttack");//для воспроизведения анимации атаки магией при выполнения тригера magicAttack
        cooldownTimer = 0; //сброс кулдауна приминения магии для того чтобы работа формула при атаке которой она смотрит на кулдаун и если он наступил, то можно вновь атаковать

        magicProjectile[FindMagicBall()].transform.position = firePoint.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки
                                                                    //получить компонент из снаряда и отправить его в направление в котором находиться игрок
        magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindMagicBall()// метод для перебора огненных шаров от 0 до +1 пока не дойдет до неактивного снаряда
    {
        for (int i = 0; i < magicProjectile.Length; i++)
        {
            if (!magicProjectile[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private void Awake()
    {
        magicAnim = GetComponent<Animator>(); //доступ к аниматору
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && cooldownTimer > attackCooldown) //если нажать на левую кнопку мыши и кулдаун таймер > чем значение AttackCooldown, то можно производить атаку
        magicAttack(); // выполнения анимации маг атаки
        cooldownTimer += Time.deltaTime; //прибавление по 1 секунде к cooldownTimer после его обнуления при выполенении метода magicAttack.
        //Debug.Log(magicAnim);
    }


}
