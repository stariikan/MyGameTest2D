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

    private float cooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����
    private float MagicCooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����
    public float maxMP = 100;
    public float currentMP;
    public float stamina = 100;
    public float currentStamina;
    public float staminaSpeedRecovery = 5f;

    public bool block = false;

    private Camera mainCamera;

    private void Start()
    {
        maxMP = SaveSerial.Instance.playerMP;
        if (maxMP == 0)
        {
            maxMP = 100;
        }
        currentMP = maxMP;

        stamina = SaveSerial.Instance.playerStamina;
        if (stamina == 0)
        {
            stamina = 100;
        }
        currentStamina = stamina;

        mainCamera = Camera.main;
    }
    private void staminaRecovery()
    {
        if (currentStamina < stamina)
        {
            currentStamina += Time.deltaTime * staminaSpeedRecovery;
        }
        if (currentStamina < 0)
        {
            currentStamina = 2;
        }
    }
    public void Block()
    {
        if (block == false)
        {
            block = true;
        }
        else
        {
            block = false;
        }
    }
    public void DecreaseStamina(float cost) //����� ��� ���������� ������� �� ��������� ��������
    {
        currentStamina -= cost;
    }
    public void Attack()
    {
       currentStamina -= 15f;
       Anim.SetTrigger("Attack");//��� ��������������� �������� ����� ��� ���������� ������� Attack
       cooldownTimer = 0;
       meleeAttackArea.transform.position = firePoint.position; //��� ������ ����� �� ����� ������ ��������� ������� � �������� �� ��������� ������� ����� �������� ��������� �� ������� � ��������� ��� � ����������� � ������� ���������� �����
       meleeAttackArea.GetComponent<MeleeWeapon>().meleeDirection(Mathf.Sign(transform.localScale.x));
    }
    public void magicAttack()
    {
        currentMP -= 10;
        Anim.SetTrigger("magicAttack");//��� ��������������� �������� ����� ������ ��� ���������� ������� magicAttack
        MagicCooldownTimer = 0; //����� �������� ���������� ����� ��� ���� ����� ������ ������� ��� ����� ������� ��� ������� �� ������� � ���� �� ��������, �� ����� ����� ���������
        magicProjectile[FindMagicBall()].transform.position = firePoint.position; //��� ������ ����� �� ����� ������ ��������� ������� � �������� �� ��������� ������� ����� �������� ��������� �� ������� � ��������� ��� � ����������� � ������� ���������� �����
                                                                                  // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world space
        Vector3 worldSpaceMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Get the direction from the shooter to the mouse
        Vector3 shootingDirection = worldSpaceMousePosition - transform.position;

        // Normalize the direction
        //shootingDirection.Normalize();
        Debug.Log(shootingDirection);
        magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(shootingDirection);
    }
    private void attackControl()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 20f)
        {
            Block();
        }
        if (Input.GetKey(KeyCode.LeftControl) && cooldownTimer > AttackCooldown && currentStamina > 15f)// ���� ������ �� ������ ������ ���� � ������� ������ > ��� �������� AttackCooldown, �� ����� ����������� ��� �����
        {
            Attack(); // ���������� �����
        }
        if (Input.GetKey(KeyCode.LeftAlt) && MagicCooldownTimer > magicAttackCooldown && currentMP >= 15) //���� ������ �� ����� ������ ���� � ������� ������ > ��� �������� MagicAttackCooldown, �� ����� ����������� �����
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
        Instance = this;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime; //����������� �� 1 ������� � cooldownTimer ����� ��� ��������� ��� ����������� ������ Attack.
        MagicCooldownTimer += Time.deltaTime; //����������� �� 1 ������� � MagicCooldownTimer ����� ��� ��������� ��� ����������� ������ magicAttack.
        attackControl();//����� � ������� �����
        staminaRecovery();
    }


}
