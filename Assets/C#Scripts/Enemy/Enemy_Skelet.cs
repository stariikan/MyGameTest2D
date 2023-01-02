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
    //public LayerMask groundLayers;//��� ����� ���� ������� ����� ����������
    public Transform groundcheck;// �������� ������������� �� ����� (������� �� ������� � ������)

    private bool isMoving = false;
    private Animator anim; //���������� ��������� ������� ���������� ������, ���������� = skelet_anim

    private bool flipRight; //������� ������� �� �����, ��������� = ������, ����� ��� �������� ������� �� ����� ����� ��������
    RaycastHit2D hit; //��� ����� �������� ���������� � ��� ������������ ������

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
        flipRight = !flipRight; //����� ����������� ����� Flip ���������� flipRight �������� �� false
        Vector3 theScale = transform.localScale; //��������� ������� �������
        theScale.x *= -1;//��� ���������� ��������� ����������� �������� 140 �������� �� -140 ��� ����� ��������� ������ ����������� ������� (�������� ���������������)
        transform.localScale = theScale; //������� �������������� ������������ ������������� ������� GameObjects
    }
    public void BoostSpeed()
    {
        speed += 0.1f;
    }
    public void PlayerFollow() //����� � ������� ��������� ������ ���������� �� �������
    {
        if (player)
        {
            float directionX = player.transform.position.x - transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
            float directionY = player.transform.position.y - transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �

            if (Mathf.Abs(directionX) < 4 && Mathf.Abs(directionX) > 0.4f && Mathf.Abs(directionY) < 2) //���� ������ ������� ������ 4 ������ �� � � 2 ������ �� y
            {
                Vector3 pos = transform.position; //�� ���������� ��������� �������
                pos.x += Mathf.Sign(directionX) * speed * Time.deltaTime;// ��� ������������� ����������� � �������� � ������� �������
                                                                        // (PS: MathF. Sign(Single) - ��� ����� ������ MathF, ������� ���������� ����� �����, ������������ ���� �����)
                transform.position = pos;//��� �������� ������� �� �� ��� ����������� � ������� ������ � �������
                isMoving = true;//���� ���� ����� ����������� ���������� isMoving ����������� �������
            }
            else
            {
                isMoving = false;//���� ���� ����� ��������� ����������� ���������� isMoving ����������� �� �������
            }
            if (directionX < 0 && flipRight) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
            else if (directionX > 0 && !flipRight) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
        }
                       
    }
    private void DieByFall() //����� ������� ������� ���� ��� ������� � ���������
    {
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity>().enemyDead == false)//���� ���������� ������ �� ��� y ������ 10 � ���� �� �����, �� ���������� ����� ������ GetDamage
        {
            Entity.Instance.TakeDamage(10);
        }
    }
    public void AnimState()//����� ��� ����������� ������ ��������
    {
        if (isMoving == false) State = States.idle;//���� �� ��������� ������ �������� ��������
        if (isMoving) State = States.run;//���� ���������� ������� ����������, �� State = run
    }
    public void groundCheckPosition()//�������� �� ��������, ����� ������ ���� �� ����
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 2f, Physics.DefaultRaycastLayers);//�� �������� Raycast ���� � ������� ������� groundcheck, �� 1 �������
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
        flipRight = true;
    }
    private void Start()
    {
        speed = SaveSerial.Instance.enemySpeed;
        if (speed == 0f)
        {
            speed = 1f;
        }
        speedRecovery = speed;
    }
    void Update() //��� ���������� ���� �������� �������� ������ (������� ������ ������������ ����)
    {
        if (this.gameObject.GetComponent<Entity>().currentHP > 0)
        {
            PlayerFollow();
            DieByFall();
            AnimState();
            groundCheckPosition();
        }
        else
        {
            return;
        }
    }
    

   
    

}
