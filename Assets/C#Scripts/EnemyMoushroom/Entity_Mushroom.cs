using UnityEngine;

public class Entity_Mushroom : MonoBehaviour
{
    public int maxHP = 50; //������������ ����� �������
    public int currentHP;
    public float takedDamage; //������� ����� ���� �� � ���������� ������
    public float enemyAttackRange = 1.2f; //��������� ��� �����
    public int enemyAttackDamage = 15; // ���� �� ��� �����
    public Transform enemyAttackPoint; //��� �� ��������� �� ����� ������� �������� �������� (����� ��� ���������� ��� �����)
    public LayerMask playerLayers;
    public Vector3 lossyScale;
    private Rigidbody2D e_rb;
    public static Entity_Mushroom Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private Animator anim;
    public bool enemyDead = false;
    public bool enemyTakeDamage = false;
    public int rewardForKillEnemy = 2;//������� �� ������ ��� ������

    private BoxCollider2D boxCollider;

    private float directionY;
    private float directionX;

    private void Start()
    {
        maxHP = SaveSerial.Instance.enemyHP;
        if (maxHP == 0)
        {
            maxHP = 50;
        }
        currentHP = maxHP;
        enemyAttackDamage = SaveSerial.Instance.enemyDamage;
        if (enemyAttackDamage == 0)
        {
            enemyAttackDamage = 15;
        }
        Instance = this;
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
        e_rb = this.gameObject.GetComponent<Rigidbody2D>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }
    public void DamageDeealToPlayer()
    {
        directionX = Enemy_Mushroom.Instance.directionX;
        directionY = Enemy_Mushroom.Instance.directionY;
        if(directionX < 0.8f && directionY < 0.3f)
        {
            Hero.Instance.GetDamage(enemyAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage  
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
    public void TakeDamage(int dmg) //����� ��� ��������� ������ ��� (int dmg) ��� �������� ����� ����� ������� ��� ������ ������ (�� ���� ���� ����� ����� ������� ����)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("damage");//�������� ��������� ��������
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / (float)maxHP; //�� ������� ���� ���������� �������� ���
            //Debug.Log(takedDamage);
            //Push();
            this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//���������� �������� ����
            //Debug.Log(currentHP + " " + gameObject.name);
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
            boxCollider.enabled = false;
            anim.SetTrigger("m_death");//�������� ������
            enemyDead = true;
            //Debug.Log("Enemy Defeat -> " + gameObject.name);
        }
    }
    public virtual void Die() //����� ������� ���� ������� ������, ���������� ����� �������� ����� ����� ���������� �������� ������
    {
        Destroy(this.gameObject);//���������� ���� ������� ������
        LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
    }
}
