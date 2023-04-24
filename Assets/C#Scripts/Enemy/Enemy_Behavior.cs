using System.Collections;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
{
    //Enemy parameters
    public float enemyMaxHP = 48; // Maximum skeleton lives
    public float enemyAttackDamage = 10; // Damage from physical attack
    public float enemySpeed = 2f;//Skeleton speed
    private float blockDMG;
    public int countOfCopy; // initially 0, when the call occurs become 3, as copies die 
    public int remainingAmmo = 3; // Bombs in stock
    public int enemyReward = 2;// reward for defeating the enemy

    //Enemy movement and attack configuration
    public float patrolDistance; // How far the object walks in standby mode. For example, starts from the leftmost point, walks 10 meters to the right, returns to the starting position. Then starts over.
    public float sightDistance; // How far away the enemy notices the player and triggers movement to the player.
    public float alarmPause; // The pause between the moment when the skeleton loses sight of the player and returns to standby mode.
    public float attackDistance; //Range of physical attack
    public float flipEnemyTimer; //Time to turn Enemy
    public float flyAmplitude; // Body Lift Force
    public float LowFlightPoint; // the height at which the body will be lifted
    public float stunDuration; // Stun duration. How many seconds the object will be stunned.
    public float vulnerableBeforeDamage; // The time between the swing and the infliction of damage. How many seconds enemy will be vulnerable before attacking.
    public float vulnerableAfterDamage; // How many seconds the enemy will be vulnerable after taking damage.
    public float jumpForce;
    public float rollForce;
    public float FlipPauseDefault; //пауза перед тем как враг развернётся на 180°.
    //public float FlipPauseRoll;  //пауза перед тем как враг развернётся на 180° когда игрок перекатился врагу за спину.

    //Enemy states
    private bool movement = false; //mob is not chasing the player
    private bool inAttack = false; //If an object prepare to attack (need for special attack move, this bool disable EnemyMovement method if true)
    private bool isAttack = false; //If an object (enemy) is attacking
    private bool stuned = false; //state of stun
    private bool jump = false;
    private bool isBlock; //check whether the block is set


    public bool isFlying; //Flying enemy or not
    public bool isBlooded; //Does the enemy have blood
    public bool enemyDead = false; // Is the object dead
    public bool isAttacked = false; //If the object has sustained damage
    public bool enemyTakeDamage = false; //If enemy take damage
    public bool block = false; //state of block
    public bool copy = false; // is this object a copy or not?

    //Enemy cooldowns
    private float jumpCooldown; //cooldown on rebound and jump
    private float physicCooldown = Mathf.Infinity; //cooldown on physical attack
    private float magicCooldown = Mathf.Infinity; //cooldown on mage attack
    private float stunCooldown; //stun recovery
    private float blockCooldown; //сooldown block
    private float alarmFollowTimer = Mathf.Infinity; //How much time has passed since the loss of the player to the object
    private float alarmPatrolTimer = Mathf.Infinity; //How much time has passed since the loss of the player to the object
    private float vulnerableAttackTimer; //timer for switching from one attack state to another attack state
    private float colliderONTimer = Mathf.Infinity;
    private float enemyTakenDamageTimer = Mathf.Infinity;

    //Other parameters
    public float currentHP; // current Hp of the object
    public float takedDamage; // Damage caused to the object
    public float currentAttackDamage; //current damage of the object
    private float speedRecovery;//need to restore speed 
    private float currentAttack = 0; //cooldown on object attack (animation)
    private float timeSinceAttack = 0.0f;// time since last attack, needed for combo attack animation
    private float e_delayToIdle = 0.0f;
    private int aState; // State of attack need for separate attack animation
    private int level; //check what level the player is at, to connect abilities
    public int e_facingDirection = 1;
    new string tag; // the object tag is assigned to this variable at the start
    public Vector3 lossyScale;

    private int patrolFlip = 1;
    public Vector3 startPosition; //start position
    public float patrolDirectionLeft;
    public float patrolDirectionRight;
    public float sightDistanceLeft;
    public float sightDistanceRight;
    private bool playerGodMode;

    public Rigidbody2D rb; //Physical body
    private CapsuleCollider2D capsuleCollider;
    private Animator anim; //Variable by which the object is animated
    private GameObject masterEnemy; //this will link to the eye wizard who calls on the other eyes
    public GameObject[] ammo; // Enemy attack shells
    public GameObject[] blood; //blood
    public GameObject player; //For identifying the player on the scene
    public GameObject meleeAttackArea; // Physical Weapons
    public Transform firePoint; //The position from which the shells will be fired

    //Variable to record the coordinate difference between player and enemy
    public float directionX;
    public float directionY;

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

    public static Enemy_Behavior Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    private void Start()
    {
        Instance = this;
        rb = this.gameObject.GetComponent<Rigidbody2D>(); // The rb variable gets the Rigidbody2D component (physics.Object) to which the script is bound
        anim = this.gameObject.GetComponent<Animator>(); // The anim variable gets information from the Animator component (animation.Object) to which the script is bound
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        tag = this.gameObject.transform.tag;
        level = LvLGeneration.Instance.Level;
        currentHP = enemyMaxHP;
        currentAttackDamage = enemyAttackDamage;
        speedRecovery = enemySpeed;
        startPosition = transform.position;
    }
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        blockCooldown += Time.deltaTime;
        jumpCooldown += Time.deltaTime;
        magicCooldown += Time.deltaTime;
        physicCooldown += Time.deltaTime;
        stunCooldown += Time.deltaTime;
        alarmFollowTimer += Time.deltaTime;
        alarmPatrolTimer += Time.deltaTime;
        vulnerableAttackTimer += Time.deltaTime;
        flipEnemyTimer += Time.deltaTime;
        enemyTakenDamageTimer += Time.deltaTime;
        colliderONTimer += Time.deltaTime;

        if (stunCooldown > stunDuration) stuned = false;//exit from stun
        if (jumpCooldown > 1.2f) jump = false;
        if (colliderONTimer > 1f) capsuleCollider.enabled = true;
        if (enemyTakenDamageTimer > 2) enemyTakeDamage = false; 
        if (currentHP > 0) EnemyBehavior();

        playerGodMode = Hero.Instance.godMode;
    }
    public enum States //Defining what states there are, named as in Unity Animator
    {
        idle,
        run
    }
    public void AnimState()//Method for determining the animation's stats
    {
        if (movement == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
            runSound.GetComponent<SoundOfObject>().ContinueSound();
        }
        if (movement == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0) this.gameObject.GetComponent<Animator>().SetInteger("State", 0);
            runSound.GetComponent<SoundOfObject>().StopSound();
        }
    }

    //Method to describe different behaviour for different enemies. The choice of behaviour depends on the object tag
    public void EnemyBehavior()
    {
        AnimState();
        EnemyMovement();
        MeleeAttack();

        if (tag == "Skeleton")
        {
            SkeletonAttack();
        }
        if (tag == "Mushroom")
        {
            MushroomAttack();
        }
        if (tag == "FlyingEye")
        {
            FlyingEyeAttack();
        }
        if (tag == "Goblin")
        {
            GoblinAttack();
        }
        if (tag == "EvilWizard")
        {
            EvilWizardAttack();
        }
        if (tag == "Martial")
        {
            MartialAttack();
        }
        if (tag == "Slime")
        {
            SlimeAttack();
        }
        if (tag == "Death")
        {
            DeathAttack();
        }
    }

    //General methods
    public void EnemyMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis

        patrolDirectionLeft = startPosition.x - patrolDistance;
        patrolDirectionRight = startPosition.x + patrolDistance;

        sightDistanceLeft = transform.position.x - sightDistance;
        sightDistanceRight = transform.position.x + sightDistance;

        bool patrol = false;
        bool follow = false;

        Vector3 pos = transform.position; //object position
        Vector3 theScale = transform.localScale; // needed to understand the direction

        if (transform.position.y < LowFlightPoint && isFlying && !isAttack && !inAttack)
        {
            float flySpeed = 1 * flyAmplitude * Time.deltaTime; //calculating direction
            pos.y += flySpeed; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
        }

        if (patrolDirectionRight < transform.position.x) patrolFlip = 2;
        if (patrolDirectionLeft > transform.position.x) patrolFlip = 1;

        if (Mathf.Abs(directionX) > sightDistance && !isAttack && !stuned && !isAttacked && alarmPatrolTimer > alarmPause && !inAttack)
        {
            alarmFollowTimer = 0;
            if (patrolDirectionLeft != transform.position.x && patrolFlip == 1)
            {
                float patrolSpeed = 1 * enemySpeed * Time.deltaTime; //calculating direction
                pos.x += patrolSpeed; //Calculating the position along the x-axis
                transform.position = pos; //applying the position
                patrol = true;

                if (patrolSpeed < 0 && transform.localScale.x > 0 && patrol) Flip();
                else if (patrolSpeed > 0 && transform.localScale.x < 0 && patrol) Flip();
            }

            if (patrolDirectionRight != transform.position.x && patrolFlip == 2)
            {
                float patrolSpeed = -1 * enemySpeed * Time.deltaTime; //calculating direction
                pos.x += patrolSpeed; //Calculating the position along the x-axis
                transform.position = pos; //applying the position
                patrol = true;

                if (patrolSpeed < 0 && transform.localScale.x > 0 && patrol) Flip();
                else if (patrolSpeed > 0 && transform.localScale.x < 0 && patrol) Flip();
            }
        }
        if (Mathf.Abs(directionX) < sightDistance && Mathf.Abs(directionX) >= attackDistance && !isAttack && !stuned && !playerGodMode && !inAttack && alarmFollowTimer > alarmPause || isAttacked && Mathf.Abs(directionX) > attackDistance && !block && !isAttack && !stuned && !playerGodMode && !inAttack || copy && !playerGodMode && !inAttack)
        {
            alarmPatrolTimer = 0;
            transform.localScale = theScale; // needed to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * enemySpeed * Time.deltaTime; //calculating direction
            pos.x += playerFollowSpeed; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
            follow = true;

            if (playerFollowSpeed < 0 && theScale.x > 0 && follow) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            else if (playerFollowSpeed > 0 && theScale.x < 0 && follow) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        }
        if (patrol || follow)
        {
            movement = true;
        }
        else
        {
            movement = false;
            if (Mathf.Abs(directionX) > attackDistance) flipEnemyTimer = 0;
        }
    }
    private void MeleeAttack() //Basic method of attack with two or more animations
    {
        float playerHP = Hero.Instance.curentHP;
        Vector3 theScale = transform.localScale; // needed to understand the direction
        float directionOfAttack = Mathf.Sign(directionX) * enemySpeed * Time.deltaTime; //calculating direction

        if (playerHP > 0 && Mathf.Abs(directionX) <= attackDistance && !stuned && !isAttack && !playerGodMode)
        {
            if (directionOfAttack < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            if (directionOfAttack > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            vulnerableAttackTimer = 0;
            isAttack = true;
            anim.SetBool("isAttack", true);
            currentAttack += Random.Range(1, 2);
            if (currentAttack > 2) currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack + ".1");
            aState = 1;
        }
        if (playerHP > 0 && vulnerableAttackTimer > vulnerableBeforeDamage && isAttack && !stuned && aState == 1)
        {
            vulnerableAttackTimer = 0;
            anim.SetTrigger("attack" + currentAttack + ".2");
            aState = 2;
        }
        if (playerHP > 0 && vulnerableAttackTimer > vulnerableAfterDamage && isAttack && !stuned && aState == 2 || playerHP > 0 && Mathf.Abs(directionX) > attackDistance)
        {
            isAttack = false;
            anim.SetBool("isAttack", false);
        }
    }
    public void EnemyAttack()
    {
        meleeAttackArea.transform.position = firePoint.position; //With each attack we will change projectile positions and give it a firing point position to receive the component from the projectile and send it in the direction of the player
        meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(firePoint.position);
        if (!copy) meleeAttackArea.GetComponent<MeleeWeapon>().GetAttackDamageInfo(currentAttackDamage);
        if (copy) meleeAttackArea.GetComponent<MeleeWeapon>().GetAttackDamageInfo(2);
    }
    public void BlockON() // Using a shield (Skeleton)
    {
        blockCooldown = 0;
        block = true;
        anim.SetBool("Block", true);
    }
    public void BlockOFF() // Using a shield (Skeleton)
    {
        block = false;
        anim.SetBool("Block", false);
    }
    public void Flip() //This is where we create the Flip method which, when called, reverses the direction of the sprite
    {
        bool playerRoll = Hero.Instance.m_rolling;
        if (!playerRoll && flipEnemyTimer > FlipPauseDefault) //|| playerRoll && flipEnemyTimer > FlipPauseRoll
        {
            flipEnemyTimer = 0;
            e_facingDirection *= -1;
            Vector3 theScale = transform.localScale; //receive the scale of the object
            theScale.x *= -1;//this flips the image e.g. 140 changes to -140, thus completely changing the direction of the sprite (the image is mirrored)
            transform.localScale = theScale; // scale conversion relative to the parent GameObjects object
        }
    }
    public void GetNameOfObject(GameObject gameObjectName) //Get a link to the game object, for summonses, so they can contact the master who summoned them
    {
        masterEnemy = gameObjectName;
    }
    public void CopyCounter()
    {
        countOfCopy -= 1;
    }
    public void ColliderOFF()
    {
        colliderONTimer = 0;
        jumpCooldown = 0;
        capsuleCollider.enabled = false;
    }
    public void Fly()
    {
        Vector3 fly = transform.position;
        if (transform.position.y < LowFlightPoint && isFlying)
        {
            float flySpeed = 1 * flyAmplitude * Time.deltaTime; //calculating direction
            fly.y += flySpeed; //Calculating the position along the x-axis
            transform.position = fly; //applying the position
        }
    }
    public void Stun()
    {
        stunCooldown = 0;
        stuned = true;
    }
    public void MeleeWeaponOff() // deactivate the weapon object
    {
        meleeAttackArea.GetComponent<MeleeWeapon>().WeaponOff();
    }
    public void JumpToPlayer() //jump to player (Goblin)
    {
        if (level >= 1) //the ability is activated at level 2
        {
            jumpCooldown = 0;
            if (directionX > 0) rb.AddForce(new Vector2(jumpForce, 2.5f), ForceMode2D.Impulse);
            if (directionX < 0) rb.AddForce(new Vector2((-1 * jumpForce), 2.5f), ForceMode2D.Impulse);
        }
    }
    public void JumpFromPlayer() // rebound from player (Goblin)
    {
        if (level >= 1) //the ability is activated at level 2
        {
            jumpCooldown = 0;
            if (directionX > 0) rb.AddForce(new Vector2((-1 * jumpForce), 2.5f), ForceMode2D.Impulse);
            if (directionX < 0) rb.AddForce(new Vector2(jumpForce, 2.5f), ForceMode2D.Impulse);
        }
    }
    public void PushFromPlayer() // rebound from a player
    {
        if (Mathf.Abs(directionX) < 1f)
        {
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (theScale.x > 0)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(-5, 1.5f), ForceMode2D.Impulse);
            }
            if (theScale.x < 0)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(5, 1.5f), ForceMode2D.Impulse);
            }
        }
    }
    public void Roll()
    {
        jumpCooldown = 0;
        if (directionX > 0)
        {
            anim.SetTrigger("roll");
            rb.AddForce(new Vector2(rollForce, 2.5f), ForceMode2D.Impulse);
            Debug.Log("Roll");
        }
        if (directionX < 0)
        {
            anim.SetTrigger("roll");
            rb.AddForce(new Vector2((-1 * rollForce), 2.5f), ForceMode2D.Impulse);
            Debug.Log("Roll");
        }
    }
    public void Push() //Method for repelling the body
    {
        if (transform.lossyScale.x < 0)
        {
            rb.velocity = Vector2.zero;
            this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(-0.5f, rb.velocity.y), ForceMode2D.Impulse);
        }
        else
        {
            rb.velocity = Vector2.zero;
            this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0.5f, rb.velocity.y), ForceMode2D.Impulse);
        }
    }
    public void LifeSteal() // Lifesteal from player
    {
        Hero.Instance.GetDamage(enemyAttackDamage);// here we access the player's script and activate the GetDamage function from there
        float heal = enemyAttackDamage * 0.5f; //The skeleton steals half of the damage the skeleton does to the player's xp
        currentHP += heal;
        float healBar = heal / (float)enemyMaxHP; // how much to increase the progress bar
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//refresh progress bar
    }
    //Attack Method of the enemies
    private void SkeletonAttack()
    {
        if ((Mathf.Abs(directionX)) < sightDistance && !stuned && !playerGodMode && !isAttack) BlockON();
        if (isAttack) BlockOFF();     
    }
    private void MushroomAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2 && !stuned && !playerGodMode) JumpToPlayer();
        if ((Mathf.Abs(directionX)) < 0.8f && magicCooldown > 10 && !stuned && !playerGodMode) MushroomSpores();
    }
    private void FlyingEyeAttack()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis
        float playerHP = Hero.Instance.curentHP;
        if ((Mathf.Abs(directionX)) < sightDistance && (Mathf.Abs(directionX)) > attackDistance && jumpCooldown > 3 && Mathf.Abs(directionY) < 3 && !stuned && !playerGodMode) JumpToPlayer();
        if (playerHP > 0 && Mathf.Abs(directionX) <= attackDistance && !stuned && !isAttack && !playerGodMode)
        {
            meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(this.gameObject.transform.position);
            if (!copy) meleeAttackArea.GetComponent<MeleeWeapon>().GetAttackDamageInfo(currentAttackDamage);
        }
    }
    private void GoblinAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        bool playerIsBlock = Hero.Instance.block;
        if ((Mathf.Abs(directionX)) < sightDistance && (Mathf.Abs(directionX)) > attackDistance && jumpCooldown > 3 && Mathf.Abs(directionY) < 3 && !stuned && !playerGodMode && !playerIsBlock) JumpToPlayer();
        if ((Mathf.Abs(directionX)) < attackDistance && jumpCooldown > 4 && Mathf.Abs(directionY) < 3 && !isAttack && !playerGodMode && playerIsBlock) JumpFromPlayer();
        if (enemyTakeDamage && jumpCooldown > 3) Roll();
    }
    private void TossingBomb() //Бросок бомбы
    {
        if ((Mathf.Abs(directionX)) < 4.5 && magicCooldown > 3 && !jump && remainingAmmo >= 1 && !stuned && !playerGodMode || isAttacked == true && magicCooldown > 3 && !jump && remainingAmmo >= 1 && !stuned && !playerGodMode)
        {
            Vector3 theScale = transform.localScale; // needed to understand the direction
            transform.localScale = theScale; // needed to understand the direction
            if (directionX < 0) // if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            {
                if (theScale.x > 0) Flip();
                GoblinBomb();
            }
            else if (directionX > 0) // if movement is greater than zero and flipRight = true, then the Flip method must be called (sprite rotation)
            {
                if (theScale.x < 0) Flip();
                GoblinBomb();
            }
        }
    }
    private void EvilWizardAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (playerHP > 0 && Mathf.Abs(directionX) < 6f && (Mathf.Abs(directionX)) > 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2 && !stuned && level >= 1 && !playerGodMode)
        {
            anim.SetTrigger("attack1");
            magicSound.GetComponent<SoundOfObject>().ContinueSound();
            timeSinceAttack = 0.0f;
            Vector3 theScale = transform.localScale; // needed to understand the direction
            transform.localScale = theScale; // needed to understand the direction
            if (directionX < 0) // if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            {
                if (theScale.x > 0) Flip();
                MagicAttack();
            }
            else if (directionX > 0) // if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            {
                if (theScale.x < 0) Flip();
                MagicAttack();
            }
        }
        else isAttack = false;
        if (playerHP > 0 && (Mathf.Abs(directionX)) < 2f && Mathf.Abs(directionY) < 2 && !stuned && !playerGodMode)
        {
            anim.SetTrigger("attack2");
            //attackSound.GetComponent<SoundOfObject>().StopSound();
            attackSound.GetComponent<SoundOfObject>().ContinueSound();
            timeSinceAttack = 0.0f;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x;
            float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y;
            if ((Mathf.Abs(directionX) < 2f && Mathf.Abs(directionY) < 2f) && magicCooldown > 0.5 && playerHP > 0)
            {
                if (directionX < 0 && theScale.x > 0) Flip();
                else if (directionX > 0 && theScale.x < 0) Flip();
                timeSinceAttack = 0.0f;
                magicCooldown = 0;
                float fireDMG = 150f * enemyAttackDamage * Time.deltaTime;
                Hero.Instance.GetDamage(fireDMG);
            }
        }
    }
    private void MartialAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (playerHP > 0 && Mathf.Abs(directionX) < 2.5f && Mathf.Abs(directionY) < 1.5f && timeSinceAttack > 1 && !stuned && !playerGodMode)
        {
            MeleeAttack();
        }
        else isAttack = false;
    }
    private void SlimeAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2 && !stuned && !playerGodMode) JumpToPlayer();
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1 && !stuned && !playerGodMode)
        {
            anim.SetTrigger("spin");
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
    }
    private void DeathAttack()
    {
        float playerHP = Hero.Instance.curentHP;

        if (playerHP > 0 && Mathf.Abs(directionX) < 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2 && !stuned && !playerGodMode)
        {
            anim.SetTrigger("attack1");
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
        if ((Mathf.Abs(directionX)) < 8f && (Mathf.Abs(directionX)) > 2 && Mathf.Abs(directionY) < 2f && !stuned && !playerGodMode || isAttacked == true && !stuned && !playerGodMode)
        {
            SpellDrainHP();
            DeathSummonMinioins();
        }
    }


    //Special ability of the enemies
    public void MushroomSpores() //creates a cloud of spore that damasks the player (Mushroom)
    {
        if (level > 0 && !stuned)
        {
            magicCooldown = 0; // reset timer
            Vector3 MoushroomScale = transform.localScale; //take the mushroom sprite rotation parameter
            transform.localScale = MoushroomScale; //take the mushroom sprite rotation parameter
            Vector3 sporeSpawnPosition = this.gameObject.transform.position; //taking a mushroom position
            GameObject newSpore = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(sporeSpawnPosition.x, sporeSpawnPosition.y, sporeSpawnPosition.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            newSpore.name = "spore" + Random.Range(1, 999);
            if (MoushroomScale.x < 0) sporeSpawnPosition.x -= 0.8f; // move the collection forward of the mushroom depending on the rotation of the sprite
            if (MoushroomScale.x > 0) sporeSpawnPosition.x += 0.8f; // move the collection forward of the mushroom depending on the rotation of the sprite
            newSpore.GetComponent<Spore>().sporeDirection(sporeSpawnPosition); //transmit coordinates for spore cloud spawning
        }
    }
    public void SummonCopy() //creates copies of the Flying Eye
    {
        if (level > 0 && countOfCopy < 1)
        {
            magicCooldown = 0;
            Vector3 pos = transform.position;
            GameObject guard1 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1.5f, pos.y, pos.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            guard1.name = "Enemy" + Random.Range(1, 999);
            guard1.GetComponent<Enemy_Behavior>().GetNameOfObject(this.gameObject);
            GameObject guard2 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1f, pos.y, pos.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            guard2.name = "Enemy" + Random.Range(1, 999);
            guard2.GetComponent<Enemy_Behavior>().GetNameOfObject(this.gameObject);
            countOfCopy = 2;
        }
        else return;

    }

    public void GoblinBomb() // Bomb Throw (Goblin)
    {
        if (level >= 0 && remainingAmmo >= 1)
        {
            remainingAmmo -= 1;
            magicCooldown = 0; // reset timer
            Vector3 goblinScale = transform.localScale; //take the goblin sprite rotation parameter
            transform.localScale = goblinScale; //take the goblin sprite rotation parameter
            Vector3 bombSpawnPosition = this.gameObject.transform.position; //taking the goblin position
            GameObject bombBall = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(bombSpawnPosition.x, bombSpawnPosition.y, bombSpawnPosition.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            bombBall.name = "Bomb" + Random.Range(1, 999);
            if (goblinScale.x < 0) bombSpawnPosition.x -= 1f; // move bomb forward goblin depending on sprite rotation
            if (goblinScale.x > 0) bombSpawnPosition.x += 1f;
            bombBall.GetComponent<Bomb>().GetEnemyName(this.gameObject.name);
            bombBall.GetComponent<Bomb>().bombDirection(bombSpawnPosition);
        }
        if (level < 5) remainingAmmo = 0;
    }
    public void MagicAttack() // EvilWizard FireBall
    {
        Vector3 shootingDirection = new Vector3(1, 0, 109);
        Vector3 pos = this.gameObject.transform.position;
        Debug.Log(pos);
        if (transform.localScale.x > 0)
        {
            shootingDirection = new Vector3(1, 0, 109);
            pos.x += 1;
        }
        if (transform.localScale.x < 0)
        {
            shootingDirection = new Vector3(-1, 0, 109);
            pos.x -= 1;
        }
        GameObject fireBall = Instantiate(ammo[0], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
        fireBall.name = "fireball" + Random.Range(1, 999);

        fireBall.GetComponent<FireBall>().SetDirection(shootingDirection);
    }
    public void DeathSummonMinioins() // Slimes call (Boss Death)
    {
        if (physicCooldown >= 8)
        {
            physicCooldown = 0; // reset timer
            anim.SetTrigger("cast1");
            Vector3 spellSpawnPosition = this.gameObject.transform.position; // taking position
            spellSpawnPosition.x -= 2f;
            SummonSlime.Instance.SummonDirection(spellSpawnPosition); //transmit coordinates for magic spawning
        }
    }
    public void SpellDrainHP() //using magic to steal lives (Boss Death)
    {
        if (magicCooldown >= 3)
        {
            magicCooldown = 0; // reset timer
            anim.SetTrigger("cast1");
            Vector3 spellSpawnPosition = player.transform.position; // taking the Player's position
            spellSpawnPosition.y += 1.7f; // need magic to spawn just above the player
            DrainHP.Instance.DrainHPDirection(spellSpawnPosition); //transmit coordinates for magic spawning
        }
    }
    public void BossDeathHeal(float heal)
    {
        currentHP += heal;
        float healBar = heal / enemyMaxHP; // how much to increase the progress bar
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//refresh progress bar
    }
    public void BossDeathDamage(float dmg)
    {
        currentHP -= dmg;
        isAttacked = true;
        takedDamage = dmg / enemyMaxHP; //how much you need to reduce the progress bar
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//refresh progress bar
    }
    //Take damage and Die methods
    public void TakeDamage(float dmg) //Damage (in dmg a value is specified, in the Hero script when the TakeDamage method is called, a variable of weapon damage is written to dmg ) 
    {
        float maxHP = enemyMaxHP;
        isBlock = this.gameObject.GetComponent<Enemy_Behavior>().block;
        enemyTakenDamageTimer = 0;  
        enemyTakeDamage = true;
        if (currentHP > 0 && !isBlock)
        {
            if (isBlooded)
            {
                GameObject bloodSpawn = Instantiate(blood[Random.Range(0, blood.Length)], new Vector3(this.gameObject.transform.position.x - 0.3f, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity); //Cloning an object
                bloodSpawn.gameObject.SetActive(true);
            }

            currentHP -= dmg;
            isAttacked = true;
            takedDamage = (float)dmg / maxHP; //how much you need to reduce the progress bar
            anim.SetTrigger("damage");// animation of getting a demage
            Enemy_Behavior.Instance.TakeDamageSound();
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//refresh progress bar
        }
        if (currentHP > 0 && isBlock)
        {
            int level = LvLGeneration.Instance.Level;
            if (level <= 4) blockDMG = dmg * 0.5f;//if the Player is below level 5 then 50% damage blocking
            if (level >= 5) blockDMG = dmg * 0.1f;//if the Player is higher than level 4 then 90% damage blocking
            currentHP -= blockDMG;
            Debug.Log(blockDMG);
            Enemy_Behavior.Instance.ShieldDamageSound();
            isAttacked = true;
            takedDamage = blockDMG / maxHP; //how much you need to reduce the progress bar
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//refresh progress bar
        }
        if (currentHP <= 0)
        {
            int reward = enemyReward;
            LvLGeneration.Instance.PlusCoin(reward);//call for a method to increase points
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            capsuleCollider.enabled = false;
            anim.StopPlayback();
            anim.SetBool("dead", true);
            anim.SetTrigger("m_death");//death animation
            enemyDead = true;
        }
    }
    public virtual void Die() //Method removes this game object, called by the animator immediately after the death animation ends
    {
        bool copy = this.gameObject.GetComponent<Enemy_Behavior>().copy;
        Destroy(this.gameObject);//destroy this game object
        if (!copy) LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
        if (copy && masterEnemy != null) masterEnemy.GetComponent<Enemy_Behavior>().CopyCounter();// copy destroying decreases the copy count, allowing you to call an extra copy.
        if (tag == "Slime")
        {
            GameObject[] deathObjects = GameObject.FindGameObjectsWithTag("Death");
            foreach (GameObject obj in deathObjects)
            {
                if (obj.name != "BossDeath")
                {
                    obj.GetComponent<Enemy_Behavior>().BossDeathDamage(50);
                }
            }
        }
        if (tag == "Death") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
    }

    //Sound Death and damage sounds are tied to Animation (for now), damage and jump sounds are tied to the code in the methods above

    public void DieSound()
    {
        dieSound.GetComponent<SoundOfObject>().ContinueSound();
    }
    public void TakeDamageSound()
    {
        takeDamageSound.GetComponent<SoundOfObject>().PlaySound();
    }
    public void ShieldDamageSound()
    {
        shieldHitSound.GetComponent<SoundOfObject>().PlaySound();
    }
    public void AttackSound()
    {
        attackSound.GetComponent<SoundOfObject>().StopSound();
        attackSound.GetComponent<SoundOfObject>().PlaySound();
    }
}