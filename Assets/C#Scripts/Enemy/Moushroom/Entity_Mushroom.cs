using UnityEngine;

public class Entity_Mushroom : MonoBehaviour
{
    public float maxHP = 50; //������������ ����� �������
    public float currentHP;
    public float takedDamage; //������� ����� ���� �� � ���������� ������
    public float enemyAttackRange = 1.2f; //��������� ��� �����
    public float enemyAttackDamage = 15; // ���� �� ��� �����
    public Transform enemyAttackPoint; //��� �� ��������� �� ����� ������� �������� �������� (����� ��� ���������� ��� �����)
    public LayerMask playerLayers;
    public Vector3 lossyScale;
    private Rigidbody2D e_rb;
    public static Entity_Mushroom Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private Animator anim;
    public bool enemyDead = false;
    public bool enemyTakeDamage = false;
    public int rewardForKillEnemy = 2;//������� �� ������ ��� ������

    private float playerHP;

    private CapsuleCollider2D capsuleCollider;

    private float directionY;
    private float directionX;

    private void Start()
    {
        maxHP = SaveSerial.Instance.moushroomHP;
        if (maxHP == 0)
        {
            maxHP = 50;
        }
        currentHP = maxHP;
        enemyAttackDamage = SaveSerial.Instance.moushroomDamage;
        if (enemyAttackDamage == 0)
        {
            enemyAttackDamage = 15;
        }
        Instance = this;
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
        e_rb = this.gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        playerHP = Hero.Instance.hp;
    }
    public void DamageDeealToPlayer()
    {
        directionX = this.gameObject.GetComponent<Enemy_Mushroom>().directionX;
        directionY = this.gameObject.GetComponent<Enemy_Mushroom>().directionY;
        if(playerHP > 0 && directionX < 1.5f && directionY < 0.3f)
        {
            Hero.Instance.GetDamage(enemyAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage
            maxHP += (enemyAttackDamage / 3); //�������� �������
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
    public void TakeDamage(float dmg) //����� ��� ��������� ������ ��� (int dmg) ��� �������� ����� ����� ������� ��� ������ ������ (�� ���� ���� ����� ����� ������� ����)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("damage");//�������� ��������� ��������
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / (float)maxHP; //�� ������� ���� ���������� �������� ���
            this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//���������� �������� ����
        }
        else
        {
            return;
        }

        if (currentHP <= 0)
        {
            LvLGeneration.Instance.PlusCoin(rewardForKillEnemy);//����� ������ ��� ���������� �����
            e_rb.gravityScale = 0;
            e_rb.velocity = Vector2.zero;
            capsuleCollider.enabled = false;
            anim.StopPlayback();
            anim.SetBool("dead", true);
            anim.SetTrigger("m_death");//�������� ������
            enemyDead = true;
        }
    }
    public virtual void Die() //����� ������� ���� ������� ������, ���������� ����� �������� ����� ����� ���������� �������� ������
    {
        Destroy(this.gameObject);//���������� ���� ������� ������
        LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
    }
}
