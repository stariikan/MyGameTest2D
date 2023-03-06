using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainHP : MonoBehaviour
{
    
    public float direction;//���������� �����������
    private float playerHP; //���������� ����� ����� �� �� ���-�� ������

    private float drainHPDamage = 15;

    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    private Animator anim; //���������� ��������� ������� ���������� ������
    public static DrainHP Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private void Start()
    {
        Instance = this;
        player = GameObject.FindWithTag("PlayerCharacter");
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object) � �������� �������� ������
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object) � �������� �������� ������
        playerHP = Hero.Instance.hp;
    }
    private void Update()
    {
        playerHP = Hero.Instance.hp;
    }
    public void DrainHPDmg()
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������ �� ��� �
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������ �� ��� y
        if ((Mathf.Abs(directionX) < 1.2f && Mathf.Abs(directionY) < 2f) && playerHP > 0)
        {
            Hero.Instance.GetDamage(drainHPDamage);
            Entity_Enemy.Instance.BossDeathHeal();
        }
    }
    public void DrainHPDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        this.gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        anim.SetTrigger("drain_hp");
    }
    public void DrainHPOff()
    {
        this.gameObject.SetActive(false);
    }
}
