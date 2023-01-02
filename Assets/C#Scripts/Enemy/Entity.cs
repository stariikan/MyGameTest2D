using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHP = 100; //������������ ����� �������
    public int currentHP;
    [SerializeField] private float AttackCooldown;//������� ����� (���)
    public float enemyAttackRange = 0.3f; //��������� ��� �����
    public int enemyAttackDamage = 7; // ���� �� ��� �����
    private float cooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown.
    public Transform enemyAttackPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� ������ (����� ��� ���������� ��� �����)
    public LayerMask playerLayers;
    public static Entity Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private Animator anim;
    public bool enemyDead = false;
    public int rewardForKillEnemy = 2;//������� �� ������ ��� ������
    private void Start()
    {
        maxHP = SaveSerial.Instance.enemyHP;
        if (maxHP == 0)
        {
            maxHP = 100;
        }
        currentHP = maxHP;
        enemyAttackDamage = SaveSerial.Instance.enemyDamage;
        if (enemyAttackDamage == 0)
        {
            enemyAttackDamage = 7;
        }
        Instance = this;
        anim = GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
                                         //� �������� �������� ������
    }
    private void Attack()
    {
        if (cooldownTimer > AttackCooldown)
        {
            //Anim.SetTrigger("Attack");//��� ��������������� �������� ����� ��� ���������� ������� Attack
            cooldownTimer = 0;
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(enemyAttackPoint.position, enemyAttackRange, playerLayers); //������� ���� �� ����� attackPoint c �������� ������� �� ���������
            foreach (Collider2D enemy in hitEnemys)
            {
                enemy.GetComponent<Hero>().GetDamage(enemyAttackDamage);//��� �� �������� ������ � ������� ����� Entity � ���������� ������ ������� TakeDamage �
                                                                        //���� �������� � ��� � attackDamage
            }
        }
    }
    public void BoostHP() //��� ��������� ��
    {
        maxHP += 10;
    }
    public void BoostAttackDamage() //��� ��������� ����
    {
        enemyAttackDamage += 3;
    }
    public void BoostReward() //��� ����������� ������� �� ��������
    {
        rewardForKillEnemy += 2;
    }
    public void Push() //����� ��� ������������ ���� �� ����� ��������� �����
    {
        if (transform.localScale.x < 0) //������� ����� ���������� � ���� ����������� ����
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(-100, 0, 0));
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(100, 0, 0));
        }
    }
    public void TakeDamage(int dmg) //����� ��� ��������� ������ ��� (int dmg) ��� �������� ����� ����� ������� ��� ������ ������ (�� ���� ���� ����� ����� ������� ����)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("damage");//�������� ��������� ��������
            currentHP -= dmg;
            Push();
            Debug.Log(currentHP + " " + gameObject.name);
        }
        else
        {
            return;
        }

        if (currentHP <= 0)
        {
            LvLGeneration.Instance.PlusCoin(rewardForKillEnemy); //����� ������ ��� ���������� �����
            anim.SetTrigger("death");//�������� ������
            enemyDead = true;
            Debug.Log("Enemy Defeat -> " + gameObject.name);
        }
    }
    public virtual void Die() //����� ������� ���� ������� ������, ���������� ����� �������� ����� ����� ���������� �������� ������
    {
        Destroy(this.gameObject); ;//���������� ���� ������� ������
    }
    private void Update()
    {
        if (currentHP > 0)
        {
            cooldownTimer += Time.deltaTime; // ����������� ������� � ������� �������
            Attack();
        }
        else
        {
            return;
        }
        
            
    }
}
