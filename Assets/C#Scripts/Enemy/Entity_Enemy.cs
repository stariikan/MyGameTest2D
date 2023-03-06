using UnityEngine;

public class Entity_Enemy : MonoBehaviour
{
    public float skeletonMaxHP = 50; //������������ ����� �������
    public float skeletonAttackDamage = 15; // ���� �� ��� �����
    public int skeletonReward = 2;//������� �� ������ ��� ������
    private bool isBlock; //�������� ��������� �� ����
    private float blockDMG;



    public float moushroomMaxHP = 50; //������������ ����� �����
    public float moushroomAttackDamage = 15; // ���� �� ��� �����
    public int moushroomReward = 2;//������� �� ������ ��� ������


    public float goblinMaxHP = 35; //������������ ����� �������
    public float goblinAttackDamage = 25; // ���� �� ��� �����
    public int goblinReward = 2;//������� �� ������ ��� ������

    public float slimeMaxHP = 20;//������������ ����� ������
    public float slimeAttackDamage = 15; // ���� �� ��� �����
    public int slimeReward = 1;//������� �� ������ ��� ������

    public float deathMaxHP = 150;//������������ ����� ������
    public float deathAttackDamage = 50; // ���� �� ��� �����
    public int deathReward = 40;//������� �� ������ ��� ������

    private float playerHP;
    //���������� ��� ������ ������� ��������� ����� ������� � ������
    private float directionY; 
    private float directionX;

    public float currentHP;
    public float takedDamage; //������� ����� ���� �� � ���������� ������
    public float enemyAttackRange = 1.2f; //��������� ��� �����

    public bool enemyDead = false;
    public bool enemyTakeDamage = false;
    
    public Transform enemyAttackPoint; //��� �� ��������� �� ����� ������� �������� �������� (����� ��� ���������� ��� �����)
    
    public LayerMask playerLayers;
    public Vector3 lossyScale;
    private Rigidbody2D e_rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    string tag; // � ���� ���������� ������������� ��� �� ������
    public static Entity_Enemy Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void Start()
    {
        Instance = this;
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
        e_rb = this.gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        tag = this.gameObject.transform.tag;
        if (tag == "Skeleton")
        {
            skeletonMaxHP = SaveSerial.Instance.skeletonHP;
            if (skeletonMaxHP == 0)
            {
                skeletonMaxHP = 50;
            }
            currentHP = skeletonMaxHP;
            skeletonAttackDamage = SaveSerial.Instance.skeletonDamage;
            if (skeletonAttackDamage == 0)
            {
                skeletonAttackDamage = 15;
            }
        }
        if (tag == "Mushroom")
        {
            moushroomMaxHP = SaveSerial.Instance.moushroomHP;
            if (moushroomMaxHP == 0)
            {
                moushroomMaxHP = 50;
            }
            currentHP = moushroomMaxHP;
            moushroomAttackDamage = SaveSerial.Instance.moushroomDamage;
            if (moushroomAttackDamage == 0)
            {
                moushroomAttackDamage = 15;
            }
        }
        if (tag == "Goblin")
        {
            goblinMaxHP = SaveSerial.Instance.goblinHP;
            if (goblinMaxHP == 0)
            {
                goblinMaxHP = 35;
            }
            currentHP = goblinMaxHP;
            goblinAttackDamage = SaveSerial.Instance.goblinDamage;
            if (goblinAttackDamage == 0)
            {
                goblinAttackDamage = 25;
            }
        }
        if (tag == "Slime")
        {
            if (slimeMaxHP == 0)
            {
                slimeMaxHP = 35;
            }
            currentHP = slimeMaxHP;
            if (slimeAttackDamage == 0)
            {
                slimeAttackDamage = 15;
            }
        }
        if (tag == "Death")
        {
            if (deathMaxHP == 0)
            {
                deathMaxHP = 150;
            }
            currentHP = deathMaxHP;
            if (deathAttackDamage == 0)
            {
                deathAttackDamage = 50;
            }
        }
    }
    public void DamageDeealToPlayer()
    {
        directionX = Enemy_Behavior.Instance.directionX;
        directionY = Enemy_Behavior.Instance.directionY;
        if(directionX < 0.8f && directionY < 0.3f)
        {
            if (tag == "Skeleton")
            {
                Hero.Instance.GetDamage(skeletonAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage
                float heal = skeletonAttackDamage * 0.5f; //������ ������ �������� ����� ������� ������� ������ ������ � ���� � ��
                currentHP += heal;
                float healBar = heal / (float)skeletonMaxHP; //�� ������� ���� ��������� �������� ���
                this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//���������� �������� ����
            }
            if (tag == "Mushroom")
            {
                Hero.Instance.GetDamage(moushroomAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage
            }
            if (tag == "Goblin")
            {
                Hero.Instance.GetDamage(goblinAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage
            }
            if (tag == "Slime")
            {
                Hero.Instance.GetDamage(slimeAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage
            }
            if (tag == "Death")
            {
                Hero.Instance.GetDamage(deathAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage
            }
            
        }                                                       
    }
    //������ ��� ���� �������� ������������� ������, ���� ����������� ����� ����, ��� ����� �������� ��� ��������������
    public void BoostEnemyHP() 
    {
        skeletonMaxHP += 10;
        moushroomMaxHP += 10;
        goblinMaxHP += 10;
    }
    public void BoostEnemyAttackDamage() //��� ��������� ����
    {
        skeletonAttackDamage += 3;
        moushroomAttackDamage += 3;
        goblinAttackDamage += 3;
    }
    public void BoostEnemyReward() //��� ����������� ������� �� ��������
    {
        skeletonReward += 2;
        moushroomReward += 2;
        goblinReward += 2;
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
        isBlock = Enemy_Behavior.Instance.skeleton_block;
        if (currentHP > 0 && !isBlock)
        {
            anim.SetTrigger("damage");//�������� ��������� ��������
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / (float)skeletonMaxHP; //�� ������� ���� ���������� �������� ���
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//���������� �������� ����
        }
        else if(currentHP > 0 && isBlock)
        {
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
            float maxHP = 50;
            if (tag == "Skeleton")
            {
                maxHP = skeletonMaxHP;
            }
            if (tag == "Mushroom")
            {
                maxHP = moushroomMaxHP;
            }
            if (tag == "Goblin")
            {
                maxHP = goblinMaxHP;
            }
            if (tag == "Slime")
            {

            }
            if (tag == "Death")
            {

            }
            takedDamage = blockDMG / maxHP; //�� ������� ���� ���������� �������� ���
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//���������� �������� ����
        }
        if (currentHP <= 0)
        {
            int reward = 2;
            if (tag == "Skeleton")
            {
                reward = skeletonReward;
            }
            if (tag == "Mushroom")
            {
                reward = moushroomReward;
            }
            if (tag == "Goblin")
            {
                reward = goblinReward;
            }
            if (tag == "Slime")
            {
                reward = 1;
            }
            if (tag == "Death")
            {
                reward = 40;
            }
            LvLGeneration.Instance.PlusCoin(reward);//����� ������ ��� ���������� �����
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
        if (tag == "Skeleton")
        {
            LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        }
        if (tag == "Mushroom")
        {
            LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        }
        if (tag == "Goblin")
        {
            LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        }
        if (tag == "Slime")
        {
            
        }
        if (tag == "Death")
        {
            LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        }
        
    }

}
