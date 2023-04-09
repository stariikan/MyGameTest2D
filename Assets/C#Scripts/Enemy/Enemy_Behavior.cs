using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
{
    //Skeleton parameters
    public float skeletonMaxHP = 48; // Maximum skeleton lives
    public float skeletonAttackDamage = 10; // Damage from physical attack
    public int skeletonReward = 2;// reward for defeating the enemy
    private bool isBlock; //check whether the block is set
    private float blockDMG;
    public float skeletonSpeed = 2f;//Skeleton speed
    private float blockCooldown; //сooldown block

    //Mushroom parameters
    public float mushroomMaxHP = 80; //Maximum lives of the Mushroom
    public float mushroomAttackDamage = 10; // Damage from physical attack
    public int mushroomReward = 2;// reward for defeating the enemy
    public float moushroomSpeed = 2f;//Mushroom's speed

    //Flying Eye parameters
    public float flyingEyeMaxHP = 24; //Maximum Flying Eye lives
    public float flyingEyeAttackDamage = 10; // Damage from physical attack
    public int flyingEyeReward = 2;// reward for defeating the enemy
    private GameObject masterEnemy; //this will link to the eye wizard who calls on the other eyes
    public float flyingEyeSpeed = 2f;// Speed of the Eye
    private int countOfCopy; // initially 0, when the call occurs become 3, as copies die 

    //Goblin Parameters
    public float goblinMaxHP = 32;  //Maximum Goblin Lives
    public float goblinAttackDamage = 15; // Damage from physical attack
    public int goblinReward = 2;// reward for defeating the enemy
    public float goblinSpeed = 3f;//Goblin Speed
    public int remainingBombs = 3; // Bombs in stock
    private bool jump = false;

    // Wizard parameters
    public float wizardMaxHP = 32; //Maximum Wizard Lives
    public float wizardAttackDamage = 10; // Damage from physical attack
    public int wizardReward = 2;// reward for defeating the enemy
    public float wizardSpeed = 2f;//Wizard Speed

    //Samurai Parameters
    public float martialMaxHP = 300; //Maximum Samurai Lives
    public float martialAttackDamage = 20; // Damage from physical attack
    public int martialReward = 2;// reward for defeating the enemy
    public float martialSpeed = 4f;//Martial Speed

    //Slime parameters
    public float slimeMaxHP = 8;//Maximum Slime lives
    public float slimeAttackDamage = 15; // Damage from physical attack
    public int slimeReward = 1;// reward for defeating the enemy
    public float slimeSpeed = 2f;//Slime speed

    //Boss Death parameters
    public float deathMaxHP = 900;//Maximum Death lives
    public float deathAttackDamage = 25; // Damage from physical attack
    public int deathReward = 40;// reward for defeating the enemy
    public float deathSpeed = 2f;//Death speed

    //Variable to record the coordinate difference between player and enemy
    public float directionX;
    public float directionY;

    // Enemy attack shells
    [SerializeField] private GameObject[] ammo;

    //General parameters
    private float jumpCooldown; //cooldown on rebound and jump
    private float physicCooldown = Mathf.Infinity; //cooldown on physical attack
    private float magicCooldown = Mathf.Infinity; //cooldown on mage attack
    public float stunCooldown; //stun recovery

    public float currentHP; // current Hp of the object
    public float takedDamage; // Damage caused to the object
    public float enemyAttackRange = 1.2f; //Range of physical attack
    public float currentAttackDamage; //current damage of the object
    public bool enemyDead = false; // Is the object dead
    public bool enemyTakeDamage = false; //If the object has sustained damage
    private bool stuned = false; //state of stun
    public bool block; //state of block
    public bool copy; // is this object a copy or not?
    private bool movement = false; //mob is not chasing the player
    private bool playerIsAttack; //Does the player attack?
    private bool isAttack; //If an object (enemy) is attacking
    private float speedRecovery;//need to restore speed 
    private int currentAttack = 0; //cooldown on object attack (animation)
    private float timeSinceAttack = 0.0f;// time since last attack, needed for combo attack animation
    private int level; //check what level the player is at, to connect abilities

    public GameObject player; //For identifying the player on the scene
    public GameObject meleeAttackArea; // Physical Weapons
    public Rigidbody2D rb; //Physical body
    private Animator anim; //Variable by which the object is animated
    private float e_delayToIdle = 0.0f;
    new string tag; // the object tag is assigned to this variable at the start

    [SerializeField] private Transform firePoint; //The position from which the shells will be fired
    [SerializeField] private GameObject[] blood; //blood
    public Vector3 lossyScale;
    public Vector3 thisObjectPosition;
    private CapsuleCollider2D capsuleCollider;

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


        if (tag == "Skeleton")
        {
            skeletonMaxHP = SaveSerial.Instance.skeletonHP;
            if (skeletonMaxHP == 0) skeletonMaxHP = 48;
            currentHP = skeletonMaxHP;

            skeletonAttackDamage = SaveSerial.Instance.skeletonDamage;
            if (skeletonAttackDamage == 0) skeletonAttackDamage = 10;
            currentAttackDamage = skeletonAttackDamage;

            skeletonSpeed = SaveSerial.Instance.skeletonSpeed;
            if (skeletonSpeed < 2f) skeletonSpeed = 2f;
            speedRecovery = skeletonSpeed;

        }
        if (tag == "Mushroom")
        {
            mushroomMaxHP = SaveSerial.Instance.mushroomHP;
            if (mushroomMaxHP == 0) mushroomMaxHP = 80;
            currentHP = mushroomMaxHP;

            mushroomAttackDamage = SaveSerial.Instance.mushroomDamage;
            if (mushroomAttackDamage == 0) mushroomAttackDamage = 10;
            currentAttackDamage = mushroomAttackDamage;

            moushroomSpeed = SaveSerial.Instance.mushroomSpeed;
            if (moushroomSpeed < 2f) moushroomSpeed = 2f;
            speedRecovery = moushroomSpeed;
        }
        if (tag == "FlyingEye")
        {
            flyingEyeMaxHP = SaveSerial.Instance.mushroomHP;
            if (flyingEyeMaxHP == 0) flyingEyeMaxHP = 24;
            currentHP = flyingEyeMaxHP;

            flyingEyeAttackDamage = SaveSerial.Instance.flyingEyeDamage;
            if (flyingEyeAttackDamage == 0) flyingEyeAttackDamage = 10;
            currentAttackDamage = flyingEyeAttackDamage;

            flyingEyeSpeed = SaveSerial.Instance.flyingEyeSpeed;
            if (flyingEyeSpeed < 2f) flyingEyeSpeed = 2f;
            speedRecovery = flyingEyeSpeed;
        }
        if (tag == "Goblin")
        {
            goblinMaxHP = SaveSerial.Instance.goblinHP;
            if (goblinMaxHP == 0) goblinMaxHP = 32;
            currentHP = goblinMaxHP;

            goblinAttackDamage = SaveSerial.Instance.goblinDamage;
            if (goblinAttackDamage == 0) goblinAttackDamage = 15;
            currentAttackDamage = goblinAttackDamage;

            goblinSpeed = SaveSerial.Instance.goblinSpeed;
            if (goblinSpeed < 2f) goblinSpeed = 2f;
            speedRecovery = goblinSpeed;
        }
        if (tag == "EvilWizard")
        {
            wizardMaxHP = SaveSerial.Instance.wizardHP;
            if (wizardMaxHP == 0) wizardMaxHP = 32;
            currentHP = wizardMaxHP;

            wizardAttackDamage = SaveSerial.Instance.wizardDamage;
            if (wizardAttackDamage == 0) wizardAttackDamage = 10;
            currentAttackDamage = wizardAttackDamage;

            wizardSpeed = SaveSerial.Instance.wizardSpeed;
            if (wizardSpeed < 2f) wizardSpeed = 2f;
            speedRecovery = wizardSpeed;
        }
        if (tag == "Martial")
        {
            martialMaxHP = SaveSerial.Instance.martialHP;
            if (martialMaxHP == 0) martialMaxHP = 300;
            currentHP = martialMaxHP;

            martialAttackDamage = SaveSerial.Instance.martialDamage;
            if (martialAttackDamage == 0) martialAttackDamage = 20;
            currentAttackDamage = martialAttackDamage;

            martialSpeed = SaveSerial.Instance.martialSpeed;
            if (martialSpeed < 4f) martialSpeed = 4f;
            speedRecovery = martialSpeed;
        }
        if (tag == "Slime")
        {
            if (slimeMaxHP == 0) slimeMaxHP = 8;
            currentHP = slimeMaxHP;

            if (slimeAttackDamage == 0) slimeAttackDamage = 15;
            currentAttackDamage = slimeAttackDamage;

            if (slimeSpeed < 2f) slimeSpeed = 2f;
            speedRecovery = slimeSpeed;
        }
        if (tag == "Death")
        {
            if (deathMaxHP == 0) deathMaxHP = 900;
            currentHP = deathMaxHP;

            if (deathAttackDamage == 0) deathAttackDamage = 25;
            currentAttackDamage = deathAttackDamage;

            if (deathSpeed < 2f) deathSpeed = 2f;
            speedRecovery = deathSpeed;
        }
    }
    void Update()
    {
        timeSinceAttack += Time.deltaTime; 
        blockCooldown += Time.deltaTime; 
        jumpCooldown += Time.deltaTime; 
        magicCooldown += Time.deltaTime; 
        physicCooldown += Time.deltaTime; 
        stunCooldown += Time.deltaTime; 

        if (currentHP > 0) EnemyBehavior(); 
    }
    //Method to describe different behaviour for different enemies. The choice of behaviour depends on the object tag
    public void EnemyBehavior()
    {
        AnimState();
        if (tag == "Skeleton")
        {
            EnemyMovement();
            SkeletonAttack();
            Block();
        }
        if (tag == "Mushroom")
        {
            //EnemyMovement();
            //MushroomAttack();
        }
        if (tag == "FlyingEye")
        {
            EnemyMovement();
            FlyingEyeAttack();
        }
        if (tag == "Goblin")
        {
            GoblinMovement();
            GoblinAttack();
        }
        if (tag == "EvilWizard")
        {
            EnemyMovement();
            EvilWizardAttack();
        }
        if (tag == "Martial")
        {
            EnemyMovement();
            MartialAttack();
        }
        if (tag == "Slime")
        {
            SlimeMovement();
            SlimeAttack();
        }
        if (tag == "Death")
        {
            DeathMovement();
            //EnemyMovement();
            DeathAttack();
        }
    }

    //General methods and behaviour
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
    private void MeleeAttack() //Basic method of attack with two or more animations
    {
        //Damage Deal
        currentAttack++;

        // Loop back to one after third attack
        if (currentAttack > 2) currentAttack = 1;

        // Reset Attack combo if time since last attack is too large
        if (timeSinceAttack > 2.0f) currentAttack = 1;
        anim.SetTrigger("attack" + currentAttack);

        // Reset timer
        timeSinceAttack = 0.0f;
    }
    public void EnemyAttack()
    {
        meleeAttackArea.transform.position = firePoint.position; //With each attack we will change projectile positions and give it a firing point position to receive the component from the projectile and send it in the direction of the player
        meleeAttackArea.GetComponent<MeleeWeapon>().MeleeDirection(firePoint.position);
        meleeAttackArea.GetComponent<MeleeWeapon>().GetAttackDamageInfo(currentAttackDamage);
    }
    public void MeleeWeaponOff() // deactivate the weapon object
    {
        meleeAttackArea.GetComponent<MeleeWeapon>().WeaponOff();
    }
    public void BoostEnemySpeed() //method to increase the speed of enemies
    {
        skeletonSpeed *= 1.1f;
        moushroomSpeed *= 1.1f;
        goblinSpeed *= 1.1f;
    }
    public void Flip() //This is where we create the Flip method which, when called, reverses the direction of the sprite
    {
        Vector3 theScale = transform.localScale; //receive the scale of the object
        theScale.x *= -1;//this flips the image e.g. 140 changes to -140, thus completely changing the direction of the sprite (the image is mirrored)
        transform.localScale = theScale; // scale conversion relative to the parent GameObjects object
    }
    public void PushFromPlayer() // rebound from a player
    {
        if (Mathf.Abs(directionX) < 1f)
        {
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (theScale.x > 0) rb.AddForce(new Vector2(-5, 1.5f), ForceMode2D.Impulse);
            if (theScale.x < 0) rb.AddForce(new Vector2(5, 1.5f), ForceMode2D.Impulse);
        }
    }
    public void Stun()
    {
        stunCooldown = 0;
        stuned = true;
        anim.SetBool("stun", true);
    }
    public void JumpToPlayer() // jump to player (Mushroom / Slime / Flying Eye)
    {
        if (level >= 1) //the ability is activated at level 3
        {
            jumpCooldown = 0;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (directionX > 0)
            {
                if (theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
                jumpSound.GetComponent<SoundOfObject>().PlaySound();
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                if (theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
                jumpSound.GetComponent<SoundOfObject>().PlaySound();
            }
        }
    }
    public void GetNameOfObject(GameObject gameObjectName) //Get a link to the game object, for summonses, so they can contact the master who summoned them
    {
        masterEnemy = gameObjectName;
    }
    public void LifeSteal() // Lifesteal from player
    {
        Hero.Instance.GetDamage(skeletonAttackDamage);// here we access the player's script and activate the GetDamage function from there
        float heal = skeletonAttackDamage * 0.5f; //The skeleton steals half of the damage the skeleton does to the player's xp
        currentHP += heal;
        float healBar = heal / (float)skeletonMaxHP; // how much to increase the progress bar
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//refresh progress bar
    }
    public void Push() //Method for repelling the body
    {
        if (transform.lossyScale.x < 0) this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(-0.5f, rb.velocity.y), ForceMode2D.Impulse);
        else this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0.5f, rb.velocity.y), ForceMode2D.Impulse);
    }
    public void TakeDamage(float dmg) //Damage (in dmg a value is specified, in the Hero script when the TakeDamage method is called, a variable of weapon damage is written to dmg ) 
    {
        float maxHP = 1;
        if (tag == "Skeleton") maxHP = skeletonMaxHP;
        if (tag == "Mushroom") maxHP = mushroomMaxHP;
        if (tag == "FlyingEye") maxHP = flyingEyeMaxHP;
        if (tag == "Goblin") maxHP = goblinMaxHP;
        if (tag == "EvilWizard") maxHP = wizardMaxHP;
        if (tag == "Martial") maxHP = martialMaxHP;
        if (tag == "Slime") maxHP = slimeMaxHP;
        if (tag == "Death") maxHP = deathMaxHP;

        isBlock = this.gameObject.GetComponent<Enemy_Behavior>().block;
        //Debug.Log(isBlock);
        if (currentHP > 0 && !isBlock)
        {
            if (tag != "Skeleton")
            {
                GameObject bloodSpawn = Instantiate(blood[Random.Range(0, blood.Length)], new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity); //Cloning an object
                bloodSpawn.gameObject.SetActive(true);
            }

            currentHP -= dmg;
            enemyTakeDamage = true;
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
            enemyTakeDamage = true;
            takedDamage = blockDMG / maxHP; //how much you need to reduce the progress bar
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//refresh progress bar
        }
        if (currentHP <= 0)
        {
            int reward = 2;
            if (tag == "Skeleton") reward = skeletonReward;
            if (tag == "Mushroom") reward = mushroomReward;
            if (tag == "FlyingEye") reward = mushroomReward;
            if (tag == "Goblin") reward = goblinReward;
            if (tag == "Martial") reward = martialReward;
            if (tag == "Slime") reward = 1;
            if (tag == "Death") reward = 40;
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
        if (tag == "Skeleton") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
        if (tag == "Mushroom") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
        if (tag == "FlyingEye" && !copy) LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
        if (tag == "FlyingEye" && copy && masterEnemy != null) masterEnemy.GetComponent<Enemy_Behavior>().CopyCounter();// copy destroying decreases the copy count, allowing you to call an extra copy.
        if (tag == "Goblin") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
        if (tag == "EvilWizard") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
        if (tag == "Martial") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
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

    //Special skins on mobs
    public void Block() // Using a shield (Skeleton)
    {
        playerIsAttack = Hero.Instance.isAttack;
        if (playerIsAttack == true && (Mathf.Abs(directionX)) < 2f && Mathf.Abs(directionY) < 2 && blockCooldown > 2)
        {
            blockCooldown = 0;
            skeletonSpeed = 0;
            block = true;
            anim.SetBool("Block", true);
        }
        if (blockCooldown > 0.8f)
        {
            skeletonSpeed = speedRecovery;
            block = false;
            anim.SetBool("Block", false);
        }
    }
    public void MushroomSpores() //creates a cloud of spore that damasks the player (Mushroom)
    {
        if (level > 0)
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
    public void CopyCounter()
    {
        countOfCopy -= 1;
    }
    public void GoblinJumpToPlayer() //jump to player (Goblin)
    {
        if (level >= 1) //the ability is activated at level 2
        {
            jumpCooldown = 0;
            if (directionX > 0) rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            if (directionX < 0) rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
        }
    }
    public void GoblinJumpFromPlayer() // rebound from player (Goblin)
    {
        if (level >= 1) //the ability is activated at level 2
        {
            jumpCooldown = 0;
            if (directionX > 0)
            {
                jump = true;
                anim.SetTrigger("roll");
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                jump = true;
                anim.SetTrigger("roll");
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
        }
    }
    public void GoblinBomb() // Bomb Throw (Goblin)
    {
        if (level >= 0 && remainingBombs >= 1)
        {
            remainingBombs -= 1;
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
        if (level < 5) remainingBombs = 0;
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

    //Methods of movement in different enemies
    public void EnemyMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis
        if ((Mathf.Abs(directionX) < 5 && Mathf.Abs(directionX) > 1.3f && Mathf.Abs(directionY) < 2) && !block && !isAttack && !stuned || enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack && !stuned || copy) 
        {
            Vector3 pos = transform.position; //object position
            Vector3 theScale = transform.localScale; // needed to understand the direction
            transform.localScale = theScale; // needed to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * Time.deltaTime; //calculating direction
            if (tag == "Skeleton") playerFollowSpeed = Mathf.Sign(directionX) * skeletonSpeed * Time.deltaTime; //calculating direction
            if (tag == "Mushroom") playerFollowSpeed = Mathf.Sign(directionX) * moushroomSpeed * Time.deltaTime; //calculating direction
            if (tag == "FlyingEye") playerFollowSpeed = Mathf.Sign(directionX) * flyingEyeSpeed * Time.deltaTime; //calculating direction
            if (tag == "Martial") playerFollowSpeed = Mathf.Sign(directionX) * martialSpeed * Time.deltaTime; //calculating direction
            if (tag == "Slime") playerFollowSpeed = Mathf.Sign(directionX) * slimeSpeed * Time.deltaTime; //calculating direction
            if (tag == "Death") playerFollowSpeed = Mathf.Sign(directionX) * deathSpeed * Time.deltaTime; //calculating direction
            pos.x += playerFollowSpeed; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        }
        else movement = false;
    }
    public void GoblinMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 3f && Mathf.Abs(directionY) < 2) && remainingBombs < 1 || enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) 
        {
            Vector3 pos = transform.position; //object position
            Vector3 theScale = transform.localScale; // needed to understand the direction
            transform.localScale = theScale; // needed to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * goblinSpeed * Time.deltaTime; //calculating direction
            pos.x += playerFollowSpeed; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        }
        else movement = false;
    }
    public void DeathMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 1.3f && Mathf.Abs(directionY) < 2) || enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) 
        {
            Vector3 pos = transform.position; //object position
            Vector3 theScale = transform.localScale; // needed to understand the direction
            transform.localScale = theScale; // needed to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * deathSpeed * Time.deltaTime; //calculating direction
            pos.x -= playerFollowSpeed; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
            movement = true;
        }
        else movement = false;
    }
    public void SlimeMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis
        if (Mathf.Abs(directionX) > 1f && !block && !isAttack || enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack) 
        {
            Vector3 pos = transform.position; //object position
            Vector3 theScale = transform.localScale; // needed to understand the direction
            transform.localScale = theScale; // needed to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * Time.deltaTime;
            if (tag == "Skeleton") playerFollowSpeed = Mathf.Sign(directionX) * skeletonSpeed * Time.deltaTime; //calculating direction
            if (tag == "Mushroom") playerFollowSpeed = Mathf.Sign(directionX) * moushroomSpeed * Time.deltaTime; //calculating direction
            if (tag == "Slime") playerFollowSpeed = Mathf.Sign(directionX) * slimeSpeed * Time.deltaTime; //calculating direction
            pos.x += playerFollowSpeed; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        }
        else movement = false;
    }
    //Attack methods for different mobs

    public void MushroomAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (stunCooldown > 3f) //exit from stun
        {
            stuned = false;
        }
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2 && !stuned) JumpToPlayer();
        if ((Mathf.Abs(directionX)) < 0.8f && magicCooldown > 10) MushroomSpores();
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            MeleeAttack();
        }
        else isAttack = false;
    }
    public void FlyingEyeAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (stunCooldown > 3f) //exit from stun
        {
            stuned = false;
        }
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2 && !stuned) JumpToPlayer();
        if ((Mathf.Abs(directionX)) < 5f && magicCooldown > 5 && !copy) SummonCopy(); 
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            MeleeAttack();
        }
        else isAttack = false;
    }
    public void SkeletonAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 1f && !block && timeSinceAttack > 1)
        {
            MeleeAttack();
        }
        else isAttack = false;
    }
    public void GoblinAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if ((Mathf.Abs(directionX)) < 5f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs < 1) GoblinJumpToPlayer();
        if ((Mathf.Abs(directionX)) < 2f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs >= 1) GoblinJumpFromPlayer();
        if ((Mathf.Abs(directionX)) < 4.5 && magicCooldown > 3 && !jump && remainingBombs >= 1 || enemyTakeDamage == true && magicCooldown > 3 && !jump && remainingBombs >= 1)
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
        if (jumpCooldown > 1.2f) jump = false;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            MeleeAttack();
        }
        else isAttack = false;
    }
    public void EvilWizardAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (stunCooldown > 3f) //exit from stun
        {
            stuned = false;
            anim.SetBool("stun", false);
        }

        if (playerHP > 0 && Mathf.Abs(directionX) < 6f && (Mathf.Abs(directionX)) > 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2 && !stuned && level >= 1)
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
        if (playerHP > 0 && (Mathf.Abs(directionX)) < 2f && Mathf.Abs(directionY) < 2 && !stuned)
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
                float fireDMG = 150f * wizardAttackDamage * Time.deltaTime; 
                Hero.Instance.GetDamage(fireDMG);
            }
        }
    }
    public void MartialAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if (stunCooldown > 2f) 
        {
            stuned = false;
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 2.5f && Mathf.Abs(directionY) < 1.5f && timeSinceAttack > 1 && !stuned)
        {
            MeleeAttack();
        }
        else isAttack = false;
    }
    public void SlimeAttack()
    {
        float playerHP = Hero.Instance.curentHP;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2) JumpToPlayer();
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            anim.SetTrigger("spin");
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
    }
    public void DeathAttack()
    {
        float playerHP = Hero.Instance.curentHP;

        if (playerHP > 0 && Mathf.Abs(directionX) < 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2)
        {
            anim.SetTrigger("attack1");
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
        if ((Mathf.Abs(directionX)) < 8f && (Mathf.Abs(directionX)) > 2 && Mathf.Abs(directionY) < 2f || enemyTakeDamage == true)
        {
            SpellDrainHP();
            DeathSummonMinioins();
        }
    }

    //The section where enemy characteristics are increased, if a new enemy is added, its characteristics should be added here
    public void BoostEnemyHP()
    {
        skeletonMaxHP *= 1.2f;
        mushroomMaxHP *= 1.2f;
        goblinMaxHP *= 1.2f;
        wizardMaxHP *= 1.2f;
        martialMaxHP *= 1.2f;
        flyingEyeMaxHP *= 1.2f;
    }
    public void BoostEnemyAttackDamage() //thereby increasing the damage
    {
        skeletonAttackDamage *= 1.2f;
        mushroomAttackDamage *= 1.2f;
        goblinAttackDamage *= 1.2f;
        wizardAttackDamage *= 1.2f;
        martialAttackDamage *= 1.2f;
        flyingEyeAttackDamage *= 1.2f;
    }
    public void BoostEnemyReward() //there we increase the reward for the kill
    {
        skeletonReward += 2;
        mushroomReward += 2;
        goblinReward += 2;
        wizardReward += 2;
        martialReward += 2;
        flyingEyeReward += 2;
    }

    //Attack methods for different mobs
    public void BossDeathHeal(float heal)
    {
        currentHP += heal;
        float healBar = heal / deathMaxHP; // how much to increase the progress bar
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//refresh progress bar
    }
    public void BossDeathDamage(float dmg)
    {
        currentHP -= dmg;
        enemyTakeDamage = true;
        takedDamage = dmg / deathMaxHP; //how much you need to reduce the progress bar
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//refresh progress bar
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