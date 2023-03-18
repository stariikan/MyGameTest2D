using UnityEngine;

public class Enemy_Behavior : MonoBehaviour //������������ ������ �������� (�� ���� ������ ������� ������������ � Entity ����� ��������� � � ����� �������)
{
    //��������� �������
    public float skeletonSpeed = 2f;//�������� �������
    private float blockCooldown; //������� �����

    //��������� �����
    public float moushroomSpeed = 2f;//�������� �����

    //��������� �����
    public float flyingEyeSpeed = 2f;//�������� �����

    //��������� �������
    public float goblinSpeed = 3f;//�������� �������
    public int remainingBombs = 3; //����� 3 ����
    private bool jump = false;

    //��������� ����� ����
    public float wizardSpeed = 2f;//�������� �������
    private bool stuned = false; //���� �������
    public float stunCooldown; //������� �����
                               
    //��������� ������� ����
    public float martialSpeed = 4f;//�������� �������

    //��������� ������
    public float slimeSpeed = 2f;//�������� ������

    //��������� ���� ������
    public float deathSpeed = 2f;//�������� ������

    //���������� ��� ������ ������� ��������� ����� ������� � ������
    public float directionX;
    public float directionY;

    //������� ��� ����� ������
    [SerializeField] private GameObject[] ammo;

    //����� ���������
    private float jumpCooldown; //������� �� ������ � ������
    private float physicCooldown = Mathf.Infinity; //������� �� ��� �����
    private float magicCooldown = Mathf.Infinity; //������� �� ��� �����

    
    public bool block = false;
    public bool copy; //���� ������ ����� ��� ���?
    private bool movement = false; //��� �� ���������� ������
    private bool playerIsAttack; //������� �� �����?
    private bool isAttack; //������� �� ������ (����)
    private float speedRecovery;//����� ��� �������������� �������� 
    private int currentAttack = 0; //������� �� ����� �������
    private float timeSinceAttack = 0.0f;//����� � ������� ����� ����� ��� ����� �������� �����
    private int level; //�������� ����� ������� �������� �����, ����� ��� ����������� ������������

