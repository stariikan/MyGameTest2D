using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainHP : MonoBehaviour
{
    
    public float direction;//���������� �����������
    private float playerHP; //���������� ����� ����� �� �� ���-�� ������

    private float drainHPDamage = 15f;

    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    private Animator anim; //���������� ��������� ������� ���������� ������
    private BoxCollider2D boxCollider; //��������� �����
    public static DrainHP Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private void Start()
    {
        Instance = this;
        player = GameObject.FindWithTag("PlayerCharacter");
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object) � �������� �������� ������
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object) � �������� �������� ������
        boxCollider = GetComponent<BoxCollider2D>();
        playerHP = Hero.Instance.curentHP;
    }
    private void Update()
    {
        playerHP = Hero.Instance.curentHP;
    }
    public void DrainHPDmg()
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if (Mathf.Abs(directionX) < 1f && Mathf.Abs(directionY) < 2f && playerHP > 0) 
        {
            Hero.Instance.GetDamage(drainHPDamage);
            GameObject[] deathObjects = GameObject.FindGameObjectsWithTag("Death");
            foreach (GameObject obj in deathObjects)
            {
                if (obj.name != "BossDeath")
                {
                    obj.GetComponent<Entity_Enemy>().BossDeathHeal(50);
                }
            }
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
