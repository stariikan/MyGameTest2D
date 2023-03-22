using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    private float playerHP; //���������� ����� ����� �� �� ���-�� ������

    private float bombDamage = 40;
    private string enemyName;
    private GameObject enemy;
    private Animator anim;
    public Rigidbody2D rb; //���������� ����
    public GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������

    private void Start() //�������� ����������� �� ������ ���� � 1 ���
    {
        Instance = this;
    }
    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object) � �������� �������� ������
    }
    private void Update()
    {
        playerHP = Hero.Instance.curentHP;
    }
    private void BombMovement() //����������� � ���� ������ ����� 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ����� �� ��� �
        if (directionX > 0) rb.AddForce(new Vector2(2.7f, 0.5f), ForceMode2D.Impulse);
        if (directionX < 0) rb.AddForce(new Vector2(-2.7f, 0.5f), ForceMode2D.Impulse);
 
    }
    public void BombDestroy() //���������� ������� �����
    {
        Destroy(this.gameObject);//���������� ���� ������� ������
    }
    public void BombExplosion() //��������� �������� ������
    {
        rb.velocity = Vector3.zero; //��� ��������� �������
        anim.SetTrigger("explosion");
    }
    public void BombDmg() //��������� �����
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ����� �� ��� �
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ����� �� ��� y
        float enemyDirectionX = enemy.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ����� �� ��� � - ������� ����� �� ��� � 
        if ((Mathf.Abs(directionX) < 2.0f && Mathf.Abs(directionY) < 2f) && playerHP > 0)
        {
            Hero.Instance.GetDamage(bombDamage);
        }
        if (Mathf.Abs(enemyDirectionX) < 2f) enemy.GetComponent<Entity_Enemy>().TakeDamage(bombDamage/1.5f);
    }
    public void PushFromPlayer() // ������ �� ������
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ����� �� ��� �
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ����� �� ��� y
        if (Mathf.Abs(directionX) < 1f)
        {
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (theScale.x > 0) rb.AddForce(new Vector2(+2.7f, 0.5f), ForceMode2D.Impulse);
            if (theScale.x < 0) rb.AddForce(new Vector2(-2.7f, 0.5f), ForceMode2D.Impulse);
        }
    }
    public void bombDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������ �� ��� �
        this.gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        BombMovement();
    }
    public void GetEnemyName(string name)
    {
        enemyName = name;
        enemy = GameObject.Find(name);
    }
}
