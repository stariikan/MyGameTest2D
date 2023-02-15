using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    public static Spore Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float lifetime; //������������ ����� �������
    private float playerHP; //���������� ����� ����� �� �� ���-�� ������

    private BoxCollider2D boxCollider; //��������� �����

    private int sporeDamage = 20;
    private float sporeCooldownDmg;
    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������

    private void Start() //�������� ����������� �� ������ ���� � 1 ���
    {
        player = GameObject.FindWithTag("PlayerCharacter");
        boxCollider = GetComponent<BoxCollider2D>(); // ����������� ���������� �� ���������� ���� ��������
        Instance = this;
        playerHP = Hero.Instance.hp;
    }

    private void Update()
    {
        lifetime += Time.deltaTime; //���������� ���������� lifetime ������ ��� +1
        sporeCooldownDmg += Time.deltaTime;//������� ����� ����
        playerHP = Hero.Instance.hp;
        SporeDmg();
        if (lifetime > 5) this.gameObject.SetActive(false);//����� ���������� ��������� 5, ��������� ����� ��������
        
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