    public GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    private Animator anim; //���������� ��������� ������� ���������� ������
    private float e_delayToIdle = 0.0f;
    new string tag; // � ���� ���������� ������������� ��� ������� �� ������
    public static Enemy_Behavior Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void Start()
    {
        Instance = this;
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object) � �������� �������� ������
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object) � �������� �������� ������
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
        timeSinceAttack += Time.deltaTime; //�� �����
        blockCooldown += Time.deltaTime; //�� �����
        jumpCooldown += Time.deltaTime; //�� ������
        magicCooldown += Time.deltaTime; //�� ��� ������
        physicCooldown += Time.deltaTime; //�� ��� ������
        stunCooldown += Time.deltaTime; //�� �����



        if (this.gameObject.GetComponent<Entity_Enemy>().currentHP > 0) EnemyBehavior(); 
    }
    //����� ����������� ������ ��������� ��� ������ ������. ����� ��������� ������ �� ���� �������
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
            Block();
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
            DeathAttack();
        }
    }
    
    //����� ������ � ���������
    public enum States //����������� ����� ������ ���������, ������ �������� ��� � ��������� Unity
    {
        idle,
        run
    }
    public void AnimState()//����� ��� ����������� ������ ��������
    {
        if (movement == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if (movement == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0) this.gameObject.GetComponent<Animator>().SetInteger("State", 0);

        }
    }
    public void BoostEnemySpeed() //����� ��� �������� �������� ������
    {
        skeletonSpeed *= 1.1f;
        moushroomSpeed *= 1.1f;
        goblinSpeed *= 1.1f;
    }
    public void Flip() //��� �� ������� ����� Flip ��� ������ �������� ������ ������ �����������
    {
        Vector3 theScale = transform.localScale; //��������� ������� �������
        theScale.x *= -1;//��� ���������� ��������� ����������� �������� 140 �������� �� -140 ��� ����� ��������� ������ ����������� ������� (�������� ���������������)
        transform.localScale = theScale; //������� �������������� ������������ ������������� ������� GameObjects
    }
    public void PushFromPlayer() // ������ �� ������
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
    public void JumpToPlayer() //������ � ������ (���� / ����� / �������� ����)
    {
        if (level >= 1) //����������� ������������ �� 3 ������
        {
            jumpCooldown = 0;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (directionX > 0)
            {
                if (theScale.x < 0) Flip();//���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                if (theScale.x > 0) Flip();//���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
            }
        }
    }
    
    //������ ����� � �����
    public void Block() // ������������� ���� (������)
    {
        playerIsAttack = Hero.Instance.isAttack;
        if (playerIsAttack == true && (Mathf.Abs(directionX)) < 1.5f && Mathf.Abs(directionY) < 2 && level > 1)
        {
            blockCooldown = 0;
            skeletonSpeed = 0;
            block = true;
            anim.SetBool("Block", true);
        }
        if (blockCooldown > 0.4f || directionX > 2f)
        {
            skeletonSpeed = speedRecovery;
            block = false;
            anim.SetBool("Block", false);
        }
    }

    public void MushroomSpores() //������� ������ ���� ������� ������� ������ (����)
    {
        if (level > 4)
        {
            magicCooldown = 0; // ����� ������� ����
            Vector3 MoushroomScale = transform.localScale; //������ ��������� �������� ������� ��������
            transform.localScale = MoushroomScale; //������ ��������� �������� ������� ��������
            Vector3 sporeSpawnPosition = this.gameObject.transform.position; //������ ������� ��������
            GameObject newSpore = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(sporeSpawnPosition.x, sporeSpawnPosition.y, sporeSpawnPosition.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            newSpore.name = "spore" + Random.Range(1, 999);
            if (MoushroomScale.x < 0) sporeSpawnPosition.x -= 0.8f; //����������� ���� ������ �������� � ����������� �� �������� �������
            if (MoushroomScale.x > 0) sporeSpawnPosition.x += 0.8f; //����������� ���� ������ �������� � ����������� �� �������� �������
            newSpore.GetComponent<Spore>().sporeDirection(sporeSpawnPosition); //�������� ���������� ��� ������ ������ ����
        }
    }
    public void SummonCopy() //������� ����� ��������� �����
    {
        if (level > 4 && !copy)
        {
            magicCooldown = 0;
            Vector3 pos = transform.position;
            GameObject guard1 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1.5f, pos.y, pos.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            guard1.name = "Enemy" + Random.Range(1, 999);
            GameObject guard2 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1f, pos.y, pos.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            guard2.name = "Enemy" + Random.Range(1, 999);
            GameObject guard3 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 2f, pos.y, pos.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            guard3.name = "Enemy" + Random.Range(1, 999);
        }
        else return;

    }
    public void GoblinJumpToPlayer() //������ � ������ (������)
    {
        if (level >= 1) //����������� ������������ �� 3 ������
        {
            jumpCooldown = 0;
            if (directionX > 0) rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            if (directionX < 0) rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
        }
    }
    public void GoblinJumpFromPlayer() // ������ �� ������ (������)
    {
        if (level >= 1) //����������� ������������ �� 3 ������
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
    public void GoblinBomb() //������ ����� (������)
    {
        if (level >= 5 && remainingBombs >= 1)
        {
            remainingBombs -= 1;
            magicCooldown = 0; // ����� ������� ����
            Vector3 goblinScale = transform.localScale; //������ ��������� �������� ������� �������
            transform.localScale = goblinScale; //������ ��������� �������� ������� �������
            Vector3 bombSpawnPosition = this.gameObject.transform.position; //������ ������� �������
            GameObject bombBall = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(bombSpawnPosition.x, bombSpawnPosition.y, bombSpawnPosition.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            bombBall.name = "Bomb" + Random.Range(1, 999);
            if (goblinScale.x < 0) bombSpawnPosition.x -= 1f; //����������� ����� ������ ������� � ����������� �� �������� �������
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
        GameObject fireBall = Instantiate(ammo[0], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
        fireBall.name = "fireball" + Random.Range(1, 999);

        fireBall.GetComponent<FireBall>().SetDirection(shootingDirection);
    }
    public void DeathSummonMinioins() //������ ������� (���� ������)
    {
        if (physicCooldown >= 8)
        {
            physicCooldown = 0; // ����� �������
            anim.SetTrigger("cast1");
            Vector3 spellSpawnPosition = this.gameObject.transform.position; //������ ������� ������
            spellSpawnPosition.x -= 2f;
            SummonSlime.Instance.SummonDirection(spellSpawnPosition); //�������� ���������� ��� ������ �����
        }
    }
    public void SpellDrainHP() //������������ ����� ����� ������ (���� ������)
    {
        if (magicCooldown >= 3)
        {
            magicCooldown = 0; // ����� �������
            anim.SetTrigger("cast1");
            Vector3 spellSpawnPosition = player.transform.position; //������ ������� ������
            spellSpawnPosition.y += 1.7f; // ����� ����� ����� ���������� ���� ���� ������
            DrainHP.Instance.DrainHPDirection(spellSpawnPosition); //�������� ���������� ��� ������ �����
        }
    }

    //������ ������������ � ������ ������
    public void EnemyMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if ((Mathf.Abs(directionX) < 5 && Mathf.Abs(directionX) > 1.5f && Mathf.Abs(directionY) < 2) && !block && !isAttack && !stuned || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack && !stuned || copy) //������� �� ������� ���� ��������� ��������� ��� ������� ����
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float playerFollowSpeed = Mathf.Sign(directionX) * Time.deltaTime;
            if (tag == "Skeleton") playerFollowSpeed = Mathf.Sign(directionX) * skeletonSpeed * Time.deltaTime; //���������� �����������
            if (tag == "Mushroom") playerFollowSpeed = Mathf.Sign(directionX) * moushroomSpeed * Time.deltaTime; //���������� �����������
            if (tag == "FlyingEye") playerFollowSpeed = Mathf.Sign(directionX) * flyingEyeSpeed * Time.deltaTime; //���������� �����������
            if (tag == "Martial") playerFollowSpeed = Mathf.Sign(directionX) * martialSpeed * Time.deltaTime; //���������� �����������
            if (tag == "Slime") playerFollowSpeed = Mathf.Sign(directionX) * slimeSpeed * Time.deltaTime; //���������� �����������
            pos.x += playerFollowSpeed; //���������� ������� �� ��� �
            transform.position = pos; //���������� �������
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();//���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();//���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
        }
        else movement = false;
    }
    public void GoblinMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 3f && Mathf.Abs(directionY) < 2) && remainingBombs < 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) //������� �� ������� ���� ��������� ��������� ��� ������� ����
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float playerFollowSpeed = Mathf.Sign(directionX) * goblinSpeed * Time.deltaTime; //���������� �����������
            pos.x += playerFollowSpeed; //���������� ������� �� ��� �
            transform.position = pos; //���������� �������
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();//���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();//���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
        }
        else movement = false;
    }
    public void DeathMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) //������� �� ������� ���� ��������� ��������� ��� ������� ����
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float playerFollowSpeed = Mathf.Sign(directionX) * deathSpeed * Time.deltaTime; //���������� �����������
            pos.x -= playerFollowSpeed; //���������� ������� �� ��� �
            transform.position = pos; //���������� �������
            movement = true;
        }
        else movement = false;
    }
    public void SlimeMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if (Mathf.Abs(directionX) > 1f && !block && !isAttack || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack) //������� �� ������� ���� ��������� ��������� ��� ������� ����
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float playerFollowSpeed = Mathf.Sign(directionX) * Time.deltaTime;
            if (tag == "Skeleton") playerFollowSpeed = Mathf.Sign(directionX) * skeletonSpeed * Time.deltaTime; //���������� �����������
            if (tag == "Mushroom") playerFollowSpeed = Mathf.Sign(directionX) * moushroomSpeed * Time.deltaTime; //���������� �����������
            if (tag == "Slime") playerFollowSpeed = Mathf.Sign(directionX) * slimeSpeed * Time.deltaTime; //���������� �����������
            pos.x += playerFollowSpeed; //���������� ������� �� ��� �
            transform.position = pos; //���������� �������
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();//���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();//���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
        }
        else movement = false;
    }
    //������ ����� � ������ �����
    public void MushroomAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (stunCooldown > 3f) //����� �� �����
        {
            stuned = false;
        }
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2 && !stuned) JumpToPlayer();
        if ((Mathf.Abs(directionX)) < 0.8f && magicCooldown > 10) MushroomSpores();
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            //Damage Deal
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 2.0f)
                currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack);
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
    }
    public void FlyingEyeAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (stunCooldown > 3f) //����� �� �����
        {
            stuned = false;
        }
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2 && !stuned) JumpToPlayer();
        if ((Mathf.Abs(directionX)) < 5f && magicCooldown > 5) SummonCopy(); 
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            //Damage Deal
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 2.0f)
                currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack);
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
    }
    public void SkeletonAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && !block && timeSinceAttack > 1)
        {
            isAttack = true;
            //Damage Deal
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 2.0f)
                currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack);
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
    }
    public void GoblinAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 5f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs < 1) GoblinJumpToPlayer();
        if ((Mathf.Abs(directionX)) < 2f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs >= 1) GoblinJumpFromPlayer();
        if ((Mathf.Abs(directionX)) < 4.5 && magicCooldown > 3 && !jump && remainingBombs >= 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && magicCooldown > 3 && !jump && remainingBombs >= 1)
        {
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            if (directionX < 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                if (theScale.x > 0) Flip();
                GoblinBomb();
            }
            else if (directionX > 0) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
            {
                if (theScale.x < 0) Flip();
                GoblinBomb();
            }
        }
        if (jumpCooldown > 1.2f) jump = false;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            //Damage Deal
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 2.0f)
                currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack);
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
    }
    public void EvilWizardAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (stunCooldown > 3f) //����� �� �����
        {
            stuned = false;
            anim.SetBool("stun", false);
        }

        if (playerHP > 0 && Mathf.Abs(directionX) < 6f && (Mathf.Abs(directionX)) > 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2 && !stuned)
        {
            anim.SetTrigger("attack1");
            timeSinceAttack = 0.0f;
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            if (directionX < 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                if (theScale.x > 0) Flip();
                MagicAttack();
            }
            else if (directionX > 0) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
            {
                if (theScale.x < 0) Flip();
                MagicAttack();
            }
        }
        else isAttack = false;
        if (playerHP > 0 && (Mathf.Abs(directionX)) < 2f && Mathf.Abs(directionY) < 2 && !stuned)
        {
            anim.SetTrigger("attack2");
            timeSinceAttack = 0.0f;
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������ �� ��� �
            float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������ �� ��� y
            if ((Mathf.Abs(directionX) < 2f && Mathf.Abs(directionY) < 2f) && magicCooldown > 0.5 && playerHP > 0)
            {
                if (directionX < 0 && theScale.x > 0) Flip();
                else if (directionX > 0 && theScale.x < 0) Flip();
                timeSinceAttack = 0.0f;
                magicCooldown = 0;
                float fireDMG = 100f * (Entity_Enemy.Instance.wizardAttackDamage) * Time.deltaTime; 
                Hero.Instance.GetDamage(fireDMG);
            }
        }
    }
    public void MartialAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (stunCooldown > 2f) //����� �� �����
        {
            stuned = false;
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 2.5f && Mathf.Abs(directionY) < 1.5f && timeSinceAttack > 1 && !stuned)
        {
            //Damage Deal
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 2.0f)
                currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack);
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
    }
    public void SlimeAttack()
    {
        float playerHP = Hero.Instance.hp;
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
        float playerHP = Hero.Instance.hp;

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
}
