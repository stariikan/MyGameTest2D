using UnityEngine;

public class Enemy_Behavior : MonoBehaviour
{
    //Skeleton parameters
    public float skeletonSpeed = 2f;//Skeleton speed
    private float blockCooldown; //сooldown block

    //Mushroom parameters
    public float moushroomSpeed = 2f;//Mushroom's speed

    //Flying Eye parameters
    public float flyingEyeSpeed = 2f;// Speed of the Eye
    private int countOfCopy; // initially 0, when the call occurs become 3, as copies die 

    //Goblin Parameters
    public float goblinSpeed = 3f;//Goblin Speed
    public int remainingBombs = 3; // Bombs in stock
    private bool jump = false;

    // Wizard parameters
    public float wizardSpeed = 2f;//Wizard Speed

    //Samurai Parameters
    public float martialSpeed = 4f;//Martial Speed

    //Slime parameters
    public float slimeSpeed = 2f;//Slime speed

    //Boss Death parameters
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
    public Rigidbody2D rb; //Physical body
    private Animator anim; //Variable by which the object is animated
    private float e_delayToIdle = 0.0f;
    new string tag; // the object tag is assigned to this variable at the start



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
        tag = this.gameObject.transform.tag;
        level = LvLGeneration.Instance.Level;

        if (tag == "Skeleton")
        {
            skeletonSpeed = SaveSerial.Instance.skeletonSpeed;
            if (skeletonSpeed < 2f) skeletonSpeed = 2f;
            speedRecovery = skeletonSpeed;

        }
        if (tag == "Mushroom")
        {
            moushroomSpeed = SaveSerial.Instance.mushroomSpeed;
            if (moushroomSpeed < 2f) moushroomSpeed = 2f;
            speedRecovery = moushroomSpeed;
        }
        if (tag == "Goblin")
        {
            goblinSpeed = SaveSerial.Instance.goblinSpeed;
            if (goblinSpeed < 2f) goblinSpeed = 2f;
            speedRecovery = goblinSpeed;
        }
        if (tag == "Martial")
        {
            martialSpeed = SaveSerial.Instance.martialSpeed;
            if (martialSpeed < 4f) martialSpeed = 4f;
            speedRecovery = martialSpeed;
        }
        if (tag == "Slime")
        {
            if (slimeSpeed < 2f) slimeSpeed = 2f;
            speedRecovery = slimeSpeed;
        }
        if (tag == "Death")
        {
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

        if (this.gameObject.GetComponent<Entity_Enemy>().currentHP > 0) EnemyBehavior(); 
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
            EnemyMovement();
            MushroomAttack();
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
            //DeathMovement();
            EnemyMovement();
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
        if (level > 4)
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
        if (level > 4 && countOfCopy < 1)
        {
            magicCooldown = 0;
            Vector3 pos = transform.position;
            GameObject guard1 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1.5f, pos.y, pos.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            guard1.name = "Enemy" + Random.Range(1, 999);
            guard1.GetComponent<Entity_Enemy>().GetNameOfObject(this.gameObject);
            GameObject guard2 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1f, pos.y, pos.z), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            guard2.name = "Enemy" + Random.Range(1, 999);
            guard2.GetComponent<Entity_Enemy>().GetNameOfObject(this.gameObject);
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
        if (level >= 5 && remainingBombs >= 1)
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
        if ((Mathf.Abs(directionX) < 5 && Mathf.Abs(directionX) > 1.3f && Mathf.Abs(directionY) < 2) && !block && !isAttack && !stuned || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack && !stuned || copy) 
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
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 3f && Mathf.Abs(directionY) < 2) && remainingBombs < 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) 
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
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 1.3f && Mathf.Abs(directionY) < 2) || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) 
        {
            Vector3 pos = transform.position; //object position
            Vector3 theScale = transform.localScale; // needed to understand the direction
            transform.localScale = theScale; // needed to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * deathSpeed * Time.deltaTime; //calculating direction
            pos.x -= playerFollowSpeed; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
        }
        else movement = false;
    }
    public void SlimeMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis
        if (Mathf.Abs(directionX) > 1f && !block && !isAttack || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack) 
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
        if ((Mathf.Abs(directionX)) < 4.5 && magicCooldown > 3 && !jump && remainingBombs >= 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && magicCooldown > 3 && !jump && remainingBombs >= 1)
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

        if (playerHP > 0 && Mathf.Abs(directionX) < 6f && (Mathf.Abs(directionX)) > 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2 && !stuned && level >= 5)
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
                float fireDMG = 150f * (Entity_Enemy.Instance.wizardAttackDamage) * Time.deltaTime; 
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
        if ((Mathf.Abs(directionX)) < 8f && (Mathf.Abs(directionX)) > 2 && Mathf.Abs(directionY) < 2f || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true)
        {
            SpellDrainHP();
            DeathSummonMinioins();
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