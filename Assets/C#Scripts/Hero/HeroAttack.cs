using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : Hero
{
    [SerializeField] private float magicAttackCooldown;//������� ������� ������� (�����)
    [SerializeField] private float AttackCooldown;//������� ����� (���)
    [SerializeField] private Transform firePoint; //������� �� ������� ����� �������� �������
    [SerializeField] private GameObject[] magicProjectile; //������ ����� ��������
    [SerializeField] private GameObject meleeAttackArea; // ��� ������

    public Animator Anim; //���������� ��� ������ � ���������
    public Transform attackPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� ������ (����� ��� ���������� ��� �����)

    private float cooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����
    private float MagicCooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����

    public int attackDamage = 20; // ���� �� ��� �����
    public float attackRange = 0.4f; //��������� ��� �����
    private bool swordHit = false; //���� �� ��������� ����� �� ����


    private void OnDrawGizmosSelected() //��������� ���������� ���� ������� ���������� � ������ Attack
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawSphere(attackPoint.position, attackRange); //���������� ���� (����� ����� � ��� Attack point, ������ ����� attackRange)
    }
    private void Attack()
    {
       Anim.SetTrigger("Attack");//��� ��������������� �������� ����� ��� ���������� ������� Attack
       cooldownTimer = 0;
       meleeAttackArea.transform.position = firePoint.position; //��� ������ ����� �� ����� ������ ��������� ������� � �������� �� ��������� ������� ����� �������� ��������� �� ������� � ��������� ��� � ����������� � ������� ���������� �����
       meleeAttackArea.GetComponent<MeleeWeapon>().meleeDirection(Mathf.Sign(transform.localScale.x));

    }
    private void magicAttack()
    {
        Anim.SetTrigger("magicAttack");//��� ��������������� �������� ����� ������ ��� ���������� ������� magicAttack
        MagicCooldownTimer = 0; //����� �������� ���������� ����� ��� ���� ����� ������ ������� ��� ����� ������� ��� ������� �� ������� � ���� �� ��������, �� ����� ����� ���������
        magicProjectile[FindMagicBall()].transform.position = firePoint.position; //��� ������ ����� �� ����� ������ ��������� ������� � �������� �� ��������� ������� ����� �������� ��������� �� ������� � ��������� ��� � ����������� � ������� ���������� �����
        magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private void attackControl()
    {
        if (Input.GetMouseButtonDown(0) && cooldownTimer > AttackCooldown)// ���� ������ �� ������ ������ ���� � ������� ������ > ��� �������� AttackCooldown, �� ����� ����������� ��� �����
        {
            Attack(); // ���������� �����
        }

        if (Input.GetMouseButtonDown(1) && MagicCooldownTimer > magicAttackCooldown) //���� ������ �� ����� ������ ���� � ������� ������ > ��� �������� MagicAttackCooldown, �� ����� ����������� �����
        {
            magicAttack(); // ���������� ��� �����
        }
    }
    private int FindMagicBall()// ����� ��� �������� �������� ����� �� 0 �� +1 ���� �� ������ �� ����������� �������
    {
        for (int i = 0; i < magicProjectile.Length; i++)
        {
            if (!magicProjectile[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private void Awake()
    {
        Anim = GetComponent<Animator>(); //������ � ���������
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime; //����������� �� 1 ������� � cooldownTimer ����� ��� ��������� ��� ����������� ������ Attack.
        MagicCooldownTimer += Time.deltaTime; //����������� �� 1 ������� � MagicCooldownTimer ����� ��� ��������� ��� ����������� ������ magicAttack.
        attackControl();//����� � ������� �����
    }


}
