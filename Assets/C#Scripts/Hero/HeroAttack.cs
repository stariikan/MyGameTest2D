using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    [SerializeField] private float magicAttackCooldown;//������� ������� ������� (�����)
    [SerializeField] private float AttackCooldown;//������� ����� (���)
    [SerializeField] private Transform firePoint; //������� �� ������� ����� �������� �������
    [SerializeField] private GameObject[] magicProjectile; //������ ����� ��������
    [SerializeField] private GameObject meleeAttackArea; // ��� ������

    public static HeroAttack Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    public Animator Anim; //���������� ��� ������ � ���������
    public Transform attackPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� ������ (����� ��� ���������� ��� �����)

    private float cooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����
    private float MagicCooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����
    public int maxMP = 100;
    public int currentMP;
    
    private void Start()
    {
        maxMP = SaveSerial.Instance.playerMP;
        if (maxMP == 0)
        {
            maxMP = 100;
        }
        currentMP = maxMP;
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
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButtonDown(0)) && cooldownTimer > AttackCooldown)// ���� ������ �� ������ ������ ���� � ������� ������ > ��� �������� AttackCooldown, �� ����� ����������� ��� �����
        {
            Attack(); // ���������� �����
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)) && MagicCooldownTimer > magicAttackCooldown && currentMP >= 10) //���� ������ �� ����� ������ ���� � ������� ������ > ��� �������� MagicAttackCooldown, �� ����� ����������� �����
        {
            currentMP -= 10;
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
        Instance = this;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime; //����������� �� 1 ������� � cooldownTimer ����� ��� ��������� ��� ����������� ������ Attack.
        MagicCooldownTimer += Time.deltaTime; //����������� �� 1 ������� � MagicCooldownTimer ����� ��� ��������� ��� ����������� ������ magicAttack.
        attackControl();//����� � ������� �����
    }


}
