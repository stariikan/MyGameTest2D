using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour
{
    public static Hero Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float speed = 4f; //��������
    public float jumpForce = 90f; //���� ������
    public float rollForce = 40f;

    public bool isGrounded = false; //���������� �� ������ �� �����, � ������ ������������� �� �� � ������ �������� ������� Collision2D 
    public bool isRoll = true; // �������� �����

    public bool flipRight; //������� ������� �� �����, ��������� = ������, ����� ��� �������� ������� �� ����� ����� ��������
    public Vector3 lossyScale; //���������� ������� �������

    public float maxHP;
    public float hp; //���������� ������
    public float stamina;

    public bool playerDead = false; //����� ����� ��� ���, ���� ����� ��� ���� ����� ��� ������ ������ ������ �������
    public int mageAttackDamage;

    public bool block = false;

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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
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
    public void CheckBlock()
    {
        block = HeroAttack.Instance.block;
        if (block == true)
        {
            speed = 2f;
        }
        else
        {
            speed = 4f;
        }
    }
    public void GetDamage(int dmg) //�� ������� ����� ����� GetDamage() 
    {
        if (block == false)
        {
            hp -= dmg;//�������� int 10 �� ���������� hp (�����).
            anim.SetTrigger("damage");
            Push();
        }
        if (block == true)
        {
            hp -= dmg * 0.15f;//�������� int �� ���������� hp (�����) � ��� �������� ����� ��������� ���� � 3 ����
            HeroAttack.Instance.DecreaseStamina(20);
            anim.SetTrigger("damage");
            Push();
        }
        if (hp <= 0) //���� ������ ������ 0
        {
            speed = 0;
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
    public void Flip() //��� �� ������� ����� Flip ��� ������ �������� ������ ������ �����������
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
        if (!isGrounded) State = States.jump; //���� �� �������� Space � �� �� ����� �� State = jump
    }//����� ��� �������� ������� ���������
    public void Jump()
    {
        if (isGrounded && stamina > 20)// ���� ���������� ������� � ���������� (GetKeyDown, � �� ������ GetKey) ������ Space � ���� isGrounded = true 
        {
            HeroAttack.Instance.DecreaseStamina(20);
            Vector2 jump = new Vector2(0, 1f);
            rb.velocity = jump * jumpForce;
            Debug.Log("JUMP!");
            isGrounded = false;            
        }
    }
    public void Roll()
    {
        if (isGrounded && isRoll && stamina > 15) //�������
        {
            HeroAttack.Instance.DecreaseStamina(15);
            if (!flipRight)
            {
                rb.AddForce(new Vector2(rollForce, 0), ForceMode2D.Impulse);
                isRoll = false;
                Debug.Log("LeftROLL");
            }
            if (flipRight)
            {
                rb.AddForce(new Vector2(rollForce * -1, 0), ForceMode2D.Impulse);
                isRoll = false;
                Debug.Log("RightRoll");
            }
        }
    }
    public void PlayerMovement()
    {
        float move = Input.GetAxis("Horizontal");//���������� Float ������-��� �������� 0.111..., ��� ������� ���� �� ����������� (������� � A D)
        float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical"); //����� ��� ������� ����� �����

        Vector2 movement = new Vector2(horizontal, 0); //, vertical//);
        rb.velocity = movement * speed;

        if (move > 0 && !flipRight) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
        {
            Flip();
        }
        else if (move < 0 && flipRight) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
        {
            Flip();
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && stamina > 20)// ���� ���������� ������� � ���������� (GetKeyDown, � �� ������ GetKey)
                                                          // ������ Space � ���� isGrounded = true 
        {
            HeroAttack.Instance.DecreaseStamina(20);
            Vector2 jump = new Vector2(0, 1f);
            rb.velocity = jump * jumpForce;
            isGrounded = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && isRoll && stamina > 15) //�������
        {
            HeroAttack.Instance.DecreaseStamina(15);
            if(!flipRight)
            {
                rb.AddForce(new Vector2(rollForce, 0), ForceMode2D.Impulse);
                isRoll = false;
            }
            if(flipRight)
            {
                rb.AddForce(new Vector2(rollForce * -1, 0), ForceMode2D.Impulse);
                isRoll = false;
            }
            
        }
        else
        {
            isRoll = true;
        }
    } 

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
        if (maxHP == 0)
        {
            maxHP = 100;
        }
        hp = maxHP;
        mageAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (mageAttackDamage == 0)
        {
            mageAttackDamage = 30;
        }

        stamina = SaveSerial.Instance.playerStamina;
        if (stamina == 0)
        {
            stamina = 100;
        }

        rb = GetComponent<Rigidbody2D>();

    }
    private void FixedUpdate()
    {
        stamina = HeroAttack.Instance.currentStamina; //�������� �������
        if (hp > 0)
        {
            PlayerMovement();//����� ��� �������� � �������� ������� ���������
            AnimState();//����� ��� �������� ��������� � ��������
            DieByFall();//����� ��� ������ �� �������
            CheckBlock(); //�������� �����
        }
        else
        {
            return;
        }
    }
}

