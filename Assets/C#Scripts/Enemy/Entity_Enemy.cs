using UnityEngine;

public class Entity_Enemy : MonoBehaviour
{
    //Skeleton parameters
    public float skeletonMaxHP = 70; // Maximum skeleton lives
    public float skeletonAttackDamage = 10; // Damage from physical attack
    public int skeletonReward = 2;// reward for defeating the enemy
    private bool isBlock; //check whether the block is set
    private float blockDMG;

    //Mushroom parameters
    public float mushroomMaxHP = 70; //Maximum lives of the Mushroom
    public float mushroomAttackDamage = 10; // Damage from physical attack
    public int mushroomReward = 2;// reward for defeating the enemy

    //Flying Eye parameters
    public float flyingEyeMaxHP = 70; //Maximum Flying Eye lives
    public float flyingEyeAttackDamage = 10; // Damage from physical attack
    public int flyingEyeReward = 2;// reward for defeating the enemy
    private GameObject masterEnemy; //this will link to the eye wizard who calls on the other eyes

    //Goblin Parameters
    public float goblinMaxHP = 50;  //Maximum Goblin Lives
    public float goblinAttackDamage = 15; // Damage from physical attack
    public int goblinReward = 2;// reward for defeating the enemy

    //The Mage parameters
    public float wizardMaxHP = 50; //Maximum Wizard Lives
    public float wizardAttackDamage = 10; // Damage from physical attack
    public int wizardReward = 2;// reward for defeating the enemy

    //Samurai Parameters
    public float martialMaxHP = 75; //Maximum Samurai Lives
    public float martialAttackDamage = 20; // Damage from physical attack
    public int martialReward = 2;// reward for defeating the enemy

    //Slime parameters
    public float slimeMaxHP = 40;//Maximum Slime lives
    public float slimeAttackDamage = 15; // Damage from physical attack
    public int slimeReward = 1;// reward for defeating the enemy

    //Boss Death Parameters
    public float deathMaxHP = 900;//Maximum Death lives
    public float deathAttackDamage = 25; // Damage from physical attack
    public int deathReward = 40;// reward for defeating the enemy

    //Replacement to record the coordinate difference between player and enemy
    private float directionY; 
    private float directionX;

    //General parameters
    public float currentHP; // current of Hp the object
    public float takedDamage; // Damage caused to the object
    public float enemyAttackRange = 1.2f; //Range of physical attack
    public bool enemyDead = false; // Is the object dead
    public bool enemyTakeDamage = false; //If the object has sustained damage

    [SerializeField] private Transform firePoint; //The position from which the shells will be fired
    [SerializeField] private GameObject[] blood; //blood
    public Vector3 lossyScale;
    public Vector3 thisObjectPosition;
    private Rigidbody2D e_rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    new string tag; // to this variable is assigned a tag at the start
    public static Entity_Enemy Instance { get; set; } // To collect and send data from this script

