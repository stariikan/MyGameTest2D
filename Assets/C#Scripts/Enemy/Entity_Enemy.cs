using UnityEngine;

public class Entity_Enemy : MonoBehaviour
{
    //��������� �������
    public float skeletonMaxHP = 70; //������������ ����� �������
    public float skeletonAttackDamage = 10; // ���� �� ��� �����
    public int skeletonReward = 2;//������� �� ������ ��� ������
    private bool isBlock; //�������� ��������� �� ����
    private float blockDMG;

    //��������� �����
    public float mushroomMaxHP = 70; //������������ ����� �����
    public float mushroomAttackDamage = 10; // ���� �� ��� �����
    public int mushroomReward = 2;//������� �� ������ ��� ������

    //��������� ��������� �����
    public float flyingEyeMaxHP = 70; //������������ ����� ��������� �����
    public float flyingEyeAttackDamage = 10; // ���� �� ��� �����
    public int flyingEyeReward = 2;//������� �� ������ ��� ������

    //��������� �������
    public float goblinMaxHP = 50; //������������ ����� �������
    public float goblinAttackDamage = 15; // ���� �� ��� �����
    public int goblinReward = 2;//������� �� ������ ��� ������

    //��������� ����� ����
    public float wizardMaxHP = 50; //������������ ����� �������
    public float wizardAttackDamage = 10; // ���� �� ��� �����
    public int wizardReward = 2;//������� �� ������ ��� ������

    //��������� �������
    public float martialMaxHP = 75; //������������ ����� �������
    public float martialAttackDamage = 20; // ���� �� ��� �����
    public int martialReward = 2;//������� �� ������ ��� ������

    //��������� ������
    public float slimeMaxHP = 40;//������������ ����� ������
    public float slimeAttackDamage = 15; // ���� �� ��� �����
    public int slimeReward = 1;//������� �� ������ ��� ������

    //��������� ���� ������
    public float deathMaxHP = 900;//������������ ����� ������
    public float deathAttackDamage = 25; // ���� �� ��� �����
    public int deathReward = 40;//������� �� ������ ��� ������

    //���������� ��� ������ ������� ��������� ����� ������� � ������
    private float directionY; 
    private float directionX;

    //����� ���������
    public float currentHP; //�� �������
    public float takedDamage; //������� ����� ���� �� � ���������� ������
    public float enemyAttackRange = 1.2f; //��������� ��� �����
    public bool enemyDead = false; //������� �� ������
    public bool enemyTakeDamage = false; //������� �� ������ ����

