using UnityEngine;

public class Hero : MonoBehaviour {
    public int platform;

    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 7.5f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    public int                  m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private float               m_JumpCooldownTime;

    //Параметры Героя
    public float maxHP;
    public float curentHP;
    public float maxMP;
    public float currentMP;
    public float stamina;
    public float currentStamina;
    public float staminaSpeedRecovery = 10f;
    public float m_speed;
    public float m_curentSpeed;
    public float mageAttackDamage;

    public bool block = false;
    public bool isAttack = false;
    public bool isPush = false;


    public bool playerDead = false; //мертв игрок или нет, пока нужно для того чтобы при смерти игрока делать рестарт
    private CapsuleCollider2D capsuleCollider;

    //Таймеры
    private float cooldownTimer = Mathf.Infinity;
    private float MagicCooldownTimer = Mathf.Infinity;

    //Touchscreen buttons for movement
    public bool move_Right = false;
    public bool move_Left = false;

    //Attack parameters
    public GameObject[] magicProjectile; //Снаряды снарядов
    public GameObject meleeAttackArea; // Физ оружее
    public GameObject shieldArea; // Щит
    private float magicAttackCooldown = 2;//кулдаун запуска снаряда (магии)
    public Transform firePointRight; //Позиция из которых будет выпущены снаряди
    public Transform firePointLeft; //Позиция из которых будет выпущены снаряди

    //Sounds
    public GameObject attackSound;
    public GameObject runSound;
    public GameObject takeDamageSound;
    public GameObject dieSound;
    public GameObject jumpSound;
    public GameObject magicSound;
    public GameObject shieldHitSound;
    public GameObject shieldHitAttackSound;
    public GameObject rollSound;

