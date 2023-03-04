using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float lifetime; //������������ ����� �������
    private float playerHP; //���������� ����� ����� �� �� ���-�� ������

    private float bombDamage = 40;
    private Animator anim;
    public Rigidbody2D rb; //���������� ����
    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������

    private void Start() //�������� ����������� �� ������ ���� � 1 ���
    {
        player = GameObject.FindWithTag("PlayerCharacter");
        Instance = this;
        playerHP = Hero.Instance.hp;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object) � �������� �������� ������
    }
    private void Update()
    {
        lifetime += Time.deltaTime; //���������� ���������� lifetime ������ ��� +1
        playerHP = Hero.Instance.hp;
        //if (lifetime > 3) this.gameObject.SetActive(false);//����� ���������� ��������� 5, ��������� ����� ��������
    }
    private void BombMovement() //����������� � ���� ������ ����� 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������ �� ��� �
        if (directionX > 0)
        {
            rb.AddForce(new Vector2(3, 1.5f), ForceMode2D.Impulse);
        }
        if (directionX < 0)
        {
            rb.AddForce(new Vector2(-3, 1.5f), ForceMode2D.Impulse);
        }
    }
    public void BombDestroy() //���������� ������� �����
    {
        this.gameObject.SetActive(false);
    }
    public void BombExplosion() //��������� �������� ������
    {
        anim.SetTrigger("explosion");
    }
    public void BombDmg() //��������� �����
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������ �� ��� �
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������ �� ��� y
        if ((Mathf.Abs(directionX) < 3f && Mathf.Abs(directionY) < 2f) && playerHP > 0)
        {
            Hero.Instance.GetDamage(bombDamage);
        }
    }
    public void bombDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������ �� ��� �
        lifetime = 0;
        this.gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        BombMovement();
    }
}
