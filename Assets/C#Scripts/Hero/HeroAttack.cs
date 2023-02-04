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

    private float cooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown. Поэтому мы поставим тут бесконечность или можно поставить любое большое число
    private float MagicCooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown. Поэтому мы поставим тут бесконечность или можно поставить любое большое число
    public float maxMP = 100;
    public float currentMP;
    public float stamina = 100;
    public float currentStamina;
    public float staminaSpeedRecovery = 5f;

    public bool block = false;

    private Camera mainCamera;

    private void Start()
    {
        maxMP = SaveSerial.Instance.playerMP;
        if (maxMP == 0)
        {
            maxMP = 100;
        }
        currentMP = maxMP;

        stamina = SaveSerial.Instance.playerStamina;
        if (stamina == 0)
        {
            stamina = 100;
        }
        currentStamina = stamina;

        mainCamera = Camera.main;
    }
    private void staminaRecovery()
    {
        if (currentStamina < stamina)
        {
            currentStamina += Time.deltaTime * staminaSpeedRecovery;
        }
        if (currentStamina < 0)
        {
            currentStamina = 2;
        }
    }
    public void Block()
    {
        if (block == false)
        {
            block = true;
        }
        else
        {
            block = false;
        }
    }
    public void DecreaseStamina(float cost) //Метод для уменьшения стамины за различные действия
    {
        currentStamina -= cost;
    }
    public void Attack()
    {
       currentStamina -= 15f;
       Anim.SetTrigger("Attack");//для воспроизведения анимации атаки при выполнения тригера Attack
       cooldownTimer = 0;
       meleeAttackArea.transform.position = firePoint.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
       meleeAttackArea.GetComponent<MeleeWeapon>().meleeDirection(Mathf.Sign(transform.localScale.x));
    }
    public void magicAttack()
    {
        currentMP -= 10;
        Anim.SetTrigger("magicAttack");//для воспроизведения анимации атаки магией при выполнения тригера magicAttack
        MagicCooldownTimer = 0; //сброс кулдауна приминения магии для того чтобы работа формула при атаке которой она смотрит на кулдаун и если он наступил, то можно вновь атаковать
        magicProjectile[FindMagicBall()].transform.position = firePoint.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
                                                                                  // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world space
        Vector3 worldSpaceMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Get the direction from the shooter to the mouse
        Vector3 shootingDirection = worldSpaceMousePosition - transform.position;

        // Normalize the direction
        //shootingDirection.Normalize();
        Debug.Log(shootingDirection);
        magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(shootingDirection);
    }
    private void attackControl()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 20f)
        {
            Block();
        }
        if (Input.GetKey(KeyCode.LeftControl) && cooldownTimer > AttackCooldown && currentStamina > 15f)// если нажать на правую кнопку мыши и кулдаун таймер > чем значение AttackCooldown, то можно производить физ атаку
        {
            Attack(); // выполнения атаки
        }
        if (Input.GetKey(KeyCode.LeftAlt) && MagicCooldownTimer > magicAttackCooldown && currentMP >= 15) //если нажать на левую кнопку мыши и кулдаун таймер > чем значение MagicAttackCooldown, то можно производить атаку
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
        Instance = this;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime; //прибавление по 1 секунде к cooldownTimer после его обнуления при выполенении метода Attack.
        MagicCooldownTimer += Time.deltaTime; //прибавление по 1 секунде к MagicCooldownTimer после его обнуления при выполенении метода magicAttack.
        attackControl();//атака с помощью мышки
        staminaRecovery();
    }


}