    [SerializeField] private Transform firePoint; //������� �� ������� ����� �������� �������
    [SerializeField] private GameObject[] blood; //�����
    public Vector3 lossyScale;
    public Vector3 thisObjectPosition;
    private Rigidbody2D e_rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    new string tag; // � ���� ���������� ������������� ��� �� ������
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
            if (skeletonMaxHP == 0) skeletonMaxHP = 70;
            currentHP = skeletonMaxHP;
            skeletonAttackDamage = SaveSerial.Instance.skeletonDamage;
            if (skeletonAttackDamage == 0) skeletonAttackDamage = 10;
        }
        if (tag == "Mushroom")
        {
            mushroomMaxHP = SaveSerial.Instance.mushroomHP;
            if (mushroomMaxHP == 0) mushroomMaxHP = 70;
            currentHP = mushroomMaxHP;
            mushroomAttackDamage = SaveSerial.Instance.mushroomDamage;
            if (mushroomAttackDamage == 0) mushroomAttackDamage = 10;
        }
        if (tag == "FlyingEye")
        {
            flyingEyeMaxHP = SaveSerial.Instance.mushroomHP;
            if (flyingEyeMaxHP == 0) flyingEyeMaxHP = 70;
            currentHP = flyingEyeMaxHP;
            flyingEyeAttackDamage = SaveSerial.Instance.flyingEyeDamage;
            if (flyingEyeAttackDamage == 0) flyingEyeAttackDamage = 10;
        }
        if (tag == "Goblin")
        {
            goblinMaxHP = SaveSerial.Instance.goblinHP;
            if (goblinMaxHP == 0) goblinMaxHP = 50;
            currentHP = goblinMaxHP;
            goblinAttackDamage = SaveSerial.Instance.goblinDamage;
            if (goblinAttackDamage == 0) goblinAttackDamage = 15;
        }
        if (tag == "EvilWizard")
        {
            wizardMaxHP = SaveSerial.Instance.wizardHP;
            if (wizardMaxHP == 0) wizardMaxHP = 50;
            currentHP = wizardMaxHP;
            wizardAttackDamage = SaveSerial.Instance.wizardDamage;
            if (wizardAttackDamage == 0) wizardAttackDamage = 10;
        }
        if (tag == "Martial")
        {
            martialMaxHP = SaveSerial.Instance.martialHP;
            if (martialMaxHP == 0) martialMaxHP = 75;
            currentHP = martialMaxHP;
            martialAttackDamage = SaveSerial.Instance.martialDamage;
            if (martialAttackDamage == 0) martialAttackDamage = 20;
        }
        if (tag == "Slime")
        {
            if (slimeMaxHP == 0) slimeMaxHP = 40;
            currentHP = slimeMaxHP;
            if (slimeAttackDamage == 0) slimeAttackDamage = 15;
        }
        if (tag == "Death")
        {
            if (deathMaxHP == 0) deathMaxHP = 900;
            currentHP = deathMaxHP;
            if (deathAttackDamage == 0) deathAttackDamage = 25;
        }
    }
    //������ ��� ���� �������� ������������� ������, ���� ����������� ����� ����, ��� ����� �������� ��� ��������������
    public void BoostEnemyHP() 
    {
        skeletonMaxHP *= 1.2f;
        mushroomMaxHP *= 1.2f;
        goblinMaxHP *= 1.2f;
        wizardMaxHP *= 1.2f;
        martialMaxHP *= 1.2f;
        flyingEyeMaxHP *= 1.2f;
    }
    public void BoostEnemyAttackDamage() //��� ��������� ����
    {
        skeletonAttackDamage *= 1.2f;
        mushroomAttackDamage *= 1.2f;
        goblinAttackDamage *= 1.2f;
        wizardAttackDamage *= 1.2f;
        martialAttackDamage *= 1.2f;
        flyingEyeAttackDamage *= 1.2f;
    }
    public void BoostEnemyReward() //��� ����������� ������� �� ��������
    {
        skeletonReward += 2;
        mushroomReward += 2;
        goblinReward += 2;
        wizardReward += 2;
        martialReward += 2;
        flyingEyeReward += 2;
    }

    //����� ������ � ���������
    public void DamageDeealToPlayer() // ����� ��� ��������� ����� ������
    {
        directionX = Enemy_Behavior.Instance.directionX;
        directionY = Enemy_Behavior.Instance.directionY;
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Skeleton")
        {
            Hero.Instance.GetDamage(skeletonAttackDamage);//��� �� �������� ������ � ������� ������ � ���������� ������ ������� GetDamage
            float heal = skeletonAttackDamage * 0.5f; //������ ������ �������� ����� ������� ������� ������ ������ � ���� � ��
            currentHP += heal;
            float healBar = heal / (float)skeletonMaxHP; //�� ������� ���� ��������� �������� ���
            if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//���������� �������� ����
        }
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Mushroom") Hero.Instance.GetDamage(mushroomAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "FlyingEye") Hero.Instance.GetDamage(mushroomAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Goblin") Hero.Instance.GetDamage(goblinAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Slime") Hero.Instance.GetDamage(slimeAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Martial") Hero.Instance.GetDamage(martialAttackDamage);
        if (directionX < 1.8f && currentHP > 0 && directionY < 1f && tag == "Death")
        {
            Hero.Instance.GetDamage(deathAttackDamage);
            float heal = deathAttackDamage * 0.5f; //������ ������ �������� ����� ������� ������� ������ ������ � ���� � ��
            currentHP += heal;
            float healBar = heal / (float)deathMaxHP; //�� ������� ���� ��������� �������� ���
            this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//���������� �������� ����
        }
    }
    public void Push() //����� ��� ������������ ����
    {
        if (transform.lossyScale.x < 0) this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(-0.5f, e_rb.velocity.y), ForceMode2D.Impulse);
        else this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0.5f, e_rb.velocity.y), ForceMode2D.Impulse);
    }
    public void TakeDamage(float dmg) //��������� ����� (� dmg ����������� ��������, � Hero ������� ��� ������ ������ TakeDamage � dmg ������������ ���������� ������ �� ������ ) 
    {
        float maxHP = 1;
        if (tag == "Skeleton") maxHP = skeletonMaxHP;
        if (tag == "Mushroom") maxHP = mushroomMaxHP;
        if (tag == "FlyingEye") maxHP = flyingEyeMaxHP;
        if (tag == "Goblin") maxHP = goblinMaxHP;
        if (tag == "EvilWizard") maxHP = wizardMaxHP;
        if (tag == "Martial") maxHP = martialMaxHP;
        if (tag == "Slime") maxHP = slimeMaxHP;
        if (tag == "Death") maxHP = deathMaxHP;

        isBlock = this.gameObject.GetComponent<Enemy_Behavior>().block;
        //Debug.Log(isBlock);
        if (currentHP > 0 && !isBlock)
        {
            if (tag != "Skeleton")
            {
                GameObject bloodSpawn = Instantiate(blood[Random.Range(0, blood.Length)], new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity); //������������ �������
                bloodSpawn.gameObject.SetActive(true);
            }
            
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / maxHP; //�� ������� ���� ���������� �������� ���
            anim.SetTrigger("damage");//�������� ��������� ��������
            Enemy_Behavior.Instance.TakeDamageSound();
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//���������� �������� ����
        }
        if (currentHP > 0 && isBlock)
        {
            int level = LvLGeneration.Instance.Level;
            if (level <= 4) blockDMG = dmg * 0.5f;//���� ����� ���� 5 ������ �� 50% ������������ �����
            if (level >= 5) blockDMG = dmg * 0.1f;//���� ����� ���� ��� 4 ������� �� 90% ������������ �����
            currentHP -= blockDMG;
            Debug.Log(blockDMG);
            Enemy_Behavior.Instance.ShieldDamageSound();
            enemyTakeDamage = true;
            takedDamage = blockDMG / maxHP; //�� ������� ���� ���������� �������� ���
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//���������� �������� ����
        }
        if (currentHP <= 0)
        {
            int reward = 2;
            if (tag == "Skeleton") reward = skeletonReward;
            if (tag == "Mushroom") reward = mushroomReward;
            if (tag == "FlyingEye") reward = mushroomReward;
            if (tag == "Goblin") reward = goblinReward;
            if (tag == "Martial") reward = martialReward;
            if (tag == "Slime") reward = 1;
            if (tag == "Death") reward = 40;
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
        bool copy = this.gameObject.GetComponent<Enemy_Behavior>().copy;
        Destroy(this.gameObject);//���������� ���� ������� ������
        if (tag == "Skeleton") LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        if (tag == "Mushroom") LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        if (tag == "FlyingEye" && !copy) LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        if (tag == "Goblin") LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        if (tag == "EvilWizard") LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        if (tag == "Martial") LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
        if (tag == "Slime")
        {
            GameObject[] deathObjects = GameObject.FindGameObjectsWithTag("Death");
            foreach (GameObject obj in deathObjects)
            {
                if (obj.name != "BossDeath")
                {
                    obj.GetComponent<Entity_Enemy>().BossDeathDamage(50);
                }
            }
        }
        if (tag == "Death") LvLGeneration.Instance.FindKey();//����� ������ ��� ��������� ������
    }

    //������ ����� � ������ �����
    public void BossDeathHeal(float heal)
    {
        currentHP += heal;
        float healBar = heal / deathMaxHP; //�� ������� ���� ��������� �������� ���
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//���������� �������� ����
    }
    public void BossDeathDamage(float dmg)
    {
        currentHP -= dmg;
        enemyTakeDamage = true;
        takedDamage = dmg / deathMaxHP; //�� ������� ���� ���������� �������� ���
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//���������� �������� ����
    }

}

