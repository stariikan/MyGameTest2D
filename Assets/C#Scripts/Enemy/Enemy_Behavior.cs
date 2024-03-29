using System.Collections;
using System.Linq.Expressions;
using System.Net;
using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
{
    //Enemy parameters
    public float enemyMaxHP = 48; // Maximum skeleton lives
    public float enemyAttackDamage = 10; // Damage from physical attack
    public float enemySpeed = 2f;//Enemy speed
    public float followSpeed = 2f;//Enemy speed
    public float rollForce = 7.5f;
    private float blockDMG;
    public int countOfSummon; // initially 0, when the call occurs become 3, as copies die 
    public int remainingAmmo = 3; // Bombs in stock
    public int enemyReward = 2;// reward for defeating the enemy

    //Enemy movement and attack configuration
    public float patrolDistance; // How far the object walks in standby mode. For example, starts from the leftmost point, walks 10 meters to the right, returns to the starting position. Then starts over.
    public float sightDistance; // How far away the enemy notices the player and triggers movement to the player.
    public float alarmPause; // The pause between the moment when the skeleton loses sight of the player and returns to standby mode.
    public float attackDistance; //Range of physical attack
    public float flyAmplitude; // Body Lift Force
    public float LowFlightPoint; // the height at which the body will be lifted
    public float stunDuration; // Stun duration. How many seconds the object will be stunned.
    public float vulnerableBeforeDamage; // The time between the swing and the infliction of damage. How many seconds enemy will be vulnerable before attacking.
    public float vulnerableAfterDamage; // How many seconds the enemy will be vulnerable after taking damage.
    public float jumpForce;
    public float flipPauseDefault; //пауза перед тем как враг развернётся на 180°.
    //public float FlipPauseRoll;  //пауза перед тем как враг развернётся на 180° когда игрок перекатился врагу за спину.

    //Enemy states
    public bool movement = false; //mob is not chasing the player
    public bool inAttackState = false; //If an object prepare to attack (need for special attack move, this bool disable EnemyMovement method if true)
    public bool isAttack = false; //If an object (enemy) is attacking
    public bool isStun = false; //state of stun
    public bool jump = false;
    public bool rolling = false;
    public bool isBlock; //check whether the block is set


    public bool isFlying; //Flying enemy or not
    public bool isBlooded; //Does the enemy have blood
    public bool grounded; 
    public bool enemyDead = false; // Is the object dead
    public bool isAttacked = false; //If the object has sustained damage
    public bool enemyTakeDamage = false; //If enemy take damage
    public bool block = false; //state of block
    public bool copy = false; // is this object a copy or not?

    //Enemy cooldowns
    private float jumpCooldown = Mathf.Infinity; //cooldown on rebound and jump
    private float rollCooldown = Mathf.Infinity; //cooldown on rebound and jump
    private float physicCooldown = Mathf.Infinity; //cooldown on physical attack
    private float magicCooldown = Mathf.Infinity; //cooldown on mage attack
    private float stunCooldown; //stun recovery
    private float specialAttackCooldown = Mathf.Infinity; //сooldown special ability 
    public float alarmFollowTimer = Mathf.Infinity; //How much time has passed since the loss of the player to the object
    public float alarmPatrolTimer = Mathf.Infinity; //How much time has passed since the loss of the player to the object
    private float vulnerableAttackTimer; //timer for switching from one attack state to another attack state
    private float colliderONTimer = Mathf.Infinity;
    private float enemyTakenDamageTimer = Mathf.Infinity;
    public float flipEnemyTimer = Mathf.Infinity; //Time to turn Enemy

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

    public int patrolFlip = 1;
    public Vector3 startPosition; //start position
    public float patrolDirectionLeft;
    public float patrolDirectionRight;
    public float sightDistanceLeft;
    public float sightDistanceRight;
    public bool playerGodMode;

    public Rigidbody2D rb; //Physical body
    private CapsuleCollider2D capsuleCollider;
    private Animator anim; //Variable by which the object is animated
    private GameObject masterEnemy; //this will link to the Master enemy who calls summons
    public GameObject[] ammo; // Enemy attack shells
    public GameObject[] blood; //blood
    public GameObject player; //For identifying the player on the scene
    public GameObject meleeAttackArea; // Physical Weapons
    public Transform firePointRight; //The position from which the shells will be fired

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
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        //directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis
        if (Mathf.Abs(directionX) < 10f)
        {

            timeSinceAttack += Time.deltaTime;
            jumpCooldown += Time.deltaTime;
            rollCooldown += Time.deltaTime;
            magicCooldown += Time.deltaTime;
            physicCooldown += Time.deltaTime;
            stunCooldown += Time.deltaTime;
            alarmFollowTimer += Time.deltaTime;
            alarmPatrolTimer += Time.deltaTime;
            vulnerableAttackTimer += Time.deltaTime;
            flipEnemyTimer += Time.deltaTime;
            enemyTakenDamageTimer += Time.deltaTime;
            colliderONTimer += Time.deltaTime;
            specialAttackCooldown += Time.deltaTime;

            if (stunCooldown > stunDuration) isStun = false;//exit from stun
            if (jumpCooldown > 1.2f) jump = false;
            if (rollCooldown > 2f) rolling = false;
            if (colliderONTimer > 1f) capsuleCollider.enabled = true;
            if (enemyTakenDamageTimer > 2) enemyTakeDamage = false;
            playerGodMode = Hero.Instance.godMode;
        }
        if (currentHP > 0 && (Mathf.Abs(directionX) < 10f))
        {
            AnimState();
            EnemyMovement();
            if (tag != "EvilWizard" && tag != "Slime") MeleeAttack();
            if (tag == "Skeleton") SkeletonAttack();
            if (tag == "EvilWizard") EvilWizardAttack();
            if (tag == "Mushroom") MushroomAttack();
            if (tag == "Slime") SlimeAttack();
            if (tag == "Martial") MartialAttack();
            if (tag == "Goblin") GoblinAttack();
            if (tag == "FlyingEye") FlyingEyeAttack();
            if (tag == "Death") DeathAttack();
        }
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
    //General methods
    public void EnemyMovement()
    {
        patrolDirectionLeft = startPosition.x - patrolDistance;
        patrolDirectionRight = startPosition.x + patrolDistance;

        sightDistanceLeft = transform.position.x - sightDistance;
        sightDistanceRight = transform.position.x + sightDistance;

        bool patrol = false;
        bool follow = false;

        Vector3 pos = transform.position; //object position
        Vector3 theScale = transform.localScale; // needed to understand the direction

        if (isFlying && !isAttack && !inAttackState)
        {
            Jump();
        }
        if (patrolDirectionRight < transform.position.x) patrolFlip = 2;
        if (patrolDirectionLeft > transform.position.x) patrolFlip = 1;

        if (Mathf.Abs(directionX) > sightDistance && !isAttack && !isStun && !isAttacked && alarmPatrolTimer > alarmPause && !inAttackState)
        {
            alarmFollowTimer = 0;
            if (patrolDirectionLeft != transform.position.x && patrolFlip == 1)
            {
                float patrolSpeed = 1 * enemySpeed * Time.deltaTime; //calculating direction
                pos.x += patrolSpeed; //Calculating the position along the x-axis
                if (patrolSpeed > 0 && transform.localScale.x > 0)
                {
                    transform.position = pos; //applying the position
                    patrol = true;
                }

                if (patrolSpeed < 0 && transform.localScale.x > 0) Flip();
                else if (patrolSpeed > 0 && transform.localScale.x < 0) Flip();
            }

            if (patrolDirectionRight != transform.position.x && patrolFlip == 2)
            {
                float patrolSpeed = -1 * enemySpeed * Time.deltaTime; //calculating direction
                pos.x += patrolSpeed; //Calculating the position along the x-axis
                if (patrolSpeed < 0 && transform.localScale.x < 0)
                {
                    transform.position = pos; //applying the position
                    patrol = true;
                }


                if (patrolSpeed < 0 && transform.localScale.x > 0) Flip();
                else if (patrolSpeed > 0 && transform.localScale.x < 0) Flip();
            }
        }
        if (Mathf.Abs(directionX) < sightDistance && Mathf.Abs(directionX) >= attackDistance && !isAttack && !isStun && !playerGodMode && !inAttackState && alarmFollowTimer > alarmPause || isAttacked && Mathf.Abs(directionX) > attackDistance && !block && !isAttack && !isStun && !playerGodMode && !inAttackState || copy && !playerGodMode && !inAttackState)
        {
            alarmPatrolTimer = 0;
            transform.localScale = theScale; // needed to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * enemySpeed * Time.deltaTime; //calculating direction

            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)

            pos.x += playerFollowSpeed; //Calculating the position along the x-axis

            if (playerFollowSpeed < 0 && theScale.x < 0)
            {
                transform.position = pos; //applying the position
                follow = true;
            }
            if (playerFollowSpeed > 0 && theScale.x > 0)
            {
                transform.position = pos; //applying the position
                follow = true;
            }
        }
        if (patrol || follow)
        {
            movement = true;
        }
        else
        {
            movement = false;
        }
    }
    public void MeleeAttack() //Basic method of attack with two or more animations
    {
        float playerHP = Hero.Instance.curentHP;
        Vector3 theScale = transform.localScale; // needed to understand the direction
        float directionOfAttack = Mathf.Sign(directionX) * enemySpeed * Time.deltaTime; //calculating direction
        if (directionOfAttack < 0 && theScale.x > 0 && isAttack == true) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        else if (directionOfAttack > 0 && theScale.x < 0 && isAttack == true) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        if (playerHP > 0 && Mathf.Abs(directionX) <= attackDistance && !isStun && !isAttack && !playerGodMode && !inAttackState)
        {

            vulnerableAttackTimer = 0;
            isAttack = true;
            anim.SetBool("isAttack", true);
            currentAttack += Random.Range(1, 2);
            if (currentAttack > 2) currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack + ".1");
            aState = 1;
        }
        if (playerHP > 0 && vulnerableAttackTimer > vulnerableBeforeDamage && isAttack && !isStun && aState == 1)
        {
            vulnerableAttackTimer = 0;
            anim.SetTrigger("attack" + currentAttack + ".2");
            aState = 2;
        }
        if (playerHP > 0 && vulnerableAttackTimer > vulnerableAfterDamage && isAttack && !isStun && aState == 2)
        {
            anim.SetBool("isAttack", false);
            isAttack = false;
            flipEnemyTimer = 0;
        }
    }
    private void PlayerFollow(Vector3 targetPos)
    {
        float directionOfAttack = Mathf.Sign(directionX) * enemySpeed * Time.deltaTime; //calculating direction
        Vector3 theScale = transform.localScale; // needed to understand the direction
        if (directionOfAttack < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        if (directionOfAttack > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        Vector3 pos = transform.position; //object position
        float endPoint = transform.position.x - targetPos.x;
        float playerFollowSpeed = (Mathf.Sign(endPoint) * -1) * followSpeed * Time.deltaTime; //calculating direction
        pos.x += playerFollowSpeed; //Calculating the position along the x-axis
        transform.position = pos; //applying the position
        movement = true;
    }
    public void EnemyAttack()
    {
        meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(firePointRight.position);
        if (!copy) meleeAttackArea.GetComponent<MeleeWeapon>().GetAttackDamageInfo(currentAttackDamage);
        if (copy) meleeAttackArea.GetComponent<MeleeWeapon>().GetAttackDamageInfo(2);
    }
    public void BlockON() // Using a shield (Skeleton)
    {
        if (physicCooldown > 0.8f)
        {
            block = true;
            anim.SetBool("Block", true);
        }
    }
    public void BlockOFF() // Using a shield (Skeleton)
    {
        physicCooldown = 0;
        block = false;
        anim.SetBool("Block", false);
    }
    public void Flip() //This is where we create the Flip method which, when called, reverses the direction of the sprite
    {
        bool playerRoll = Hero.Instance.m_rolling;
        if (!playerRoll && flipEnemyTimer > flipPauseDefault) //|| playerRoll && flipEnemyTimer > FlipPauseRoll
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
        countOfSummon -= 1;
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
        isStun = true;
    }
    public void MeleeWeaponOff() // deactivate the weapon object
    {
        meleeAttackArea.GetComponent<MeleeWeapon>().WeaponOff();
    }
    public void Jump()
    {
        if (grounded == true) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
    }
    public void RollForward()
    {
        if(rolling == false && inAttackState == true)
        {
            rollCooldown = 0;
            rolling = true;
            if (e_facingDirection == 1) rb.velocity = new Vector2(rollForce, rb.velocity.y);
            if (e_facingDirection == -1) rb.velocity = new Vector2((rollForce * -1), rb.velocity.y);
        }
    }
    public void RollBackward()
    {
        if (rolling == false && inAttackState == true)
        {
            rollCooldown = 0;
            rolling = true;
            anim.SetTrigger("roll");
            if (e_facingDirection == -1) rb.velocity = new Vector2(rollForce, rb.velocity.y);
            if (e_facingDirection == 1) rb.velocity = new Vector2((rollForce * -1), rb.velocity.y);
        }
    }
    public void JumpForward()
    {
        Vector3 targetPos = player.transform.position;
        float endPoint = transform.position.x - targetPos.x;
        float playerHP = Hero.Instance.curentHP;
        Vector3 theScale = transform.localScale; // needed to understand the direction
        float directionOfAttack = Mathf.Sign(directionX) * enemySpeed * Time.deltaTime; //calculating direction
        if (directionOfAttack < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        if (directionOfAttack > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        if (rolling == false && inAttackState == true && playerHP > 0)
        {
            rollCooldown = 0;
            rolling = true;
            if (e_facingDirection == 1) rb.velocity = new Vector2(rollForce, rb.velocity.y + 2);
            if (e_facingDirection == -1) rb.velocity = new Vector2((rollForce * -1), rb.velocity.y + 2);
            if ((Mathf.Abs(endPoint)) < 1f) player.GetComponent<Hero>().PlayerStun();
        }
    }
    public void ArcAttack() //Дуговая атака
    {
        if (specialAttackCooldown > 3) 
        {
            Vector3 pos = transform.position;
            Vector3 targetPos = player.transform.position;
            if (tag == "FlyingEye") targetPos = Hero.Instance.bodyBackPoint.transform.position;
            float playerHP = Hero.Instance.curentHP;
            float endPoint = transform.position.x - targetPos.x;

            inAttackState = true;
            
            if (transform.position.y < LowFlightPoint)
            {
                float flySpeed = 1 * (flyAmplitude * 1.4f) * Time.deltaTime; //calculating direction
                pos.y += flySpeed; //Calculating the position along the x-axis
            }

            float arcAttackSpeed = 10.0f;
            float directionArcAttack = targetPos.x - this.gameObject.transform.position.x;
            float arcAttackMove = Mathf.Sign(directionArcAttack) * arcAttackSpeed * Time.deltaTime;
            pos.x += arcAttackMove;
            transform.position = pos;
            if (Mathf.Abs(endPoint) < 0.6f)
            {
                if (playerHP > 0 && (Mathf.Abs(directionX)) <= 0.6f && !isStun && !isAttack && !playerGodMode && tag == "FlyingEye") Hero.Instance.GetDamage(enemyAttackDamage);
                specialAttackCooldown = 0;
                inAttackState = false;
            }
        }
    }
    public void ArcDodge() //Дуговая атака
    {
        if (specialAttackCooldown > 3)
        {
            Vector3 pos = transform.position;
            Vector3 targetPos = player.transform.position;
            float playerHP = Hero.Instance.curentHP;
            float endPoint = transform.position.x - targetPos.x;
            inAttackState = true;
            if (transform.position.y < LowFlightPoint)
            {
                float flySpeed = 1 * (flyAmplitude * 1.4f) * Time.deltaTime; //calculating direction
                pos.y += flySpeed; //Calculating the position along the x-axis
            }
            float arcAttackSpeed = 10.0f;
            float directionArcAttack = targetPos.x - this.gameObject.transform.position.x;
            float arcAttackMove = (Mathf.Sign(directionArcAttack) * -1) * arcAttackSpeed * Time.deltaTime;
            pos.x += arcAttackMove;
            transform.position = pos;
            if (Mathf.Abs(endPoint) > 4f)
            {
               specialAttackCooldown = 0;
               inAttackState = false;
            }
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
        if (currentHP < enemyMaxHP)
        {
            Hero.Instance.GetDamage(enemyAttackDamage);// here we access the player's script and activate the GetDamage function from there
            float heal = enemyAttackDamage * 0.5f; //The skeleton steals half of the damage the skeleton does to the player's xp
            currentHP += heal;
            float healBar = heal / (float)enemyMaxHP; // how much to increase the progress bar
            if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//refresh progress bar
        }
    }
    //Attack Method of the enemies
    public void SkeletonAttack()
    {
        if ((Mathf.Abs(directionX)) < sightDistance && !isStun && !playerGodMode && !isAttack) BlockON();
        if ((Mathf.Abs(directionX)) > sightDistance && !isStun && !playerGodMode && !isAttack) BlockOFF();
        if (isAttack) BlockOFF();     
    }
    public void MushroomAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        bool playerIsBlock = Hero.Instance.block;
        bool playerIsJumpPC = Hero.Instance.isJumpPC;
        bool playerIsJumpMobile = Hero.Instance.isJumpMobile;
        if ((Mathf.Abs(directionX)) < sightDistance && jumpCooldown > 3 && !isStun && !playerGodMode && !playerIsBlock && !enemyTakeDamage && currentHP > 0)
        {
            if ((Mathf.Abs(directionX)) < sightDistance && !playerGodMode && !isAttack && !playerIsBlock)
            {
                Vector3 targetPos = Hero.Instance.bodyFront.transform.position;
                float endPoint = transform.position.x - targetPos.x;
                inAttackState = true;
                if ((Mathf.Abs(endPoint)) < 0.2f)
                {
                    inAttackState = false;
                }
                else
                {
                    inAttackState = true;
                    JumpForward();
                    PlayerFollow(targetPos);
                }
            }
            else
            {
                inAttackState = false;
                rb.velocity = Vector3.zero;
            }
        }
    }
    public void FlyingEyeAttack()
    {
        if ((Mathf.Abs(directionX)) < sightDistance && !isStun && !playerGodMode && !isAttack && currentHP > 0)
        {
            float playerHP = Hero.Instance.curentHP;
            Vector3 targetPos = Hero.Instance.bodyBackPoint.transform.position;
            float endPoint = transform.position.x - targetPos.x;
            inAttackState = true;
            if ((Mathf.Abs(endPoint)) < 0.2f)
            {
                inAttackState = false;
            }
            else
            {
                inAttackState = true;
                Jump();
                PlayerFollow(Hero.Instance.bodyBackPoint.transform.position);
            }
        }
        else
        {
            inAttackState = false;
        }
    }
    public void GoblinAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        bool playerIsBlock = Hero.Instance.block;
        bool playerIsJumpPC = Hero.Instance.isJumpPC;
        bool playerIsJumpMobile = Hero.Instance.isJumpMobile;
        if ((Mathf.Abs(directionX)) < sightDistance && jumpCooldown > 3 && !isStun && !playerGodMode && !playerIsBlock && !enemyTakeDamage && currentHP > 0)
        {
            if ((Mathf.Abs(directionX)) < sightDistance && !isStun && !playerGodMode && !isAttack)
            {
                Vector3 targetPos = Hero.Instance.bodyBackPoint.transform.position;
                float endPoint = transform.position.x - targetPos.x;
                inAttackState = true;
                if ((Mathf.Abs(endPoint)) < 0.2f)
                {
                    inAttackState = false;
                }
                else
                {
                    inAttackState = true;
                    RollForward();
                    PlayerFollow(targetPos);
                }
            }
            else
            {
                inAttackState = false;
                rb.velocity = Vector3.zero;
            }
        }
        if ((Mathf.Abs(directionX)) < sightDistance && jumpCooldown > 1 && !isAttack && !playerGodMode && playerIsBlock && !enemyTakeDamage && currentHP > 0)
        {
            if ((Mathf.Abs(directionX)) < sightDistance && !isStun && !playerGodMode && !isAttack)
            {
                Vector3 targetPos = player.transform.position;
                float endPoint = transform.position.x - targetPos.x;
                inAttackState = true;
                if ((Mathf.Abs(endPoint)) < 1.5f)
                {
                    inAttackState = false;
                }
                else
                {
                    inAttackState = true;
                    RollBackward();
                }
            }
            else
            {
                inAttackState = false;
                rb.velocity = Vector3.zero;
            }
        }
        if ((Mathf.Abs(directionX)) < sightDistance && jumpCooldown > 1 && !isAttack && !playerGodMode && (playerIsJumpMobile || playerIsJumpPC) && !enemyTakeDamage && currentHP > 0)
        {
            if ((Mathf.Abs(directionX)) < sightDistance && !isStun && !playerGodMode && !isAttack)
            {
                Vector3 targetPos = player.transform.position;
                float endPoint = transform.position.x - targetPos.x;
                inAttackState = true;
                if ((Mathf.Abs(endPoint)) < 1.5f)
                {
                    inAttackState = false;
                }
                else
                {
                    inAttackState = true;
                    RollBackward();
                }
            }
        }
        if (enemyTakeDamage && jumpCooldown > 1 && !isAttack && !playerGodMode && !isStun && currentHP > 0)
        {
            Vector3 targetPos = Hero.Instance.bodyBackPoint.transform.position;
            float endPoint = transform.position.x - targetPos.x;
            int directionOfRoll = Random.Range(1, 2);
            inAttackState = true;
            if (directionOfRoll == 1)
            {
                inAttackState = true;
                RollForward();
                PlayerFollow(targetPos);
                Debug.Log(directionOfRoll);
            }
            if (directionOfRoll == 2)
            {
                inAttackState = true;
                RollBackward();
            }
        }
        if ((playerIsJumpPC || playerIsJumpMobile) && (Mathf.Abs(directionX)) <= attackDistance && jumpCooldown > 1 && !isAttack && !playerGodMode && !isStun && currentHP > 0)
        {
            Vector3 targetPos = Hero.Instance.bodyBackPoint.transform.position;
            float endPoint = transform.position.x - targetPos.x;
            inAttackState = true;
            RollBackward();
        }
    }
        private void TossingBomb() //Бросок бомбы
    {
        if ((Mathf.Abs(directionX)) < 4.5 && magicCooldown > 3 && !jump && remainingAmmo >= 1 && !isStun && !playerGodMode || isAttacked == true && magicCooldown > 3 && !jump && remainingAmmo >= 1 && !isStun && !playerGodMode)
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
    public void EvilWizardAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (playerHP > 0 && Mathf.Abs(directionX) < 6f && (Mathf.Abs(directionX)) > 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2 && !isStun && level >= 1 && !playerGodMode)
        {
            anim.SetTrigger("attack1.1");
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
        if (playerHP > 0 && (Mathf.Abs(directionX)) < 2f && Mathf.Abs(directionY) < 2 && !isStun && !playerGodMode)
        {
            anim.SetTrigger("attack2.2");
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
    public void MartialAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (playerHP > 0 && Mathf.Abs(directionX) < 2.5f && Mathf.Abs(directionY) < 1.5f && timeSinceAttack > 1 && !isStun && !playerGodMode)
        {
            MeleeAttack();
        }
        else isAttack = false;
    }
    public void SlimeAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1 && !isStun && !playerGodMode)
            {
                anim.SetTrigger("spin");
                isAttack = true;
                timeSinceAttack = 0.0f;// Reset timer
        }
        else isAttack = false;
    }
    public void DeathAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if ((Mathf.Abs(directionX)) < 8f && (Mathf.Abs(directionX)) > 2 && Mathf.Abs(directionY) < 2f && !isStun && !playerGodMode || isAttacked == true && !isStun && !playerGodMode)
        {
            SpellDrainHP();
            DeathSummonMinioins();
        }
    }

    //Special ability of the enemies
    public void MushroomSpores() //creates a cloud of spore that damasks the player (Mushroom)
    {
        if (level > 0 && !isStun)
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
        if (level > 0 && countOfSummon < 1)
        {
            magicCooldown = 0;
            Vector3 pos = transform.position;
            GameObject guard1 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1.5f, pos.y, pos.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            guard1.name = "Enemy" + Random.Range(1, 999);
            guard1.GetComponent<Enemy_Behavior>().GetNameOfObject(this.gameObject);
            GameObject guard2 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1f, pos.y, pos.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            guard2.name = "Enemy" + Random.Range(1, 999);
            guard2.GetComponent<Enemy_Behavior>().GetNameOfObject(this.gameObject);
            countOfSummon = 2;
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
        if (physicCooldown >= 8 && countOfSummon < 1)
        {
            physicCooldown = 0; // reset timer
            anim.SetTrigger("cast1");
            Vector3 spellSpawnPosition = this.gameObject.transform.position; // taking position
            spellSpawnPosition.x -= 2f;
            SummonSlime.Instance.MasterOfSummon(this.gameObject);
            SummonSlime.Instance.SummonDirection(spellSpawnPosition); //transmit coordinates for magic spawning
            countOfSummon = 3;
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
                GameObject bloodSpawn = Instantiate(blood[Random.Range(0, blood.Length)], new Vector3(this.gameObject.transform.position.x - 0.1f, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity); //Cloning an object
                bloodSpawn.gameObject.SetActive(true);
            }
            currentHP -= dmg;
            isAttacked = true;
            takedDamage = (float)dmg / maxHP; //how much you need to reduce the progress bar
            if (takedDamage > 1) takedDamage = 1;
            anim.SetTrigger("damage");// animation of getting a demage
            this.gameObject.GetComponent<Enemy_Behavior>().TakeDamageSound();
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//refresh progress bar
        }
        if (currentHP > 0 && isBlock)
        {
            int level = LvLGeneration.Instance.Level;
            if (level <= 4) blockDMG = dmg * 0.5f;//if the Player is below level 5 then 50% damage blocking
            if (level >= 5) blockDMG = dmg * 0.1f;//if the Player is higher than level 4 then 90% damage blocking
            currentHP -= blockDMG;
            this.gameObject.GetComponent<Enemy_Behavior>().ShieldDamageSound();
            isAttacked = true;
            takedDamage = blockDMG / maxHP; //how much you need to reduce the progress bar
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//refresh progress bar
        }
        if (currentHP <= 0 && enemyDead == false)
        {
            rb.gravityScale = 1;
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
        LvLGeneration.Instance.PlusCoin(enemyReward);//call for a method to increase points
        bool copy = this.gameObject.GetComponent<Enemy_Behavior>().copy;
        Destroy(this.gameObject);//destroy this game object
        if (this.gameObject.tag != "Summon") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
        if (masterEnemy != null && this.gameObject.tag == "Summon")
        {
            masterEnemy.GetComponent<Enemy_Behavior>().CopyCounter();// copy destroying decreases the copy count, allowing you to call an extra copy.
            masterEnemy.GetComponent<Enemy_Behavior>().BossDeathDamage(30);
        }
        if (tag == "Death") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
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