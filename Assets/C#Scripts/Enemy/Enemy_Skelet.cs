using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skelet : MonoBehaviour //������������ ������ �������� (�� ���� ������ ������� ������������ � Entity ����� ��������� � � ����� �������)
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 2f;//�������� �������� �������
    [SerializeField] private float speed2 = 2f;//�������� �������� ������� 2 �������� ����� ��� �������������� �������� �� ��������� (����� ��������� ����� ��������� � �������� ����� ���������� ������� � ��� �� ��������))
    
    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    public LayerMask groundLayers;//��� ����� ���� ������� ����� ����������
    public Transform groundcheck;// �������� ������������� �� ����� (������� �� ������� � ������)

    private bool isMoving = false;
    private Animator anim; //���������� ��������� ������� ���������� ������, ���������� = skelet_anim

    private bool flipRight = true; //������� ������� �� �����, ��������� = ������, ����� ��� �������� ������� �� ����� ����� ��������
    RaycastHit2D hit; //��� ����� �������� ���������� � ��� ������������ ������

    public void Flip() //��� �� ������� ����� Flip ��� ������ �������� ������ ������ �����������
    {
        flipRight = !flipRight; //����� ����������� ����� Flip ���������� flipRight �������� �� false
        Vector3 theScale = transform.localScale; //��������� ������� �������
        theScale.x *= -1;//��� ���������� ��������� ����������� �������� 140 �������� �� -140 ��� ����� ��������� ������ ����������� ������� (�������� ���������������)
        transform.localScale = theScale; //������� �������������� ������������ ������������� ������� GameObjects
    }
    public void PlayerFollow() //����� � ������� ��������� ������ ���������� �� �������
    {
        if (player)
        {
            float direction = player.transform.position.x - transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �

            if (Mathf.Abs(direction) < 4) //���� ������ ������� ������ 4 ������
            {
                Vector3 pos = transform.position; //�� ���������� ��������� �������
                pos.x += Mathf.Sign(direction) * speed * Time.deltaTime;// ��� ������������� ����������� � �������� � ������� �������
                                                                        // (PS: MathF. Sign(Single) - ��� ����� ������ MathF, ������� ���������� ����� �����, ������������ ���� �����)
                transform.position = pos;//��� �������� ������� �� �� ��� ����������� � ������� ������ � �������
                isMoving = true;//���� ���� ����� ����������� ���������� isMoving ����������� �������
            }
            else
            {
                isMoving = false;//���� ���� ����� ��������� ����������� ���������� isMoving ����������� �� �������
            }
            if (direction > 0 && !flipRight) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
            else if (direction < 0 && flipRight) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
        }
                       
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player"); //��� ��� ������ ���� ������ ������� ������ �� ���� Player � ����������� ��������� � ���������� ���������� player
    }
    
       
    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 1f, groundLayers);//�� �������� Raycast ���� � ������� ������� groundcheck, �� 1 �������
                                                                                       //� ��������� ���������� �� ������ � ������ (groundLayers)
                                                                                       //PlayerFollow();
                       
        if (hit.collider != true) //���� ������ groundcheck �� ���������� � ����� (�� ���� ��������)
        {
            speed = 0f;//�� ��������� ����� �� 0
        }
        else
        {
            speed = speed2;//���� ������ ground check ����� ������������ � ����� (����� �����), �� ���������� ���������� ��������.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //��� ����� ������ ��� � �� ���� ����� �������� ���� ������
    {
        if (collision.gameObject == Hero.Instance.gameObject) //���� ������ ������������� ������ � ������ 
                                                              //(��� ���������� ������ �� ������ Hero � ������ ������� gameObject)
        {
            Hero.Instance.GetDamage(); //�� ������� Hero ���������� ��������� ����� ������� ������ ���������� hp -= 10.         
        }
    }

    void Update() //��� ���������� ���� �������� �������� ������ (������� ������ ������������ ����)
    {
       
        PlayerFollow();

        //������ ��������
        if (isMoving == false) State = States.idle;//���� �� ��������� ������ �������� ��������
        if (isMoving) State = States.run;//���� ���������� ������� ����������, �� State = run
        if (!hit.collider) State = States.jump; //� ���� �� �� �� ����� State = jump. ��� ��� ����� ����� �������� ��������

    }
    //���� � ��������� �������
    public enum States //����������� ����� ������ ���������, ������ �������� ��� � ��������� Unity
    {
        idle,
        run,
        jump
    }
    private States State //�������� �����������, ���������� = State. �������� ��������� ����� ���� �������� ��� �������� ����� ��������� get � set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
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
    }

}
