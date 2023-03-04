using UnityEngine;

public class Entity_Skeleton : MonoBehaviour
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
    public static Entity_Skeleton Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private Animator anim;
    public bool enemyDead = false;
    public bool enemyTakeDamage = false;
    public int rewardForKillEnemy = 2;//������� �� ������ ��� ������

    private CapsuleCollider2D capsuleCollider;

    private float directionY;
    private float directionX;


    private bool isBlock; //�������� ��������� �� ����
    private float blockDMG;

    private void Start()
    {
        maxHP = SaveSerial.Instance.skeletonHP;
        if (maxHP == 0)
        {
            maxHP = 50;
        }
        currentHP = maxHP;
        enemyAttackDamage = SaveSerial.Instance.skeletonDamage;
        if (enemyAttackDamage == 0)
        {
            enemyAttackDamage = 15;
        }
        Instance = this;
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
        e_rb = this.gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
    }
    public void DamageDeealToPlayer()
    {
        directionX = this.gameObject.GetComponent<Enemy_Skeleton>().directionX;
        directionY = this.gameObject.GetComponent<Enemy_Skeleton>().directionY;
        if(directionX < 0.8f && directionY < 0.3f)
        {
            Hero.Instance.GetDamage(enemyAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage
            float heal = enemyAttackDamage * 0.5f; //������ ������ �������� ����� ������� ������� ������ ������ � ���� � ��
            currentHP += heal;
            float healBar = heal / (float)maxHP; //�� ������� ���� ��������� �������� ���
            this.gameObject.GetComponentInChildren<SkeletonProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//���������� �������� ����
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
        if (transform.lossyScale.x < 0) //������� � ���������� � ����� ������� �������� �� � ������
        {
            this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(-0.5f, e_rb.velocity.y ), ForceMode2D.Impulse);//������� ��� ������ ��� ���� ����������� ����� 1 ���
        }
        else
        {
            this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0.5f, e_rb.velocity.y), ForceMode2D.Impulse);//������� ��� ������ ��� ���� ����������� ����� 1 ���
        }
    }
    public void TakeDamage(float dmg) //����� ��� ��������� ������ ��� (int dmg) ��� �������� ����� ����� ������� ��� ������ ������ (�� ���� ���� ����� ����� ������� ����)
    {
        isBlock = Enemy_Skeleton.Instance.skeleton_block;
        if (currentHP > 0 && !isBlock)
        {
            anim.SetTrigger("damage");//�������� ��������� ��������
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / (float)maxHP; //�� ������� ���� ���������� �������� ���
            this.gameObject.GetComponentInChildren<SkeletonProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//���������� �������� ����
        }
        else if(currentHP > 0 && isBlock)
        {
            //anim.SetTrigger("block_damage");//�������� ��������� ��������
            int level = LvLGeneration.Instance.Level;
            if (level < 5) //���� ������ 5 ������ �� 50% ������������ �����
            {
                blockDMG = dmg * 0.5f;
            }
            if (level >= 5) //���� ������ 5 ������ �� 90% ������������ �����
            {
                blockDMG = dmg * 0.1f;
            }
            currentHP -= blockDMG;
            enemyTakeDamage = true;
            takedDamage = blockDMG / (float)maxHP; //�� ������� ���� ���������� �������� ���
            this.gameObject.GetComponentInChildren<SkeletonProgressBar>().UpdateEnemyProgressBar(takedDamage);//���������� �������� ����
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
