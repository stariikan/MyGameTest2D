using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour
{
    public static Hero Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public int movement_scalar = 60; //����� ��� �������� 
    public float maxSpeed = 3f; //������������ ��������
    private bool flipRight; //������� ������� �� �����, ��������� = ������, ����� ��� �������� ������� �� ����� ����� ��������
    public Vector3 lossyScale; //���������� ������� �������
    public bool isGrounded = false; //���������� �� ������ �� �����, � ������ ������������� �� �� � ������ �������� ������� Collision2D 
    public float gravityScale = 10; //���� ���������� ��� ��� ���� ��� ���� ������
    public float fallingGravityScale = 40; //���� ���������� ��� ������� ��� ���� ��� ������� ������� ������ ����� ����
    public int maxHP = 100;
    public int hp = 100; //���������� ������
    public bool playerDead = false; //����� ����� ��� ���, ���� ����� ��� ���� ����� ��� ������ ������ ������ �������
    public int mageAttackDamage = 30;

    private Rigidbody2D rb; //���� � ���������� ���������� � �������� ����������� ������, ���������� = rb
    private Animator anim; //���������� ��������� ������� ���������� ������, ���������� = anim
    private States State //�������� �����������, ���������� = State. �������� ��������� ����� ���� �������� ��� �������� ����� ��������� get � set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    public enum States //����������� ����� ������ ���������, ������ �������� ��� � ��������� Unity
    {
        idle,
        run,
        jump
    }
    private void OnCollisionEnter2D(Collision2D collision) //OnCollisionEnter ����������, ����� ���� ��������/���� �������� �������� ������� ����/����������.
                                                           //� ������� �� OnTriggerEnter, OnCollisionEnter ���������� ����� Collision, � �� Collider.
                                                           //����� Collision �������� ����������, ��������, � ������ �������� � �������� �����.
    {
        isGrounded = true; //���� �������� �������� ������� ����, ��������� ��� �� �� �����
    }
    public void Push() //����� ��� ������������ ���� �� ����� ��������� �����
    {
        if (transform.lossyScale.x < 0) //������� � ���������� � ����� ������� �������� �� � ������ 
        {
            rb.AddForce(new Vector2(2.5f, 2.5f), ForceMode2D.Impulse);//������� ��� ������ ��� ���� ����������� ����� 1 ���
        }
        else
        {
            rb.AddForce(new Vector2(-2.5f, 2.5f), ForceMode2D.Impulse);//������� ��� ������ ��� ���� ����������� ����� 1 ���
        }
    }
    public void GetDamage(int dmg) //�� ������� ����� ����� GetDamage() 
                            //����� ������������ �������� � ��� � 
    {
        hp -= dmg;//�������� int 10 �� ���������� hp (�����).
        anim.SetTrigger("damage");
        Push();
        if (hp <= 0) //���� ������ ������ 0
        {
            maxSpeed = 0;
            anim.SetTrigger("death");
            playerDead = true;
        }
    }
    private void Deactivate() //����������� ������ ����� ���������� �������� ������ (��������� ����� � ��������� ����������� ���� �����
    {
        gameObject.SetActive(false);
    }
    public void Hero_hp() //����� ������� ������ �������� �������� ���������� HP, ����� ��� ��� ��� �������� ����� ����� � ������ � ��������� ������
    {
        Debug.Log(hp);
    }
    private void Flip() //��� �� ������� ����� Flip ��� ������ �������� ������ ������ �����������
    {
        flipRight = !flipRight; //����� ����������� ����� Flip ���������� flipRight �������� �� false
        Vector3 theScale = transform.localScale; //��������� ������� �������
        theScale.x *= -1;//��� ���������� ��������� ����������� �������� 140 �������� �� -140 ��� ����� ��������� ������ ����������� ������� (�������� ���������������)
        transform.localScale = theScale; //������� �������������� ������������ ������������� ������� GameObjects
    }
    private void DieByFall() //����� ������� ������� ���� ��� ������� � ���������
    {
        if (rb.transform.position.y < -100)//���� ���������� ������ �� ��� y ������ 10, �� ���������� ����� ������ GetDamage
        {
            GetDamage(100);
        }
    }
    public void AnimState()
    {
        if (!Input.GetButton("Horizontal")) State = States.idle;//���� �� �� ����� State = idle
        if (Input.GetButton("Horizontal")) State = States.run;//���� �� �������� �� ������ (������� ��� A D) �� State = run
        if (Input.GetKeyDown(KeyCode.Space)) State = States.jump; //���� �� �������� Space � �� �� ����� �� State = jump
    }//����� ��� �������� ������� ���������
    public void PlayerMovement()
    {
        float move = Input.GetAxis("Horizontal");//���������� Float ������-��� �������� 0.111..., ��� ������� ���� �� ����������� (������� � A D)
        if (rb.velocity.magnitude < maxSpeed)
        {
            Vector2 movement = new Vector2(move, 0);
            rb.AddForce(new Vector2 (movement_scalar * move, 0), ForceMode2D.Force);//��� ������ ��� ������� ��������� Rigidbody2D
        }
        
                                                                                                                    //� ������ game.Object � ��������� new Vector2
                                                                                                                    //��������� ������� game.Object ���������� (*)
                                                                                                                    //������������ �������� ������� �� ������� � ����������
                                                                                                                    //�� ��� x
                                                                                                                    //velocity = ������� ����� ��������� �������, �� ����� ���� ������������ ��� ��������� ������.
                                                                                                                    //����� ����� �������� � X, Y � Z, ��������� �����������.
        if (move > 0 && !flipRight) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
        {
            Flip();
        }
        else if (move < 0 && flipRight) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
        {
            Flip();
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)// ���� ���������� ������� � ���������� (GetKeyDown, � �� ������ GetKey)
                                                          // ������ Space � ���� isGrounded = true 
        {
            isGrounded = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 20f), ForceMode2D.Impulse); //ForceMode2D.Impulse  It may seem like your object is pushed once in Y axis and it will fall down automatically due to gravity.

        }
        if (rb.velocity.y >= 0) //���� �������� ���� �� ��� Y ������ ��� ����� 0, ��
        {
            rb.gravityScale = gravityScale; //��� ��� ��� ���������� ��� �� ��������� ���������� � ���������� ����� �������� ���� ������
        }
        else if (rb.velocity.y < 0) //���� �������� ��������� �� ��� Y ������ 0 ��
        {
            rb.gravityScale = fallingGravityScale; //���������� �������� ����������� ������ � ��� � �����������
                                                   //�� ����� ������� �� ����� � ���������� fallingGravityScale
        }
    } //����� ��� �������� ������� ���������

    void Awake() //Awake ������������ ��� ������������� ����� ���������� ��� �������� ��������� ����� ������� ����.
                 //Awake ���������� ������ ���� ��� �� ��� ����� ������������� ���������� ��������.
                 //����� Awake ���������� ����� ������������� ���� ��������, ������� ����� ��������� ���������� � ������ ��������
                 //��� ����������� ��, ���������, ��������, GameObject.
    {
        rb = GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object)
                                          //� �������� �������� ������
        anim = GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
                                         //� �������� �������� ������
        Instance = this; //'this' - ��� �������� �����, ������������ �����, � ������� ����������� ���.
                         //��������� ��� ��������, ��� ������� �� ���������, �� ������ ��� ����� ����������� this. transform. position and transform.
        flipRight = true;
    }
    private void Start()
    {
        SaveSerial.Instance.LoadGame();
        maxHP = SaveSerial.Instance.playerHP;
        mageAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (maxHP == 0)
        {
            maxHP = 100;
        }
        hp = maxHP;
        if (mageAttackDamage == 0)
        {
            mageAttackDamage = 30;
        }
    }
    private void FixedUpdate()
    {
        if (hp > 0)
        {
            PlayerMovement();//����� ��� �������� � �������� ������� ���������
            AnimState();//����� ��� �������� ��������� � ��������
            DieByFall();//����� ��� ������ �� �������
        }
        else
        {
            return;
        }
    }
}

