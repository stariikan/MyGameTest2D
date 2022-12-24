using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int trapDmg = 30;//���� �������
    private float cooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown.
    private float AttackCooldown = 1.5f;//������� �����
    private bool canAttack = false;
    private void OnCollisionEnter2D(Collision2D collision) //���� ���������� ��������������� ���
    {
        if (collision.gameObject == Hero.Instance.gameObject)//���� ������� ������������� ������ � ������ (��� ���������� ������ �� ������ Hero � ������ ������� gameObject)
        {
            canAttack = true;
        }
        
    }
    void OnCollisionExit2D(Collision2D collision) //����������, ����� ��������� ������� ������� ��������� ������������� � ����������� ����� ������� (������ 2D ������).
    {
        canAttack = false;
    }
    private void trapAttack()//����� ��� ����� �������
    {
        if (cooldownTimer > AttackCooldown & canAttack) //��� ����� ������� � �������, ����� ��� �� ������� ��������� �� 1 ���
        {
            cooldownTimer = 0;
            Hero.Instance.GetDamage(trapDmg); // ����� �������
        }
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime; //����������� ����� � �������
        trapAttack();
    }
}