    private void Start()
    {
        Instance = this;
        anim = this.gameObject.GetComponent<Animator>(); // Variable anim receives information from the Animator component (Animator.Object)
        e_rb = this.gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        tag = this.gameObject.transform.tag;

        if (tag == "Skeleton")
        {
            skeletonMaxHP = SaveSerial.Instance.skeletonHP;
            if (skeletonMaxHP == 0) skeletonMaxHP = 70;
            currentHP = skeletonMaxHP;
            skeletonAttackDamage = SaveSerial.Instance.skeletonDamage;
            if (skeletonAttackDamage == 0) skeletonAttackDamage = 10;
        }
        if (tag == "Mushroom")
        {
            mushroomMaxHP = SaveSerial.Instance.mushroomHP;
            if (mushroomMaxHP == 0) mushroomMaxHP = 70;
            currentHP = mushroomMaxHP;
            mushroomAttackDamage = SaveSerial.Instance.mushroomDamage;
            if (mushroomAttackDamage == 0) mushroomAttackDamage = 10;
        }
        if (tag == "FlyingEye")
        {
            flyingEyeMaxHP = SaveSerial.Instance.mushroomHP;
            if (flyingEyeMaxHP == 0) flyingEyeMaxHP = 70;
            currentHP = flyingEyeMaxHP;
            flyingEyeAttackDamage = SaveSerial.Instance.flyingEyeDamage;
            if (flyingEyeAttackDamage == 0) flyingEyeAttackDamage = 10;
        }
        if (tag == "Goblin")
        {
            goblinMaxHP = SaveSerial.Instance.goblinHP;
            if (goblinMaxHP == 0) goblinMaxHP = 50;
            currentHP = goblinMaxHP;
            goblinAttackDamage = SaveSerial.Instance.goblinDamage;
            if (goblinAttackDamage == 0) goblinAttackDamage = 15;
        }
        if (tag == "EvilWizard")
        {
            wizardMaxHP = SaveSerial.Instance.wizardHP;
            if (wizardMaxHP == 0) wizardMaxHP = 50;
            currentHP = wizardMaxHP;
            wizardAttackDamage = SaveSerial.Instance.wizardDamage;
            if (wizardAttackDamage == 0) wizardAttackDamage = 10;
        }
        if (tag == "Martial")
        {
            martialMaxHP = SaveSerial.Instance.martialHP;
            if (martialMaxHP == 0) martialMaxHP = 75;
            currentHP = martialMaxHP;
            martialAttackDamage = SaveSerial.Instance.martialDamage;
            if (martialAttackDamage == 0) martialAttackDamage = 20;
        }
        if (tag == "Slime")
        {
            if (slimeMaxHP == 0) slimeMaxHP = 40;
            currentHP = slimeMaxHP;
            if (slimeAttackDamage == 0) slimeAttackDamage = 15;
        }
        if (tag == "Death")
        {
            if (deathMaxHP == 0) deathMaxHP = 900;
            currentHP = deathMaxHP;
            if (deathAttackDamage == 0) deathAttackDamage = 25;
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

    //General methods and behaviour
    public void GetNameOfObject(GameObject gameObjectName) //Get a link to the game object, for summonses, so they can contact the master who summoned them
    {
        masterEnemy = gameObjectName;
    }
    public void DamageDeealToPlayer() // A method for dealing damage to the Player
    {
        directionX = Enemy_Behavior.Instance.directionX;
        directionY = Enemy_Behavior.Instance.directionY;
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Skeleton")
        {
            Hero.Instance.GetDamage(skeletonAttackDamage);// here we access the player's script and activate the GetDamage function from there
            float heal = skeletonAttackDamage * 0.5f; //The skeleton steals half of the damage the skeleton does to the player's xp
            currentHP += heal;
            float healBar = heal / (float)skeletonMaxHP; // how much to increase the progress bar
            if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//refresh progress bar
        }
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Mushroom") Hero.Instance.GetDamage(mushroomAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "FlyingEye") Hero.Instance.GetDamage(mushroomAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Goblin") Hero.Instance.GetDamage(goblinAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Slime") Hero.Instance.GetDamage(slimeAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Martial") Hero.Instance.GetDamage(martialAttackDamage);
        if (directionX < 1.8f && currentHP > 0 && directionY < 1f && tag == "Death")
        {
            Hero.Instance.GetDamage(deathAttackDamage);
            float heal = deathAttackDamage * 0.5f; //Death steals half the damage a skeleton does to the player's xp
            currentHP += heal;
            float healBar = heal / (float)deathMaxHP; // how much to increase the progress bar
            this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);  //refresh progress bar
        }
    }
    public void Push() //Method for repelling the body
    {
        if (transform.lossyScale.x < 0) this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(-0.5f, e_rb.velocity.y), ForceMode2D.Impulse);
        else this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0.5f, e_rb.velocity.y), ForceMode2D.Impulse);
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
            e_rb.gravityScale = 0;
            e_rb.velocity = Vector2.zero;
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
                    obj.GetComponent<Entity_Enemy>().BossDeathDamage(50);
                }
            }
        }
        if (tag == "Death") LvLGeneration.Instance.FindKey();//call a method to retrieve the keys
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
}

