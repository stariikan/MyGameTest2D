using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    [SerializeField] private float magicAttackCooldown;//кулдаун запуска снаряда (магии)
    [SerializeField] private float AttackCooldown;//кулдаун Атаки (физ)
    [SerializeField] private Transform firePoint; //Позиция из которых будет выпущены снаряди
    [SerializeField] private GameObject[] magicProjectile; //Массив наших снарядов
    [SerializeField] private GameObject meleeAttackArea; // Физ оружее

    public static HeroAttack Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    public Animator Anim; //Переменная для работы с Анимацией
    public Transform attackPoint; //Тут мы ссылаемся на точку которая является дочерним обьектом игрока (нужна для реализации физ атаки)

    private float cooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown. Поэтому мы поставим тут бесконечность или можно поставить любое большое число
    private float MagicCooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown. Поэтому мы поставим тут бесконечность или можно поставить любое большое число
    public int maxMP = 100;
    public int currentMP;
    
    private void Start()
    {
        maxMP = SaveSerial.Instance.playerMP;
        if (maxMP == 0)
        {
            maxMP = 100;
        }
        currentMP = maxMP;
    }

    private void Attack()
    {
       Anim.SetTrigger("Attack");//для воспроизведения анимации атаки при выполнения тригера Attack
       cooldownTimer = 0;
       meleeAttackArea.transform.position = firePoint.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
       meleeAttackArea.GetComponent<MeleeWeapon>().meleeDirection(Mathf.Sign(transform.localScale.x));

    }
    private void magicAttack()
    {
        Anim.SetTrigger("magicAttack");//для воспроизведения анимации атаки магией при выполнения тригера magicAttack
        MagicCooldownTimer = 0; //сброс кулдауна приминения магии для того чтобы работа формула при атаке которой она смотрит на кулдаун и если он наступил, то можно вновь атаковать
        magicProjectile[FindMagicBall()].transform.position = firePoint.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
        magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private void attackControl()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButtonDown(0)) && cooldownTimer > AttackCooldown)// если нажать на правую кнопку мыши и кулдаун таймер > чем значение AttackCooldown, то можно производить физ атаку
        {
            Attack(); // выполнения атаки
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)) && MagicCooldownTimer > magicAttackCooldown && currentMP >= 10) //если нажать на левую кнопку мыши и кулдаун таймер > чем значение MagicAttackCooldown, то можно производить атаку
        {
            currentMP -= 10;
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
        Instance = this;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime; //прибавление по 1 секунде к cooldownTimer после его обнуления при выполенении метода Attack.
        MagicCooldownTimer += Time.deltaTime; //прибавление по 1 секунде к MagicCooldownTimer после его обнуления при выполенении метода magicAttack.
        attackControl();//атака с помощью мышки
    }


}
