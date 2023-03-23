using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    public static Spore Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float lifetime; //������������ ����� �������
    private float playerHP; //���������� ����� ����� �� �� ���-�� ������

    private CircleCollider2D circleCollider; //��������� �����

    private float sporeDamage = 20;
    private float sporeCooldownDmg;
    private float sporeSpeed = 1f;
    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������

    private void Start() //�������� ����������� �� ������ ���� � 1 ���
    {
        player = GameObject.FindWithTag("PlayerCharacter");
        circleCollider = GetComponent<CircleCollider2D>(); // ����������� ���������� �� ���������� ���� ��������
        Instance = this;
        playerHP = Hero.Instance.curentHP;
    }
    private void Update()
    {
        lifetime += Time.deltaTime; //���������� ���������� lifetime ������ ��� +1
        sporeCooldownDmg += Time.deltaTime;//������� ����� ����
        playerHP = Hero.Instance.curentHP;
        SporeDmg();
        SporeMovement();
        if (lifetime > 5) Destroy(this.gameObject);//���������� ���� ������� ������

    }
    private void SporeMovement()
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������ �� ��� �
        //int level = LvLGeneration.Instance.Level;
        if (playerHP > 0)
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float playerFollowSpeed = Mathf.Sign(directionX) * sporeSpeed * Time.deltaTime; //���������� �����������
            pos.x += playerFollowSpeed; //���������� ������� �� ��� �
            transform.position = pos; //���������� �������
        }
    }
    private void SporeDmg()
    {
       float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������ �� ��� �
       float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������ �� ��� y
        if ((Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 2f) && sporeCooldownDmg > 1 && playerHP > 0)
       {
            sporeCooldownDmg = 0;
            Hero.Instance.GetDamage(sporeDamage);
       }
    }
    public void sporeDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        lifetime = 0;
        this.gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
    }
}
