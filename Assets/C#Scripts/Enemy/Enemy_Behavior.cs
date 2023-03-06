using UnityEngine;

public class Enemy_Behavior : MonoBehaviour //������������ ������ �������� (�� ���� ������ ������� ������������ � Entity ����� ��������� � � ����� �������)
{
    public float skeletonSpeed = 2f;//�������� �������
    private float blockCooldown;
    public bool skeleton_block = false;

    public float moushroomSpeed = 2f;//�������� �����
    
    private float sporesCooldown = 10f; //������� ����� ����

    public float goblinSpeed = 3f;//�������� �������
    private float bombCooldown = 4f; //������� ������ �����
    public int remainingBombs = 3; //����� 3 ����
    private bool jump = false;

    public float slimeSpeed = 2f;//�������� ������

    public float deathSpeed = 2f;//�������� ������

    private float speedRecovery;//����� ��� �������������� �������� 

    private float jumpCooldown; //������� �� ������ � ������
    private bool movement = false; //��� �� ���������� ������
    private bool playerIsAttack; //������� �� �����?
    private bool isAttack; //������� �� ������ (����)

    public float directionX; //���������� ��� ��������� ������� ����� ������� � ������
    public float directionY; //���������� ��� ��������� ������� ����� ������� � ������
    private int currentAttack = 0; //������� �� ����� �������
    private float timeSinceAttack = 0.0f;//����� � ������� ����� ����� ��� ����� �������� �����
    private int level; //�������� ����� ������� �������� �����, ����� ��� ����������� ������������

    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    private Animator anim; //���������� ��������� ������� ���������� ������, ���������� = skelet_anim
    private float e_delayToIdle = 0.0f;
    string tag; // � ���� ���������� ������������� ��� �� ������

