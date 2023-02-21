using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    [SerializeField] private float magicAttackCooldown;//������� ������� ������� (�����)
    [SerializeField] private Transform firePointRight; //������� �� ������� ����� �������� �������
    [SerializeField] private Transform firePointLeft; //������� �� ������� ����� �������� �������
    [SerializeField] private GameObject[] magicProjectile; //������ ����� ��������
    [SerializeField] private GameObject meleeAttackArea; // ��� ������

    public static HeroAttack Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    
    private float MagicCooldownTimer = Mathf.Infinity; //���� �� �������� ��� 0, �� ����� ������� �� ������ ���������� ������-��� �� ����� ������ attackCooldown. ������� �� �������� ��� ������������� ��� ����� ��������� ����� ������� �����
    public float maxMP = 100;
    public float currentMP;
    public float stamina = 100;
    public float currentStamina;
    private float staminaSpeedRecovery = 10f;

    public bool block = false;

    private int playerDirecction;

    private Vector3 shootingDirection;


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
        Instance = this;
        mainCamera = Camera.main;
    }
    private void Update()
    {
        MagicCooldownTimer += Time.deltaTime; //����������� �� 1 ������� � MagicCooldownTimer ����� ��� ��������� ��� ����������� ������ magicAttack.
        attackControl();//����� � ������� �����
        staminaRecovery();
        playerDirecction = Hero.Instance.m_facingDirection;
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
        if (currentStamina < 20)
        {
            block = false;
        }
    }
    public void Block()
    {
        if (block == false && currentStamina > 20)
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
       if(playerDirecction > 0)
        {
            meleeAttackArea.transform.position = firePointRight.position; //��� ������ ����� �� ����� ������ ��������� ������� � �������� �� ��������� ������� ����� �������� ��������� �� ������� � ��������� ��� � ����������� � ������� ���������� �����
            meleeAttackArea.GetComponent<MeleeWeapon>().meleeDirection(firePointRight.position);
        }
       else if (playerDirecction < 0)
        {
            meleeAttackArea.transform.position = firePointLeft.position;
            meleeAttackArea.GetComponent<MeleeWeapon>().meleeDirection(firePointLeft.position);
        }
    }
    public void magicAttack()
    {
        if(MagicCooldownTimer > magicAttackCooldown)
        {
            currentMP -= 20;
            MagicCooldownTimer = 0; //����� �������� ���������� ����� ��� ���� ����� ������ ������� ��� ����� ������� ��� ������� �� ������� � ���� �� ��������, �� ����� ����� ���������
            if (playerDirecction > 0)
            {
                magicProjectile[FindMagicBall()].transform.position = firePointRight.position; //��� ������ ����� �� ����� ������ ��������� ������� � �������� �� ��������� ������� ����� �������� ��������� �� ������� � ��������� ��� � ����������� � ������� ���������� �����
            }
            if (playerDirecction < 0)
            {
                magicProjectile[FindMagicBall()].transform.position = firePointLeft.position;
            }

            if (playerDirecction > 0)
            {
                shootingDirection = new Vector3(1, 0, 109);
            }
            if (playerDirecction < 0)
            {
                shootingDirection = new Vector3(-1, 0, 109);
            }
            magicProjectile[FindMagicBall()].GetComponent<Projectile>().SetDirection(shootingDirection);
        }
    }
    private void attackControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > 20f)
        {
            Block();
        }
        if (Input.GetKey(KeyCode.LeftAlt) && currentMP >= 15) //���� ������ �� ����� ������ ���� � ������� ������ > ��� �������� MagicAttackCooldown, �� ����� ����������� �����
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
}
