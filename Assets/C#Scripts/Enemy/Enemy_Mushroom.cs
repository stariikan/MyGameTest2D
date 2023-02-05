using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : MonoBehaviour //������������ ������ �������� (�� ���� ������ ������� ������������ � Entity ����� ��������� � � ����� �������)
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 2f;//�������� �������� �������
    [SerializeField] private float speedRecovery;//�������� �������� ������� 2 �������� ����� ��� �������������� �������� �� ��������� (����� ��������� ����� ��������� � �������� ����� ���������� ������� � ��� �� ��������))
    public int attackDamage = 7;

    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    public Transform groundcheck;// �������� ������������� �� ����� (������� �� ������� � ������)
    private Animator anim; //���������� ��������� ������� ���������� ������, ���������� = skelet_anim
    private float e_delayToIdle = 0.0f;
    public Transform wallChekPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� 
    private bool isGround; // ���������� �� ������ �� �����
    private bool playerFollow = false; //��� �� ���������� ������
    RaycastHit2D hit; //��� ����� �������� ���������� � ��� ������������ ������
    private float patrolCouldown = 0; //������� ����������� ��������������

    public float directionX;
    public float directionY;

    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;

    public static Enemy_Mushroom Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void OnCollisionEnter2D(Collision2D collision) //����������� �����, ����� ��� ������ ������������� � ������ ��������:
    {
            isGround = true;
    }
    private void OnCollisionExit2D(Collision2D collision) //����������� �����, ����� ��������������� ���� �������� �����������.
    {
            isGround = false;
    }
    private States State //�������� �����������, ���������� = State. �������� ��������� ����� ���� �������� ��� �������� ����� ��������� get � set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
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
        speed += 0.1f;
    }
    public void Patrol() //�������������� �����
    {
        float patroldirectionX = player.transform.position.x - transform.localPosition.x;
        if (patrolCouldown >= 6f)
        {
            patrolCouldown = 0;
        }

        if (playerFollow == false && (patrolCouldown <= 3f) && Mathf.Abs(patroldirectionX) > 0.5f)
        {
            Vector3 patrolPos = transform.position;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            patrolPos.x -= Mathf.Sign(patroldirectionX) * speed * Time.deltaTime;
            transform.position = patrolPos;
            Debug.Log(patrolPos.x);
            
            if (theScale.x > 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
        }
        if ((patrolCouldown > 3f) && (patrolCouldown <= 6f) && playerFollow == false && Mathf.Abs(patroldirectionX) > 0.5f)
        {
            Vector3 patrolPos = transform.position;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            patrolPos.x += Mathf.Sign(patroldirectionX) * speed * Time.deltaTime;
            transform.position = patrolPos;
            
            if (theScale.x < 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
        }
    }
    public void PlayerFollow() //����� � ������� ��������� ������ ���������� �� �������
    {
            directionX = player.transform.position.x - transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
            directionY = player.transform.position.y - transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y

        if ((Mathf.Abs(directionX) < 6 && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) || this.gameObject.GetComponent<Entity_Mushroom>().enemyTakeDamage == true && Mathf.Abs(directionX) > 0.9f) //������� �� ������� ���� ��������� ��������� ��� ������� ����
        {
                Vector3 pos = transform.position;
                Vector3 theScale = transform.localScale;
                transform.localScale = theScale;
                float playerFollowSpeed = Mathf.Sign(directionX) * speed * Time.deltaTime;
                pos.x += playerFollowSpeed;
                transform.position = pos;
                playerFollow = true;
            Debug.Log(directionX);

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
            playerFollow = false;
        }
    }
    public void Attack()
    {
        float playerHP = Hero.Instance.hp;
        float directionX = player.transform.position.x - transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        float directionY = player.transform.position.y - transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.05f && Mathf.Abs(directionY) < 1f)
        {
            //Damage Deal
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 2.0f)
            currentAttack = 1;
            Debug.Log("ATTACK!");
            anim.SetTrigger("attack" + currentAttack);
            // Reset timer
            timeSinceAttack = 0.0f;
        }
    }
    public void EnemyJump() //������ ���� ��������� ����� �����������
    {
        RaycastHit2D wall = Physics2D.Raycast(wallChekPoint.position, transform.localPosition, 0.04f, LayerMask.GetMask("Ground")); //�������� ��� ����� ������������ ����� ������ ����� ����� � ������� 0,04f ����� ����� ������ � ����� ����� 
        if (wall != false && isGround != false)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 20f), ForceMode2D.Impulse); // ������������� ���� �� Y ��� ������
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
        if (playerFollow == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if(playerFollow == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0)
                this.gameObject.GetComponent<Animator>().SetInteger("State", 0);
        }
    }
    public void groundCheckPosition()//�������� �� ��������, ����� ������ ���� �� ����
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 0.1f, LayerMask.GetMask("Ground"));//�� �������� Raycast ���� � ������� ������� groundcheck, �� 2 �������
                                                                                       //� ��������� ���������� �� ������ � ������ (groundLayers)
                                                                                       //PlayerFollow();

        if (hit.collider != true) //���� ������ groundcheck �� ���������� � ����� (�� ���� ��������)
        {
            speed = 0f;//�� ��������� ����� �� 0
        }
        else
        {
            speed = speedRecovery;//���� ������ ground check ����� ������������ � ����� (����� �����), �� ���������� ���������� ��������.
        }
    }
    private void Awake() //������� ������� ������ ��������� ��� ������ ����
    {
        player = GameObject.FindWithTag("Player"); //��� ��� ������ ���� ������ ������� ������ �� ���� Player � ����������� ��������� � ���������� ���������� player
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object)
                                          //� �������� �������� ������
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
                                         //� �������� �������� ������
    }
    private void Start()
    {
        speed = SaveSerial.Instance.enemySpeed;
        if (speed < 1f)
        {
            speed = 1f;
        }
        speedRecovery = speed;
        Instance = this;
    }
    void Update() //��� ���������� ���� �������� �������� ������ (������� ������ ������������ ����)
    {
        patrolCouldown += Time.deltaTime;

        if (this.gameObject.GetComponent<Entity_Mushroom>().currentHP > 0)
        {
            PlayerFollow(); //�������� �� �������
            DieByFall(); // ������ ��� �������
            AnimState(); //����������� ��������
            groundCheckPosition(); //�������� ��������
            EnemyJump(); //������ ����� �����������
            Attack();
            //Patrol();//��������������

        }

        else
        {
            return;
        }
    }
    

   
    

}