    public static Enemy_Behavior Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void Start()
    {
        Instance = this;
        player = GameObject.FindWithTag("PlayerCharacter"); //��� ��� ������ ���� ������ ������� ������ �� ���� Player � ����������� ��������� � ���������� ���������� player
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
            moushroomSpeed = SaveSerial.Instance.moushroomSpeed;
            if (moushroomSpeed < 2f) moushroomSpeed = 2f;
            speedRecovery = moushroomSpeed;
        }
        if (tag == "Goblin")
        {
            goblinSpeed = SaveSerial.Instance.goblinSpeed;
            if (goblinSpeed < 2f) goblinSpeed = 2f;
            speedRecovery = goblinSpeed;
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
    void Update() //��� ���������� ���� �������� �������� ������ (������� ������ ������������ ����)
    {
        timeSinceAttack += Time.deltaTime;
        blockCooldown += Time.deltaTime;
        jumpCooldown += Time.deltaTime;
        sporesCooldown += Time.deltaTime;
        bombCooldown += Time.deltaTime;

        if (this.gameObject.GetComponent<Entity_Enemy>().currentHP > 0)
        {
            DieByFall(); // ������ ��� �������
            AnimState(); //����������� ��������
            EnemyBehavior(); //��������� �����
        }
        else
        {
            return;
        }
    }
    public void EnemyBehavior()
    {
        if (tag == "Skeleton")
        {
            EnemyMovement();
            SkeletonAttack();
            Block();
        }
        if (tag == "Mushroom")
        {
            EnemyMovement();
            MoushroomAttack();
        }
        if (tag == "Goblin")
        {
            GoblinMovement();
            GoblinAttack();
        }
        if (tag == "Slime")
        {
            EnemyMovement();
            SlimeAttack();
        }
        if (tag == "Death")
        {
            EnemyMovement();
            DeathAttack();
        }
    }
    public enum States //����������� ����� ������ ���������, ������ �������� ��� � ��������� Unity
    {
        idle,
        run
    }
    public void Flip() //��� �� ������� ����� Flip ��� ������ �������� ������ ������ �����������
    {
        Vector3 theScale = transform.localScale; //��������� ������� �������
        theScale.x *= -1;//��� ���������� ��������� ����������� �������� 140 �������� �� -140 ��� ����� ��������� ������ ����������� ������� (�������� ���������������)
        transform.localScale = theScale; //������� �������������� ������������ ������������� ������� GameObjects
    }
    public void BoostEnemySpeed() //����� ��� �������� �������� ������
    {
        skeletonSpeed *= 1.1f;
        moushroomSpeed *= 1.1f;
        goblinSpeed *= 1.1f;
    }
    public void Block() // ������������� ����
    {
        playerIsAttack = Hero.Instance.isAttack;
        if (playerIsAttack == true && (Mathf.Abs(directionX)) < 1.5f && Mathf.Abs(directionY) < 2 && level > 1)
        {
            blockCooldown = 0;
            skeletonSpeed = 0;
            skeleton_block = true;
            anim.SetBool("Block", true);
        }
        if (blockCooldown > 0.4f || directionX > 2f)
        {
            skeletonSpeed = speedRecovery;
            skeleton_block = false;
            anim.SetBool("Block", false);
        }
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
    public void EnemyMovement() //����� � ������� ��������� ������ ���������� �� �������
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if ((Mathf.Abs(directionX) < 5 && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) && !skeleton_block && !isAttack || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !skeleton_block && !isAttack) //������� �� ������� ���� ��������� ��������� ��� ������� ����
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float playerFollowSpeed = Mathf.Sign(directionX) * Time.deltaTime;
            if (tag == "Skeleton")
            {
                playerFollowSpeed = Mathf.Sign(directionX) * skeletonSpeed * Time.deltaTime; //���������� �����������

            }
            if (tag == "Mushroom")
            {
                playerFollowSpeed = Mathf.Sign(directionX) * moushroomSpeed * Time.deltaTime; //���������� �����������
            }
            if (tag == "Slime")
            {
                playerFollowSpeed = Mathf.Sign(directionX) * slimeSpeed * Time.deltaTime; //���������� �����������
            }
            if (tag == "Death")
            {
                playerFollowSpeed = Mathf.Sign(directionX) * deathSpeed * Time.deltaTime;
            }
            pos.x += playerFollowSpeed; //���������� ������� �� ��� �
            transform.position = pos; //���������� �������
            movement = true;
        
            if (playerFollowSpeed < 0 && theScale.x > 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
            else if (playerFollowSpeed > 0 && theScale.x < 0) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
        }
        else
        {
            movement = false;
        }
    }
    public void SkeletonAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && !skeleton_block && timeSinceAttack > 1)
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
        else
        {
            isAttack = false;
        }
    }
    private void DieByFall() //����� ������� ������� ���� ��� ������� � ���������
    {
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity_Enemy>().enemyDead == false)//���� ���������� ������ �� ��� y ������ 10 � ���� �� �����, �� ���������� ����� ������ GetDamage
        {
            this.gameObject.GetComponent<Entity_Enemy>().TakeDamage(10);
        }
    }
    public void AnimState()//����� ��� ����������� ������ ��������
    {      
        if (movement == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if(movement == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0)
            this.gameObject.GetComponent<Animator>().SetInteger("State", 0);
        }
    }
    public void MoushroomJumpToPlayer() //������ � ������
    {
        if (level >= 1) //����������� ������������ �� 3 ������
        {
            jumpCooldown = 0;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (directionX > 0)
            {
                if (theScale.x < 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                {
                    Flip();
                }
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                if (theScale.x > 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                {
                    Flip();
                }
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
            }
        }
    }
    public void MushroomSpores() //������� ������ ���� ������� ������� ������
    {
        if (level >= 5)
        {
            sporesCooldown = 0; // ����� ������� ����
            Vector3 MoushroomScale = transform.localScale; //������ ��������� �������� ������� ��������
            transform.localScale = MoushroomScale; //������ ��������� �������� ������� ��������
            Vector3 sporeSpawnPosition = this.gameObject.transform.position; //������ ������� ��������
            if (MoushroomScale.x < 0) sporeSpawnPosition.x -= 0.8f; //����������� ���� ������ �������� � ����������� �� �������� �������
            if (MoushroomScale.x > 0) sporeSpawnPosition.x += 0.8f; //����������� ���� ������ �������� � ����������� �� �������� �������
            Spore.Instance.sporeDirection(sporeSpawnPosition); //�������� ���������� ��� ������ ������ ����
        }
    }
    public void MoushroomAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2)
        {
            MoushroomJumpToPlayer();
        }
        if ((Mathf.Abs(directionX)) < 0.8f && sporesCooldown > 10)
        {
            MushroomSpores();
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
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
        else
        {
            return;
        }
    }
    public void GoblinJumpToPlayer() //������ � ������
    {
        if (level >= 1) //����������� ������������ �� 3 ������
        {
            jumpCooldown = 0;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (directionX > 0)
            {
                if (theScale.x < 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                {
                    Flip();
                }
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                if (theScale.x > 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                {
                    Flip();
                }
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
            }
        }
    }
    public void GoblinJumpFromPlayer() // ������ �� ������
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
    public void GoblinBomb() //������ �����
    {
        if (level >= 5 && remainingBombs >= 1)
        {
            remainingBombs -= 1;
            bombCooldown = 0; // ����� ������� ����
            Vector3 goblinScale = transform.localScale; //������ ��������� �������� ������� �������
            transform.localScale = goblinScale; //������ ��������� �������� ������� �������
            Vector3 bombSpawnPosition = this.gameObject.transform.position; //������ ������� �������
            if (goblinScale.x < 0) bombSpawnPosition.x -= 1f; //����������� ����� ������ ������� � ����������� �� �������� �������
            if (goblinScale.x > 0) bombSpawnPosition.x += 1f;
            Bomb.Instance.bombDirection(bombSpawnPosition); //�������� ���������� ��� ������ �����
        }
        if (level < 5)
        {
            remainingBombs = 0;
        }
    }
    public void GoblinMovement() //����� � ������� ��������� ������ ���������� �� �������
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
            if (playerFollowSpeed < 0 && theScale.x > 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
            else if (playerFollowSpeed > 0 && theScale.x < 0) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
        }
        else
        {
            movement = false;
        }
    }
    public void GoblinAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 5f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs < 1)
        {
            GoblinJumpToPlayer();
        }
        if ((Mathf.Abs(directionX)) < 2f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs >= 1)
        {
            GoblinJumpFromPlayer();
        }
        if ((Mathf.Abs(directionX)) < 4.5 && bombCooldown > 3 && !jump && remainingBombs >= 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && bombCooldown > 3 && !jump && remainingBombs >= 1)
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float RunSpeed = Mathf.Sign(directionX) * goblinSpeed * Time.deltaTime; //���������� �����������
            if (theScale.x > 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
                GoblinBomb();
            }
            else if (theScale.x < 0) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
                GoblinBomb();
            }
        }
        if (jumpCooldown > 2.1f)
        {
            jump = false;
        }
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
        else
        {
            return;
        }
    }
    public void SlimeAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2)
        {
            MoushroomJumpToPlayer();
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            anim.SetTrigger("spin");
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else
        {
            return;
        }
    }
    public void DeathAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2)
        {
            
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            anim.SetTrigger("attack1");
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else
        {
            return;
        }
    }
}
