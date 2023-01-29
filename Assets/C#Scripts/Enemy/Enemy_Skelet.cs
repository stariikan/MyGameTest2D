using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skelet : MonoBehaviour //������������ ������ �������� (�� ���� ������ ������� ������������ � Entity ����� ��������� � � ����� �������)
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 1f;//�������� �������� �������
    [SerializeField] private float speedRecovery;//�������� �������� ������� 2 �������� ����� ��� �������������� �������� �� ��������� (����� ��������� ����� ��������� � �������� ����� ���������� ������� � ��� �� ��������))
    public int attackDamage = 7;

    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    public Transform groundcheck;// �������� ������������� �� ����� (������� �� ������� � ������)

    private bool isMoving = false;
    private Animator anim; //���������� ��������� ������� ���������� ������, ���������� = skelet_anim

    public Transform wallChekPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� 

    private bool isGround; // ���������� �� ������ �� �����

    private bool playerFollow = false; //��� �� ���������� ������
    private float patrolCouldown = 0; //������� ����������� ��������������
    RaycastHit2D hit; //��� ����� �������� ���������� � ��� ������������ ������

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
    private States State //�������� �����������, ���������� = State. �������� ��������� ����� ���� �������� ��� �������� ����� ��������� get � set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
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
        isMoving = true;
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
            float directionX = player.transform.position.x - transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
            float directionY = player.transform.position.y - transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y

        if (Mathf.Abs(directionX) < 6 && Mathf.Abs(directionX) > 0.5f && Mathf.Abs(directionY) < 2)
        {
                Vector3 pos = transform.position;
                Vector3 theScale = transform.localScale;
                transform.localScale = theScale;
                float playerFollowSpeed = Mathf.Sign(directionX) * speed * Time.deltaTime;
                pos.x += playerFollowSpeed;
                transform.position = pos;
                isMoving = true;//���� ���� ����� ����������� ���������� isMoving ����������� �������
                playerFollow = true;
                
                if (playerFollowSpeed < 0 && theScale.x < 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
                {
                    Flip();
                }
                else if (playerFollowSpeed > 0 && theScale.x > 0) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
                {
                    Flip();
                }
        }
        else
        {
            playerFollow = false;
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
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity>().enemyDead == false)//���� ���������� ������ �� ��� y ������ 10 � ���� �� �����, �� ���������� ����� ������ GetDamage
        {
            this.gameObject.GetComponent<Entity>().TakeDamage(10);
        }
    }
    public void AnimState()//����� ��� ����������� ������ ��������
    {
        if (isMoving != true) State = States.idle;//���� �� ��������� ������ �������� ��������
        if (isMoving == true) State = States.run;//���� ���������� ������� ����������, �� State = run
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
        rb = GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object)
                                          //� �������� �������� ������
        anim = GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
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
    }
    void Update() //��� ���������� ���� �������� �������� ������ (������� ������ ������������ ����)
    {
        patrolCouldown += Time.deltaTime;
        if (this.gameObject.GetComponent<Entity>().currentHP > 0)
        {
            PlayerFollow(); //�������� �� �������
            DieByFall(); // ������ ��� �������
            AnimState(); //����������� ��������
            groundCheckPosition(); //�������� ��������
            EnemyJump(); //������ ����� �����������
          //  Patrol();//��������������
        }
        else
        {
            return;
        }
    }
    

   
    

}
