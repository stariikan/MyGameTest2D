using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHP = 100; //������������ ����� �������
    int currentHP;
    [SerializeField] private float AttackCooldown;//������� ����� (���)
    public float enemyAttackRange = 0.4f; //��������� ��� �����
    public int enemyAttackDamage = 7; // ���� �� ��� �����
    private float cooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown.
    public Transform enemyAttackPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� ������ (����� ��� ���������� ��� �����)
    public LayerMask playerLayers;
    public static Entity Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    
    private Animator anim;
    private void Start()
    {
        currentHP = maxHP;
        Instance = this;
        anim = GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
                                         //� �������� �������� ������
    }
    private void Attack()
    {
        //Anim.SetTrigger("Attack");//��� ��������������� �������� ����� ��� ���������� ������� Attack
        cooldownTimer = 0;
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(enemyAttackPoint.position, enemyAttackRange, playerLayers); //������� ���� �� ����� attackPoint c �������� ������� �� ���������
        foreach (Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<Entity>().TakeDamage(enemyAttackDamage);//��� �� �������� ������ � ������� ����� Entity � ���������� ������ ������� TakeDamage �
                                                                  //���� �������� � ��� � attackDamage
        }
    }
    public void Push() //����� ��� ������������ ���� �� ����� ��������� �����
    {
        if (transform.localScale.x < 0) //������� ����� ���������� � ���� ����������� ����
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(-50, 0, 0));
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(50, 0, 0));
        }
    }
    public void TakeDamage(int dmg) //����� ��� ��������� ������ ��� (int dmg) ��� �������� ����� ����� ������� ��� ������ ������ (�� ���� ���� ����� ����� ������� ����)
    {
        currentHP -= dmg;
        anim.SetTrigger("damage");//�������� ��������� ��������
        Push();
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public virtual void Die() //��������� ��������� ����� Die
    {
        //anim.SetTrigger("death");//�������� ������
        Debug.Log("Enemy Defeat");
        Destroy(this.gameObject);//���������� ���� ������� ������
    }
}
