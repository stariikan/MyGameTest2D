using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    [SerializeField] private float magicAttackCooldown;//кулдаун запуска снар€да (магии)
    [SerializeField] private Transform firePointRight; //ѕозици€ из которых будет выпущены снар€ди
    [SerializeField] private Transform firePointLeft; //ѕозици€ из которых будет выпущены снар€ди
    [SerializeField] private GameObject[] magicProjectile; //ћассив наших снар€дов
    [SerializeField] private GameObject meleeAttackArea; // ‘из оружее

    public static HeroAttack Instance { get; set; } //ƒл€ сбора и отправки данных из этого скрипта
    
    private float MagicCooldownTimer = Mathf.Infinity; //≈сли мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown. ѕоэтому мы поставим тут бесконечность или можно поставить любое большое число
    public float maxMP = 100;
    public float currentMP;
    public float stamina = 100;
    public float currentStamina;
    private float staminaSpeedRecovery = 10f;

    public bool block = false;

    private int playerDirecction;

    private Vector3 shootingDirection;


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
        Instance = this;
        mainCamera = Camera.main;
    }
    private void Update()
    {
        MagicCooldownTimer += Time.deltaTime; //прибавление по 1 секунде к MagicCooldownTimer после его обнулени€ при выполенении метода magicAttack.
        attackControl();//атака с помощью мышки
        staminaRecovery();
        playerDirecction = Hero.Instance.m_facingDirection;
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
    public void DecreaseStamina(float cost) //ћетод дл€ уменьшени€ стамины за различные действи€
    {
        currentStamina -= cost;
    }
    public void Attack()
    {
       currentStamina -= 15f;
       if(playerDirecction > 0)
        {
            meleeAttackArea.transform.position = firePointRight.position; //ѕри каждой атаки мы будем мен€ть положени€ снар€да и задавать ей положение огневой точки получить компонент из снар€да и отправить его в направление в котором находитьс€ игрок
            meleeAttackArea.GetComponent<MeleeWeapon>().meleeDirection(firePointRight.position);
        }
       else if (playerDirecction < 0)
        {
            meleeAttackArea.transform.position = firePointLeft.position;
            meleeAttackArea.GetComponent<MeleeWeapon>().meleeDirection(firePointLeft.position);
        }
    }
    public void magicAttack()
    {
        if(MagicCooldownTimer > magicAttackCooldown)
        {
            currentMP -= 20;
            MagicCooldownTimer = 0; //сброс кулдауна приминени€ магии дл€ того чтобы работа формула при атаке которой она смотрит на кулдаун и если он наступил, то можно вновь атаковать
            if (playerDirecction > 0)
            {
                magicProjectile[FindMagicBall()].transform.position = firePointRight.position; //ѕри каждой атаки мы будем мен€ть положени€ снар€да и задавать ей положение огневой точки получить компонент из снар€да и отправить его в направление в котором находитьс€ игрок
            }
            if (playerDirecction < 0)
            {
                magicProjectile[FindMagicBall()].transform.position = firePointLeft.position;
            }

            if (playerDirecction > 0)
            {
                shootingDirection = new Vector3(1, 0, 109);
            }
            if (playerDirecction < 0)
            {
                shootingDirection = new Vector3(-1, 0, 109);
            }
            magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(shootingDirection);
        }
    }
    private void attackControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > 20f)
        {
            Block();
        }
        if (Input.GetKey(KeyCode.LeftAlt) && currentMP >= 15) //если нажать на левую кнопку мыши и кулдаун таймер > чем значение MagicAttackCooldown, то можно производить атаку
        {
            magicAttack(); // выполнени€ маг атаки
        }
    }
    private int FindMagicBall()// метод дл€ перебора огненных шаров от 0 до +1 пока не дойдет до неактивного снар€да
    {
        for (int i = 0; i < magicProjectile.Length; i++)
        {
            if (!magicProjectile[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
