using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    [SerializeField] private float magicAttackCooldown;//кулдаун запуска снаряда (магии)
    [SerializeField] private Transform firePointRight; //Позиция из которых будет выпущены снаряди
    [SerializeField] private Transform firePointLeft; //Позиция из которых будет выпущены снаряди
    [SerializeField] private GameObject [] magicProjectile; //Снаряды снарядов
    [SerializeField] private GameObject meleeAttackArea; // Физ оружее
    [SerializeField] private GameObject shieldArea; // Щит

    public static HeroAttack Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    
    private float MagicCooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown. Поэтому мы поставим тут бесконечность или можно поставить любое большое число
    public float maxMP = 100;
    public float currentMP;
    public float stamina = 100;
    public float currentStamina;
    public float staminaSpeedRecovery = 10f;

    public bool block = false;

    private int playerDirecction;

    private Vector3 shootingDirection;

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
        Instance = this;
    }
    private void Update()
    {
        MagicCooldownTimer += Time.deltaTime; //прибавление по 1 секунде к MagicCooldownTimer после его обнуления при выполенении метода magicAttack.
        AttackControl();//атака с помощью мышки
        StaminaRecovery();
        playerDirecction = Hero.Instance.m_facingDirection;
    }
    private void StaminaRecovery()
    {
        if (currentStamina < stamina)
        {
            currentStamina += Time.deltaTime * staminaSpeedRecovery;
        }
        if (currentStamina < 0)
        {
            currentStamina = 2;
        }
        if (currentStamina < 20)
        {
            block = false;
        }
    }
    public void Block()
    {
        if (block == false && currentStamina > 20)
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
       if(playerDirecction > 0)
        {
            meleeAttackArea.transform.position = firePointRight.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
            meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(firePointRight.position);
        }
       else if (playerDirecction < 0)
        {
            meleeAttackArea.transform.position = firePointLeft.position;
            meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(firePointLeft.position);
        }
    }
    public void MeleeWeaponOff() //отключения обьекта бомбы
    {
        MeleeWeapon.Instance.WeaponOff();
    }
    public void MagicAttack()
    {
        if(MagicCooldownTimer > magicAttackCooldown && currentMP >= 20)
        {
            currentMP -= 20;
            MagicCooldownTimer = 0; //сброс кулдауна приминения магии для того чтобы работа формула при атаке которой она смотрит на кулдаун и если он наступил, то можно вновь атаковать

            Vector3 shootingDirection = new Vector3(1, 0, 109);
            Vector3 pos = firePointRight.position;
            GameObject fireBall = Instantiate(magicProjectile[Random.Range(0, magicProjectile.Length)], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            fireBall.name = "Enemy" + Random.Range(1, 999);
            if (playerDirecction > 0)
            {
                shootingDirection = new Vector3(1, 0, 109);
            }
            if (playerDirecction < 0)
            {
                shootingDirection = new Vector3(-1, 0, 109);
            }
            fireBall.GetComponent<Projectile>().SetDirection(shootingDirection);
        }
    }
    private void AttackControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Block();
        }
        if (Input.GetKey(KeyCode.LeftAlt) && currentMP >= 15) //если нажать на левую кнопку мыши и кулдаун таймер > чем значение MagicAttackCooldown, то можно производить атаку
        {
            MagicAttack(); // выполнения маг атаки
        }
    }
    public void Enemy_Push_by_BLOCK()
    {
        currentStamina -= 5;
        if (playerDirecction > 0)
        {
            shieldArea.transform.position = firePointRight.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
            shieldArea.GetComponent<Shield>().MeleeDirection(firePointRight.position);
        }
        else if (playerDirecction < 0)
        {
            shieldArea.transform.position = firePointLeft.position;
            shieldArea.GetComponent<Shield>().MeleeDirection(firePointLeft.position);
        }
    }
}
