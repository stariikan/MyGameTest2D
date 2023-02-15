﻿using UnityEngine;

public class Hero : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
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
    public int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    public static Hero Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    public float maxHP;
    public float hp; //Количество жизней
    public float stamina;

    public bool playerDead = false; //мертв игрок или нет, пока нужно для того чтобы при смерти игрока делать рестарт
    public int mageAttackDamage;

    public bool block = false;
    public bool isAttack = false;

    private BoxCollider2D boxCollider;

    private float cooldownTimer = Mathf.Infinity;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        SaveSerial.Instance.LoadGame();
        maxHP = SaveSerial.Instance.playerHP;
        if (maxHP == 0)
        {
            maxHP = 100;
        }
        hp = maxHP;
        mageAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (mageAttackDamage == 0)
        {
            mageAttackDamage = 30;
        }

        stamina = SaveSerial.Instance.playerStamina;
        if (stamina == 0)
        {
            stamina = 100;
        }
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
        stamina = HeroAttack.Instance.currentStamina; //проверка стамины
        if (hp > 0)
        {
            PlayerMovement();//Метод для движения и поворота спрайта персонажа
            DieByFall();//Метод для смерти от падения
            CheckBlock(); //Проверка блока
            Jump();
            PlayerSpeedMode();
            Roll(); //Кувырок
            if (Input.GetKey(KeyCode.LeftControl) && m_timeSinceAttack > 0.25f && !m_rolling && stamina > 15f)
            {
                MeeleAtack();
            }
        }
        else
        {
            return;
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
            m_speed = 0f;
        }
        else
        {
            m_speed = 4f;
        }
        if (cooldownTimer > 0.5f)
        {
            isAttack = false;
        }
    }
    public void CheckBlock()
    {
        block = HeroAttack.Instance.block;
        if (block == true && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }
        else
        {
            m_animator.SetBool("IdleBlock", false);
        }
    }
    public void GetDamage(int dmg) //Мы создаем новый метод GetDamage() 
    {
        if (block == false && !m_rolling)
        {
            hp -= dmg;//Отнимает int 10 из переменной hp (жизни).
            m_animator.SetTrigger("Hurt");
            //Push();
        }
        if (block == true && !m_rolling)
        {
            hp -= dmg * 0.15f;//Отнимает int из переменной hp (жизни) и при активном блоке уменьшает урон в 3 раза
            HeroAttack.Instance.DecreaseStamina(20);
            m_animator.SetTrigger("Hurt");
            //Push();
        }
        if (hp <= 0 && !m_rolling) //Если жизней меньше 0
        {
            m_speed = 0;
            m_body2d.gravityScale = 0;
            m_body2d.velocity = Vector2.zero;
            boxCollider.enabled = false;
            m_animator.StopPlayback();
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
    }
    private void Deactivate() //деактивация игрока после завершения анимации смерти (благодоря метки в аниматоре выполняется этот метод
    {
        //gameObject.SetActive(false);
        playerDead = true;
    }
    public void Hero_hp() //Метод который просто вызывает значение переменной HP, нужен мне был для передачи этого числа в скрипт с каунтером жизней
    {
        Debug.Log(hp);
    }
    private void DieByFall() //Метод который наносит урон при падении с платформы
    {
        if (m_body2d.transform.position.y < -100)//если координаты игрока по оси y меньше 10, то происходит вызов метода GetDamage
        {
            GetDamage(100);
        }
    }
    public void Jump()
    {
        //Jump
        //for jump and roll
        float joystickMoveY = JoystickMovement.Instance.moveY; //joystick
        float vertical = Input.GetAxis("Vertical"); //потом для лестниц нужно будет

        if (joystickMoveY > 0.35f && m_grounded && !m_rolling || vertical > 0.35f && m_grounded && !m_rolling)
        {
            if (stamina > 10 && cooldownTimer > 1)// если происходит нажатие и отпускания (GetKeyDown, а не просто GetKey) кнопки Space и если isGrounded = true 
            {
                cooldownTimer = 0;
                HeroAttack.Instance.DecreaseStamina(10);
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
        }
    }
    public void Roll()
    {
        float joystickMoveY = JoystickMovement.Instance.moveY; //joystick
        float vertical = Input.GetAxis("Vertical");
        //Roll
        if ((joystickMoveY < -0.35f || vertical < 0) && cooldownTimer > 1.5f && stamina > 5 && !m_rolling && !m_isWallSliding && cooldownTimer > 1) //кувырок
        {
            cooldownTimer = 0;
            HeroAttack.Instance.DecreaseStamina(5);
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2((m_facingDirection*-1) * m_rollForce, m_body2d.velocity.y);
        }
    }
    public void PlayerMovement()
    {
        float joystickMoveX = JoystickMovement.Instance.moveX; //joystick
        float move = Input.GetAxis("Horizontal");//Используем Float потому-что значение 0.111..., тут берется ввод по Горизонтали (стрелки и A D)

        //Run
        //keyboard control
        Vector2 movementX = new Vector2(move, m_body2d.velocity.y); //keyboard
        if (move != 0 && !m_rolling)
        {
            m_body2d.velocity = new Vector2(move * m_speed, m_body2d.velocity.y);
        }

        //joystick controll
        Vector2 movementJoystickX = new Vector2(joystickMoveX, m_body2d.velocity.y); //tuchpad
        if (joystickMoveX != 0 && !m_rolling)
        {
            m_body2d.velocity = new Vector2(joystickMoveX * m_speed, m_body2d.velocity.y);
        }

        //player state
        if (m_body2d.velocity != Vector2.zero)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        //Flip
        if (joystickMoveX > 0 || move > 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (joystickMoveX < 0 || move < 0) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }
    }
    public void MeeleAtack()
    {  
        if(stamina > 20 && !m_rolling && !block) 
        {
            cooldownTimer = 0;
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
            HeroAttack.Instance.Attack();
            // Reset timer
            m_timeSinceAttack = 0.0f;
        }    
    }
}
