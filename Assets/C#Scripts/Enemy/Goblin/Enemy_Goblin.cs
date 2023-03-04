using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Goblin : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3f;//�������� �������� �������
    [SerializeField] private float speedRecovery;//�������� �������� ������� 2 �������� ����� ��� �������������� �������� �� ��������� (����� ��������� ����� ��������� � �������� ����� ���������� ������� � ��� �� ��������))

    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    public Transform groundcheck;// �������� ������������� �� ����� (������� �� ������� � ������)
    private Animator anim; //���������� ��������� ������� ���������� ������, ���������� = skelet_anim
    private float e_delayToIdle = 0.0f;
    public Transform wallChekPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� 
    private bool isGround; // ���������� �� ������ �� �����
    private bool runFromPlayer = false; //��� �� ������� �� ������
    RaycastHit2D hit; //��� ����� �������� ���������� � ��� ������������ ������

    public float directionX; //���������� ��� ��������� ������� ����� ������� � ������
    public float directionY; //���������� ��� ��������� ������� ����� ������� � ������

    private int currentAttack = 0; //������� �� ����� �������
    private float timeSinceAttack = 0.0f;//����� � ������� ����� ����� ��� ����� �������� �����

    private float jumpCooldown; //������� �� ������ � ������
    private int level; //�������� ����� ������� �������� �����, ����� ��� ����������� ������������

    private bool bombAttack; //�������� ��������� �� ����
    private float bombCooldown = 4f; //������� ������ �����
    private int remainingBombs = 5; //����� 5 ����
    public static Enemy_Goblin Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private void Start()
    {
        player = GameObject.FindWithTag("PlayerCharacter"); //��� ��� ������ ���� ������ ������� ������ �� ���� Player � ����������� ��������� � ���������� ���������� player
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object) � �������� �������� ������
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object) � �������� �������� ������
        speed = SaveSerial.Instance.moushroomSpeed;
        if (speed < 3f)
        {
            speed = 3f;
        }
        speedRecovery = speed;
        Instance = this;
        level = LvLGeneration.Instance.Level;
    }
    void Update() //��� ���������� ���� �������� �������� ������ (������� ������ ������������ ����)
    {
        jumpCooldown += Time.deltaTime;
        bombCooldown += Time.deltaTime;
        timeSinceAttack += Time.deltaTime;
        if (this.gameObject.GetComponent<Entity_Goblin>().currentHP > 0)
        {
            RunFromPlayer(); //�������� �� ������
            DieByFall(); // ������ ��� �������
            AnimState(); //����������� ��������
            GroundCheckPosition(); //�������� ��������
            EnemyJump(); //������ ����� �����������
            Attack(); //�����
        }
        else
        {
            return;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) //����������� �����, ����� ��� ������ ������������� � ������ ��������:
    {
        isGround = true;
    }
    private void OnCollisionExit2D(Collision2D collision) //����������� �����, ����� ��������������� ���� �������� �����������.
    {
        isGround = false;
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
    public void BoostSpeed() //����� ��� �������� ��������
    {
        speed *= 1.1f;
    }
    public void JumpToPlayer() //������ � ������
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
    public void JumpFromPlayer() // ������ �� ������
    {
        if (Mathf.Abs(directionX) < 1f)
        {
            jumpCooldown = 0;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (directionX > 0)
            {
                if (theScale.x > 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                {
                    Flip();
                }
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                if (theScale.x < 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                {
                    Flip();
                }
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
        }
    }
    public void GoblinBomb() //������ �����
    {
        if (level >= 1 && remainingBombs >= 1)
        {
            remainingBombs -= 1;
            bombCooldown = 0; // ����� ������� ����
            Vector3 goblinScale = transform.localScale; //������ ��������� �������� ������� �������
            transform.localScale = goblinScale; //������ ��������� �������� ������� �������
            Vector3 bombSpawnPosition = this.gameObject.transform.position; //������ ������� �������
            if (goblinScale.x < 0) bombSpawnPosition.x -= 0.8f; //����������� ����� ������ ������� � ����������� �� �������� �������
            if (goblinScale.x > 0) bombSpawnPosition.x += 0.8f; 
            Bomb.Instance.bombDirection(bombSpawnPosition); //�������� ���������� ��� ������ �����
        }
    }
    public void RunFromPlayer() //����� � ������� ��������� ������ ���������� �� �������
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if ((Mathf.Abs(directionX) < 2f && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) || this.gameObject.GetComponent<Entity_Goblin>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f) //������� �� ������� ���� ��������� ��������� ��� ������� ����
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float playerFollowSpeed = Mathf.Sign(directionX) * speed * Time.deltaTime; //���������� �����������
            pos.x -= playerFollowSpeed; //���������� ������� �� ��� �
            transform.position = pos; //���������� �������
            runFromPlayer = true;

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
            runFromPlayer = false;
        }
    }
    public void Attack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 1 && Mathf.Abs(directionY) < 2 && remainingBombs < 1)
        {
            JumpToPlayer();
        }
        if ((Mathf.Abs(directionX)) < 7.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 1 && Mathf.Abs(directionY) < 2 && remainingBombs >= 1)
        {
            JumpFromPlayer();
        }
        if ((Mathf.Abs(directionX)) < 7f && bombCooldown > 4)
        {
            GoblinBomb();
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
    public void EnemyJump() //������ ���� ��������� ����� �����������
    {
        RaycastHit2D wall = Physics2D.Raycast(wallChekPoint.position, transform.localPosition, 0.04f, LayerMask.GetMask("Ground")); //�������� ��� ����� ������������ ����� ������ ����� ����� � ������� 0,04f ����� ����� ������ � ����� ����� 
        if (wall != false && isGround != false)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(rb.velocity.x, 1f), ForceMode2D.Impulse); // ������������� ���� �� Y ��� ������
        }
    }
    private void DieByFall() //����� ������� ������� ���� ��� ������� � ���������
    {
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity_Mushroom>().enemyDead == false)//���� ���������� ������ �� ��� y ������ 10 � ���� �� �����, �� ���������� ����� ������ GetDamage
        {
            this.gameObject.GetComponent<Entity_Mushroom>().TakeDamage(10);
        }
    }
    public void AnimState()//����� ��� ����������� ������ ��������
    {
        if (runFromPlayer == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if (runFromPlayer == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0)
                this.gameObject.GetComponent<Animator>().SetInteger("State", 0);
        }
    }
    //�������� �� ��������, ����� ������ ���� �� ���� �� �������� Raycast ���� � ������� ������� groundcheck, �� 2 ������� � ��������� ���������� �� ������ � ������ (groundLayers) PlayerFollow();
    public void GroundCheckPosition()
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.collider != true) //���� ������ groundcheck �� ���������� � ����� (�� ���� ��������)
        {
            speed = 0f;//�� ��������� ����� �� 0
        }
        else
        {
            speed = speedRecovery;//���� ������ ground check ����� ������������ � ����� (����� �����), �� ���������� ���������� ��������.
        }
    }
}
