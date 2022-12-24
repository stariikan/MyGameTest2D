using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static Hero Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float maxSpeed = 10f; //������������ ��������
    private bool flipRight = true; //������� ������� �� �����, ��������� = ������, ����� ��� �������� ������� �� ����� ����� ��������
    public bool isGrounded = true; //���������� �� ������ �� �����, � ������ ������������� �� �� � ������ �������� ������� Collision2D 
    public float gravityScale = 10; //���� ���������� ��� ��� ���� ��� ���� ������
    public float fallingGravityScale = 40; //���� ���������� ��� ������� ��� ���� ��� ������� ������� ������ ����� ����
    [SerializeField] public int hp = 100; //���������� ������
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
        if (transform.localScale.x < 0) //������� ����� ���������� � ���� ����������� ����
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(50, 0, 0));
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(-50, 0, 0));
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
            anim.SetTrigger("death");
            maxSpeed = 0;
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
        if (rb.transform.position.y < -10)//���� ���������� ������ �� ��� y ������ 10, �� ���������� ����� ������ GetDamage
        {
            GetDamage(100);
        }
    }
    public void AnimState()
    {
        if (isGrounded) State = States.idle;//���� �� �� ����� State = idle
        if (Input.GetButton("Horizontal")) State = States.run;//���� �� �������� �� ������ (������� ��� A D) �� State = run
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) State = States.jump; //���� �� �������� Space � �� �� ����� �� State = jump
        if (!isGrounded) State = States.jump; //� ���� �� �� �� ����� State = jump. ��� ��� ����� ����� �������� ��������
    }//����� ��� �������� ������� ���������
    private void PlayerMovement()
    {
        float move = Input.GetAxis("Horizontal");//���������� Float ������-��� �������� 0.111..., ��� ������� ���� �� ����������� (������� � A D)
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);//��� ������ ��� ������� ��������� Rigidbody2D
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)// ���� ���������� ������� � ���������� (GetKeyDown, � �� ������ GetKey)
                                                          // ������ Space � ���� isGrounded = true 
        {
            isGrounded = false; // �� isGrounded �������� �� false 
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 1000, 0)); //������� ��������� Rigidbody2D � game.Object
                                                                           //� ����������� ������ �� (new Vetor3) � ����������� ����� Y
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
    }
    void Update() //Update = ���������� ������� ������ ������ ����.
    {
        PlayerMovement();//����� ��� �������� � �������� ������� ���������
        AnimState();//����� ��� �������� ��������� � ��������
        DieByFall();//����� ��� ������ �� �������
    }

}