    public static Hero Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        SaveSerial.Instance.LoadGame();
        maxHP = SaveSerial.Instance.playerHP;
        platform = Joystick.Instance.platform;
        if (maxHP == 0)
        {
            maxHP = 100;
        }
        curentHP = maxHP;
        mageAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (mageAttackDamage == 0)
        {
            mageAttackDamage = 30;
        }
        m_speed = SaveSerial.Instance.playerSpeed;
        if (m_speed == 0)
        {
            m_speed = 4;
        }
        m_curentSpeed = m_speed;
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
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration) 
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);
       //old
        cooldownTimer += Time.deltaTime; //прибавление по 1 секунде к cooldownTimer
        m_JumpCooldownTime += Time.deltaTime;

        MagicCooldownTimer += Time.deltaTime; //прибавление по 1 секунде к MagicCooldownTimer после его обнуления при выполенении метода magicAttack.
        AttackControl();//атака с помощью мышки
        StaminaRecovery();

        Jostick_Settings_Controll();

        if (curentHP > 0)
        {
            PlayerMovement();//Метод для движения и поворота спрайта персонажа
            CheckBlock(); //Проверка блока
            PlayerSpeedMode();
            if (Input.GetKey(KeyCode.LeftControl)) MeeleAtack();
            if (cooldownTimer > 1f) capsuleCollider.enabled = true;
        }
        else
        {
            return;
        }  
    }

    private void Jostick_Settings_Controll()
    {
        bool joystick = SaveSerial.Instance.joystick_settings;
        if (!joystick)
        {
            JoystickPosition.Instance.Joystick_OFF();
            Movement_buttons.Instance.MovementButtons_ON();
        }
        if (joystick)
        {
            JoystickPosition.Instance.Joystick_ON();
            Movement_buttons.Instance.MovementButtons_OFF();
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
    public void Push() //Метод для отталкивания тела во время получения урона
    {
        if (transform.lossyScale.x < 0) //смотрим в трансформе в какую сторону повернут по х обьект 
        {
            m_body2d.AddForce(new Vector2(0.5f, m_body2d.velocity.y), ForceMode2D.Impulse);//Импульс это значит что сила приложиться всего 1 раз
        }
        else
        {
            m_body2d.AddForce(new Vector2(-0.5f, m_body2d.velocity.y), ForceMode2D.Impulse);//Импульс это значит что сила приложиться всего 1 раз
        }
    }
    private void PlayerSpeedMode()
    {
        if (block || isAttack)
        {
            m_curentSpeed = m_speed / 2;
        }
        else
        {
            m_curentSpeed = m_speed;
        }
        if (cooldownTimer > 0.5f)
        {
            isAttack = false;
        }
    }
    public void CheckBlock()
    {
        if (block == true)
        {
            m_animator.SetBool("IdleBlock", true);
        }
        else
        {
            m_animator.SetBool("IdleBlock", false);
        }
    }
    public void GetDamage(float dmg) //Мы создаем новый метод GetDamage() 
    {
        if (block == false && !m_rolling)
        {
            curentHP -= dmg;//Отнимает dmg из переменной hp (жизни).
            takeDamageSound.GetComponent<SoundOfObject>().StopSound();
            takeDamageSound.GetComponent<SoundOfObject>().PlaySound();
            m_animator.SetTrigger("Hurt");
            Push();
        }
        if (block == true && !m_rolling)
        {
            curentHP -= dmg * 0.15f;//Отнимает int из переменной hp (жизни) и при активном блоке уменьшает урон в 3 раза
            DecreaseStamina(20);
            m_animator.SetTrigger("Hurt");
            shieldHitSound.GetComponent<SoundOfObject>().StopSound();
            shieldHitSound.GetComponent<SoundOfObject>().PlaySound();
            Push();
        }
        if (curentHP <= 0 && !m_rolling) //Если жизней меньше 0
        {
            m_curentSpeed = 0;
            m_body2d.gravityScale = 0;
            m_body2d.velocity = Vector2.zero;
            m_animator.StopPlayback();
            m_animator.SetBool("noBlood", m_noBlood);
            dieSound.GetComponent<SoundOfObject>().StopSound();
            dieSound.GetComponent<SoundOfObject>().PlaySound();
            m_animator.SetBool("dead", true);
            m_animator.SetTrigger("Death");
        }
    }
    public void Hero_hp() //Метод который просто вызывает значение переменной HP, нужен мне был для передачи этого числа в скрипт с каунтером жизней
    {
        Debug.Log(curentHP);
    }
    public void Jump()
    {
            if (stamina > 10 && m_JumpCooldownTime > 1 && m_grounded && !m_rolling && !block)// если происходит нажатие и отпускания (GetKeyDown, а не просто GetKey) кнопки Space и если isGrounded = true 
            {
                m_JumpCooldownTime = 0;
                DecreaseStamina(10);
                m_animator.SetTrigger("Jump");
                jumpSound.GetComponent<SoundOfObject>().StopSound();
                jumpSound.GetComponent<SoundOfObject>().PlaySound();
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
    }
    public void Roll()
    {
        if (stamina > 5 && cooldownTimer > 0.5f && !block) //кувырок
        {
            cooldownTimer = 0;
            DecreaseStamina(5);
            capsuleCollider.enabled = false;
            m_body2d.velocity = new Vector2((m_facingDirection * -1) * m_rollForce, m_body2d.velocity.y);
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            rollSound.GetComponent<SoundOfObject>().StopSound();
            rollSound.GetComponent<SoundOfObject>().PlaySound();
        }
    }
    public void PlayerMovement()
    {
        //Keyboard controll
        if (platform == 1 || platform == 0)
        {
            float move = Input.GetAxis("Horizontal");//Используем Float потому-что значение 0.111..., тут берется ввод по Горизонтали (стрелки и A D)
            float vertical = Input.GetAxis("Vertical");
            if (move > 0f)
            {
                if (!m_rolling)
                {
                    m_body2d.velocity = new Vector2(move * m_curentSpeed, m_body2d.velocity.y);
                    m_facingDirection = 1;
                }
            }
            if (move < 0f)
            {
                if (!m_rolling)
                {
                    m_body2d.velocity = new Vector2(move * m_curentSpeed, m_body2d.velocity.y);
                    m_facingDirection = -1;
                }
            }
            //Jump
            if (vertical > 0)
            {
                Jump();
            }
            //Roll
            if (vertical < 0)
            {
                Roll();
            }
        }
        //Joystick controll
        if (platform == 2 || platform == 0)
        {
            float joystickMoveX = JoystickMovement.Instance.moveX; //joystick
            float joystickMoveY = JoystickMovement.Instance.moveY; //joystick
            if (joystickMoveX > 0f)
            {
                if (!m_rolling)
                {
                    m_body2d.velocity = new Vector2(joystickMoveX * m_curentSpeed, m_body2d.velocity.y);
                    m_facingDirection = 1;
                }
            }
            if (joystickMoveX < 0f)
            {
                if (!m_rolling)
                {
                    m_body2d.velocity = new Vector2(joystickMoveX * m_curentSpeed, m_body2d.velocity.y);
                    m_facingDirection = -1;
                }
            }
            //Jump
            if (joystickMoveY > 0)
            {
                Jump();
            }
        }
        //TouchScreen Button
        if (move_Right == true && platform == 2 || move_Right == true && platform == 0)
        {
            if (!m_rolling)
            {
                m_body2d.velocity = new Vector2(1 * m_curentSpeed, m_body2d.velocity.y);
                m_facingDirection = 1;
            }
        }
        if (move_Left == true && platform == 2 || move_Left == true && platform == 0)
        {
            if (!m_rolling)
            {
                m_body2d.velocity = new Vector2(-1 * m_curentSpeed, m_body2d.velocity.y);
                m_facingDirection = -1;
            }
        }
        //player state
        if (m_body2d.velocity != Vector2.zero)
        {
            runSound.GetComponent<SoundOfObject>().ContinueSound();
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            runSound.GetComponent<SoundOfObject>().StopSound();
            m_animator.SetInteger("AnimState", 0);
        }

        //Flip
        if (m_facingDirection == 1) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (m_facingDirection == -1) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public void Right()
    {
        move_Right = true;
    }
    public void Left()
    {
        move_Left = true;
    }
    public void Stop()
    {
        move_Right = false;
        move_Left = false;
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
    public void DecreaseStamina(float cost) //Метод для уменьшения стамины за различные действия
    {
        currentStamina -= cost;
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
    public void MeeleAtack()
    {
        if (!m_rolling && block && m_timeSinceAttack > 0.5f && !m_rolling && stamina > 5f)
        {
            m_timeSinceAttack = 0.0f;
            m_animator.SetTrigger("BlockAttack");
            shieldHitAttackSound.GetComponent<SoundOfObject>().StopSound();
            shieldHitAttackSound.GetComponent<SoundOfObject>().PlaySound();
            Enemy_Push_by_BLOCK();
        }
            if (!m_rolling && !block && m_timeSinceAttack > 0.25f && !m_rolling && stamina > 15f) 
        {
            isAttack = true;
            m_currentAttack++;
            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);
            attackSound.GetComponent<SoundOfObject>().StopSound();
            attackSound.GetComponent<SoundOfObject>().PlaySound();
            // Reset timer
            m_timeSinceAttack = 0.0f;
        }    
    }
    public void Attack()
    {
        currentStamina -= 15f;
        if (m_facingDirection > 0)
        {
            meleeAttackArea.transform.position = firePointRight.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
            meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(firePointRight.position);
        }
        else if (m_facingDirection < 0)
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
        if (MagicCooldownTimer > magicAttackCooldown && currentMP >= 20)
        {
            currentMP -= 20;
            MagicCooldownTimer = 0; //сброс кулдауна приминения магии для того чтобы работа формула при атаке которой она смотрит на кулдаун и если он наступил, то можно вновь атаковать

            Vector3 shootingDirection = new Vector3(1, 0, 109);
            Vector3 pos = firePointRight.position;
            GameObject fireBall = Instantiate(magicProjectile[Random.Range(0, magicProjectile.Length)], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            fireBall.name = "Enemy" + Random.Range(1, 999);
            magicSound.GetComponent<SoundOfObject>().StopSound();
            magicSound.GetComponent<SoundOfObject>().PlaySound();
            if (m_facingDirection > 0)
            {
                shootingDirection = new Vector3(1, 0, 109);
            }
            if (m_facingDirection < 0)
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
        if (m_facingDirection > 0)
        {
            shieldArea.transform.position = firePointRight.position; //При каждой атаки мы будем менять положения снаряда и задавать ей положение огневой точки получить компонент из снаряда и отправить его в направление в котором находиться игрок
            shieldArea.GetComponent<Shield>().MeleeDirection(firePointRight.position);
        }
        else if (m_facingDirection < 0)
        {
            shieldArea.transform.position = firePointLeft.position;
            shieldArea.GetComponent<Shield>().MeleeDirection(firePointLeft.position);
        }
    }
    private void Deactivate() //деактивация игрока после завершения анимации смерти (благодоря метки в аниматоре выполняется этот метод
    {
        playerDead = true;
    }
}
