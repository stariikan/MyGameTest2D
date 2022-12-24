using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : Hero
{
    [SerializeField] private float magicAttackCooldown;//������� ������� ������� (�����)
    [SerializeField] private float AttackCooldown;//������� ����� (���)
    [SerializeField] private Transform firePoint; //������� �� ������� ����� �������� �������
    [SerializeField] private GameObject[] magicProjectile; //������ ����� ��������

    
    private float MagicCooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����
    public int magicAttackDamage = 30;
    public float magicAttackRange = 1f;

    private Animator Anim; //���������� ��� ������ � ���������
    public Transform attackPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� ������ (����� ��� ���������� ��� �����)
    public Transform magicAttackPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� ����� (����� ��� ���������� ��� �����)

    private float cooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����
    public float attackRange = 0.4f; //��������� ��� �����
    public int attackDamage = 20; // ���� �� ��� �����
    private bool magicHit = true; //���� �� ��������� ��� ��������
    private bool swordHit = true; //���� �� ��������� ����� �� ����
    public LayerMask enemyLayers; //����� ���� � �������� ����������� �����

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
       
    }
    private void magicAttack()
    {
        Anim.SetTrigger("magicAttack");//��� ��������������� �������� ����� ������ ��� ���������� ������� magicAttack
        MagicCooldownTimer = 0; //����� �������� ���������� ����� ��� ���� ����� ������ ������� ��� ����� ������� ��� ������� �� ������� � ���� �� ��������, �� ����� ����� ���������
        magicProjectile[FindMagicBall()].transform.position = firePoint.position; //��� ������ ����� �� ����� ������ ��������� ������� � �������� �� ��������� ������� ����� �������� ��������� �� ������� � ��������� ��� � ����������� � ������� ���������� �����
        magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private void checkMagicball() //�������� ��������� ��� ��������
    {
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(magicAttackPoint.position, attackRange, enemyLayers); //������� ���� �� ����� attackPoint c �������� ������� �� ���������
        foreach (Collider2D enemy in hitEnemys)
        {
            magicHit = true;// ���� ��������� �� ����
            //Debug.Log(enemy.name);//����������� ��� �������� ���� �������� �� ��� � ���� �� ������
        }
    }
    private void checkSword() //�������� ��������� �����
    {
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, magicAttackRange, enemyLayers); //������� ���� �� ����� attackPoint c �������� ������� �� ���������
        foreach (Collider2D enemy in hitEnemys)
        {
            swordHit = true;
            //Debug.Log(enemy.name);
        }
    }
    private void magicDamage() // ��������� ����� ������
    {
        if (magicHit == true )
        {
            Entity.Instance.TakeDamage(magicAttackDamage);//��� �� �������� ������ � ������� ����� Entity � ���������� ������ ������� TakeDamage � ���� �������� � ��� � attackDamage
            magicHit = false;
        }
    }
    private void swordDamage() // ��������� ����� �����
    {
        if (swordHit == true)
        {
            Entity.Instance.TakeDamage(magicAttackDamage);//��� �� �������� ������ � ������� ����� Entity � ���������� ������ ������� TakeDamage � ���� �������� � ��� � attackDamage
        }
    }
    private void attackControl()
    {
        if (Input.GetMouseButtonDown(0) && cooldownTimer > AttackCooldown)// ���� ������ �� ������ ������ ���� � ������� ������ > ��� �������� AttackCooldown, �� ����� ����������� ��� �����
        {
            Attack(); // ���������� �����
            swordDamage();// �� ������� ����� �� ����
        }

        if (Input.GetMouseButtonDown(1) && MagicCooldownTimer > magicAttackCooldown) //���� ������ �� ����� ������ ���� � ������� ������ > ��� �������� MagicAttackCooldown, �� ����� ����������� �����
        {
            magicAttack(); // ���������� ��� �����
            magicDamage(); // �� ��������� ����� �� �����
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
        checkMagicball();//�������� ��������� ��� �������� �� ����
        checkSword();//�������� ��������� ����� �� ����
        attackControl();//����� � ������� �����
    }


}
