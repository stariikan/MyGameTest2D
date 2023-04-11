using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hero : MonoBehaviour {
    public int platform;

    [SerializeField] float      m_jumpForce = 2.5f;
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
    public float playerAttackDamage;
    public float mageAttackDamage;

    public bool block = false;
    public bool isAttack = false;
    public bool isMovingPC = false;
    public bool isMovingJoystick = false;
    public bool isMovingButton = false;
    public bool playerDead = false; // whether the player is dead or not, for now, to make a restart when the player dies

    //Timers
    private float cooldownTimer = Mathf.Infinity;
    private float MagicCooldownTimer = Mathf.Infinity;

    //Touchscreen buttons for movement
    public bool move_Right = false;
    public bool move_Left = false;

    //Attack parameters
    public GameObject[] magicProjectile; //Snapshots
    public GameObject meleeAttackArea; // Physical Weapons
    public GameObject shieldArea; // The Shield
    public GameObject bodyFront; // bodyFront
    private float magicAttackCooldown = 2;//cooldown projectile launch (magic)
    public Transform firePointRight; //The position from which the shells will be fired
    public Transform firePointLeft; //The position from which the shells will be fired

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

    public static Hero Instance { get; set; } // To collect and send data from this script

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        SaveSerial.Instance.LoadGame();
        maxHP = SaveSerial.Instance.playerHP;
        platform = Joystick.Instance.platform;
        if (maxHP == 0) maxHP = 100;
        curentHP = maxHP;

        playerAttackDamage = SaveSerial.Instance.playerAttackDamage;
        if (playerAttackDamage == 0) playerAttackDamage = 8;

        mageAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (mageAttackDamage == 0) mageAttackDamage = 30;

        m_speed = SaveSerial.Instance.playerSpeed;
        if (m_speed == 0) m_speed = 4;
        m_curentSpeed = m_speed;

        maxMP = SaveSerial.Instance.playerMP;
        if (maxMP == 0) maxMP = 100;
        currentMP = maxMP;

        stamina = SaveSerial.Instance.playerStamina;
        if (stamina == 0) stamina = 100;
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
        cooldownTimer += Time.deltaTime; // add 1 second to cooldownTimer
        m_JumpCooldownTime += Time.deltaTime;
        MagicCooldownTimer += Time.deltaTime; //add 1 second to MagicCooldownTimer after resetting it to zero when magicAttack is executed.

        if (curentHP > 0)
        {
            PlayerMovement();//Method for moving and rotating a character sprite
            CheckBlock(); //Checking the block
            PlayerSpeedMode();
            AttackControl();
            StaminaRecovery();
            Jostick_Settings_Controll();
        }
    }
    private void Jostick_Settings_Controll()
    {
        bool joystick = SaveSerial.Instance.joystick_settings;
        if (!joystick)
        {
            JoystickPosition.Instance.Joystick_OFF();
            Movement_buttons.Instance.MovementButtons_ON();
            HideUpButton.Instance.Buttons_ON();
            HideRollButton.Instance.Buttons_ON();
        }
        if (joystick)
        {
            JoystickPosition.Instance.Joystick_ON();
            Movement_buttons.Instance.MovementButtons_OFF();
            HideUpButton.Instance.Buttons_OFF();
            HideRollButton.Instance.Buttons_OFF();
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
    public void Push() //Method for pushing the body away while taking damage
    {
        if (transform.lossyScale.x < 0) //see which way the object is rotated in x direction in the transformer 
        {
            m_body2d.AddForce(new Vector2(1f, m_body2d.velocity.y), ForceMode2D.Impulse);//Impulse means that the force will only be applied once
        }
        else
        {
            m_body2d.AddForce(new Vector2(-1f, m_body2d.velocity.y), ForceMode2D.Impulse);//Impulse means that the force will only be applied once
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
        if (cooldownTimer > 0.3f)
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
    public void GetDamage(float dmg) //We create a new GetDamage() method 
    {
        if (block == false && !m_rolling)
        {
            curentHP -= dmg;//Takes dmg from the hp (life) variable.
            takeDamageSound.GetComponent<SoundOfObject>().StopSound();
            takeDamageSound.GetComponent<SoundOfObject>().PlaySound();
            m_animator.SetTrigger("Hurt");
            //Push();
        }
        if (block == true && !m_rolling)
        {
            curentHP -= dmg * 0.15f;//Takes int from the hp (life) variable and reduces damage by a factor of 3 when the unit is active
            DecreaseStamina(20);
            m_animator.SetTrigger("Hurt");
            shieldHitSound.GetComponent<SoundOfObject>().StopSound();
            shieldHitSound.GetComponent<SoundOfObject>().PlaySound();
            //Push();
        }
        if (curentHP <= 0 && !m_rolling) //If lives are less than 0
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
    public void Jump()
    {
            if (currentStamina > 10 && m_JumpCooldownTime > 1 && m_grounded && !m_rolling && !block)// if the Space button is pressed and released (GetKeyDown, not just GetKey) and if isGrounded = true 
        {
                m_JumpCooldownTime = 0;
                DecreaseStamina(10);
                m_animator.SetTrigger("Jump");
                jumpSound.GetComponent<SoundOfObject>().StopSound();
                jumpSound.GetComponent<SoundOfObject>().PlaySound();
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                transform.position = new Vector3(transform.position.x, transform.position.y + m_jumpForce, 109f);
                m_groundSensor.Disable(0.2f);
            }
    }
    public void Roll()
    {
        if (currentStamina > 5 && cooldownTimer > 0.5f && !block && m_grounded)
        {
            cooldownTimer = 0;
            DecreaseStamina(5);
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
            float move = Input.GetAxis("Horizontal");//We use Float because the value is 0.111..., here the input is horizontal (arrows and A D)
            float vertical = Input.GetAxis("Vertical");
            if (move > 0f)
            {
                    Vector3 position = transform.position;
                    position.x += move * m_curentSpeed * Time.deltaTime;
                    transform.position = position;
                    m_facingDirection = 1;
                    isMovingPC = true;
            }
            if (move < 0f)
            {
                    Vector3 position = transform.position;
                    position.x += move * m_curentSpeed * Time.deltaTime;
                    transform.position = position;
                    m_facingDirection = -1;
                    isMovingPC = true;
            }
            if (move == 0f) isMovingPC = false;

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
            if (!block && Input.GetKey(KeyCode.LeftShift)) Block();
            if (block && !Input.GetKey(KeyCode.LeftShift)) Block();
        }
        //Joystick controll
        if (platform == 2 || platform == 0)
        {
            float joystickMoveX = JoystickMovement.Instance.moveX; //joystick
            float joystickMoveY = JoystickMovement.Instance.moveY; //joystick
            if (joystickMoveX > 0f)
            {
                 Vector3 position = transform.position;
                 position.x += 1 * m_curentSpeed * Time.deltaTime;
                 transform.position = position;
                 m_facingDirection = 1;
                 isMovingJoystick = true;
            }
            if (joystickMoveX < 0f)
            {
                    Vector3 position = transform.position;
                    position.x += -1 * m_curentSpeed * Time.deltaTime;
                    transform.position = position;
                    m_facingDirection = -1;
                    isMovingJoystick = true;
            }
            if (joystickMoveX == 0f) isMovingJoystick = false;

            //Jump
            if (joystickMoveY > 0)
            {
                Jump();
            }
            //Roll
            if (joystickMoveY < 0)
            {
                Roll();
            }
        }
        //TouchScreen Button
        if (move_Right == true && platform == 2 || move_Right == true && platform == 0)
        {
                Vector3 position = transform.position;
                position.x += 1 * m_curentSpeed * Time.deltaTime;
                transform.position = position;
                m_facingDirection = 1;
                isMovingButton = true;
        }
        if (move_Left == true && platform == 2 || move_Left == true && platform == 0)
        {
                Vector3 position = transform.position;
                position.x += -1 * m_curentSpeed * Time.deltaTime;
                transform.position = position;
                m_facingDirection = -1;
                isMovingButton = true;
        }
        
        //player state
        if (isMovingJoystick || isMovingPC || isMovingButton)
        {
            if (!m_rolling && m_grounded)runSound.GetComponent<SoundOfObject>().ContinueSound();
            if (m_rolling) runSound.GetComponent<SoundOfObject>().StopSound();
            if (!m_grounded) runSound.GetComponent<SoundOfObject>().StopSound();
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            runSound.GetComponent<SoundOfObject>().StopSound();
            m_animator.SetInteger("AnimState", 0);
        }

        //Flip
        if (m_facingDirection == 1) // if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (m_facingDirection == -1) // if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
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
        isMovingButton = false;
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
    public void DecreaseStamina(float cost) //Method for reducing stamina for various actions
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
    public void BlockAttack()
    {
        m_animator.SetTrigger("BlockAttack");
        shieldHitAttackSound.GetComponent<SoundOfObject>().StopSound();
        shieldHitAttackSound.GetComponent<SoundOfObject>().PlaySound();
        Enemy_Push_by_BLOCK();
    }
    public void MeeleAtack()
    {
        if (m_timeSinceAttack > 0.25f && !m_rolling && currentStamina > 15f) 
        {
            isAttack = true;
            cooldownTimer = 0;
            currentStamina -= 15f;
            m_currentAttack++;
            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);
            
            // Reset timer
            m_timeSinceAttack = 0.0f;
        }    
    }
    public void Attack()
    {
        if (m_facingDirection > 0)
        {
            meleeAttackArea.transform.position = firePointRight.position; //With each attack we will change projectile positions and give it a firing point position to receive the component from the projectile and send it in the direction of the player
            meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(firePointRight.position);
            MeleeWeapon.Instance.GetAttackDamageInfo(playerAttackDamage);
        }
        else if (m_facingDirection < 0)
        {
            meleeAttackArea.transform.position = firePointLeft.position;
            meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(firePointLeft.position);
            MeleeWeapon.Instance.GetAttackDamageInfo(playerAttackDamage);
        }
    }
    public void CapsuleColliderOFF()
    {
        bodyFront.GetComponent<BodyFront>().ColliderOFF();
    }
    public void MeleeWeaponOff() // deactivate the weapon object
    {
        this.gameObject.GetComponentInChildren<MeleeWeapon>().WeaponOff();
    }
    public void MagicAttack()
    {
        if (MagicCooldownTimer > magicAttackCooldown && currentMP >= 20)
        {
            currentMP -= 20;
            MagicCooldownTimer = 0; // resetting the magic application kuldun so that the formula works when attacking it looks at the kuldun and if it has come, you can attack again.

            Vector3 shootingDirection = new Vector3(1, 0, 109);
            Vector3 pos = firePointRight.position;
            GameObject fireBall = Instantiate(magicProjectile[Random.Range(0, magicProjectile.Length)], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
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
        if (Input.GetKey(KeyCode.LeftControl)) MeeleAtack(); // performing a melee attack
        if (Input.GetKey(KeyCode.LeftAlt) && currentMP >= 15) MagicAttack(); // performing a mag attack
    }
    public void Enemy_Push_by_BLOCK()
    {
        if (m_facingDirection > 0)
        {
            shieldArea.transform.position = firePointRight.position; //With each attack we will change projectile positions and give it a firing point position to receive the component from the projectile and send it in the direction of the player
            shieldArea.GetComponent<Shield>().MeleeDirection(firePointRight.position);
        }
        else if (m_facingDirection < 0)
        {
            shieldArea.transform.position = firePointLeft.position;
            shieldArea.GetComponent<Shield>().MeleeDirection(firePointLeft.position);
        }
    }
    private void Deactivate() //deactivate the player after finishing the death animation (thanks to the label in the animator this method is executed
    {
        playerDead = true;
    }
    public void AttackSound()
    {
        attackSound.GetComponent<SoundOfObject>().StopSound();
        attackSound.GetComponent<SoundOfObject>().PlaySound();
    }
}
