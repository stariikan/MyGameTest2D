using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : Hero
{
    [SerializeField] private float magicAttackCooldown;//кулдаун запуска снаряда (магии)
    [SerializeField] private float AttackCooldown;//кулдаун Атаки (физ)
    [SerializeField] private Transform firePoint; //Позиция из которых будет выпущены снаряди
    [SerializeField] private GameObject[] magicProjectile; //Массив наших снарядов

    public Animator Anim; //Переменная для работы с Анимацией
    public Transform attackPoint; //Тут мы ссылаемся на точку которая является дочерним обьектом игрока (нужна для реализации физ атаки)

    private float cooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown. Поэтому мы поставим тут бесконечность или можно поставить любое большое число
    private float MagicCooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown. Поэтому мы поставим тут бесконечность или можно поставить любое большое число

    public int attackDamage = 20; // Урон от физ атаки
    public float attackRange = 0.4f; //Дальность физ атаки
    private bool swordHit = false; //есть ли попадание мечем по цели


    private void OnDrawGizmosSelected() //позволяет отобразить круг который появляется в методе Attack
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawSphere(attackPoint.position, attackRange); //нарисовать круг (центр круга у нас Attack point, размер круга attackRange)
    }
    private void Attack()
    {
       Anim.SetTrigger("Attack");//для воспроизведения анимации атаки при выполнения тригера Attack
       cooldownTimer = 0;
       
    }
    private void magicAttack()
    {
        Anim.SetTrigger("magicAttack");//для воспроизведения анимации атаки магией при выполнения тригера magicAttack
        MagicCooldownTimer = 0; //сброс кулдауна приминения магии для того чтобы работа формула при атаке которой она смотрит на кулдаун и если он наступил, то можно вновь атаковать
        magicProjectile[FindMagicBall()].transform.position = firePoint.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
        magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private void swordDamage() // нанесения урона мечем
    {
    }
    private void attackControl()
    {
        if (Input.GetMouseButtonDown(0) && cooldownTimer > AttackCooldown)// если нажать на правую кнопку мыши и кулдаун таймер > чем значение AttackCooldown, то можно производить физ атаку
        {
            Attack(); // выполнения атаки
            swordDamage();// на несение урона от меча
        }

        if (Input.GetMouseButtonDown(1) && MagicCooldownTimer > magicAttackCooldown) //если нажать на левую кнопку мыши и кулдаун таймер > чем значение MagicAttackCooldown, то можно производить атаку
        {
            magicAttack(); // выполнения маг атаки
        }
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
        Anim = GetComponent<Animator>(); //доступ к аниматору
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime; //прибавление по 1 секунде к cooldownTimer после его обнуления при выполенении метода Attack.
        MagicCooldownTimer += Time.deltaTime; //прибавление по 1 секунде к MagicCooldownTimer после его обнуления при выполенении метода magicAttack.
        attackControl();//атака с помощью мышки
    }


}
