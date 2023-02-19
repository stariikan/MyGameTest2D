using UnityEngine;

public class Enemy_Skeleton : MonoBehaviour //������������ ������ �������� (�� ���� ������ ������� ������������ � Entity ����� ��������� � � ����� �������)
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 2f;//�������� �������� �������
    [SerializeField] private float speedRecovery;//�������� �������� ������� 2 �������� ����� ��� �������������� �������� �� ��������� (����� ��������� ����� ��������� � �������� ����� ���������� ������� � ��� �� ��������))

    GameObject player; //���������� ����� � ���� ����� ����� ��� �� ������������ � ������������� ���� ����������
    public Rigidbody2D rb; //���������� ����
    public Transform groundcheck;// �������� ������������� �� ����� (������� �� ������� � ������)
    private Animator anim; //���������� ��������� ������� ���������� ������, ���������� = skelet_anim
    private float e_delayToIdle = 0.0f;
    public Transform wallChekPoint; //��� �� ��������� �� ����� ������� �������� �������� �������� 
    private bool isGround; // ���������� �� ������ �� �����
    private bool playerFollow = false; //��� �� ���������� ������
    RaycastHit2D hit; //��� ����� �������� ���������� � ��� ������������ ������

    public float directionX; //���������� ��� ��������� ������� ����� ������� � ������
    public float directionY; //���������� ��� ��������� ������� ����� ������� � ������

    private bool playerIsAttack; //������� �� �����?
    private bool isAttack; //������� �� ������ (����)
    private int currentAttack = 0; //������� �� ����� �������
    private float timeSinceAttack = 0.0f;//����� � ������� ����� ����� ��� ����� �������� �����
    private float blockCooldown; //������� �� ������ � ������
    private int level; //�������� ����� ������� �������� �����, ����� ��� ����������� ������������
    public bool skeleton_block = false;
    public static Enemy_Skeleton Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private void Start()
    {
        player = GameObject.FindWithTag("PlayerCharacter"); //��� ��� ������ ���� ������ ������� ������ �� ���� Player � ����������� ��������� � ���������� ���������� player
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object) � �������� �������� ������
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object) � �������� �������� ������
        speed = SaveSerial.Instance.skeletonSpeed;
        if (speed < 2f)
        {
            speed = 2f;
        }
        speedRecovery = speed;
        Instance = this;
        level = LvLGeneration.Instance.Level;
    }
    void Update() //��� ���������� ���� �������� �������� ������ (������� ������ ������������ ����)
    {
        timeSinceAttack += Time.deltaTime;
        blockCooldown += Time.deltaTime;
        if (this.gameObject.GetComponent<Entity_Skeleton>().currentHP > 0)
        {
            PlayerFollow(); //�������� �� �������
            DieByFall(); // ������ ��� �������
            AnimState(); //����������� ��������
            groundCheckPosition(); //�������� ��������
            EnemyJump(); //������ ����� �����������
            Attack(); //�����
            Block(); //����
        }
        else
        {
            return;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) //����������� �����, ����� ��� ������ ������������� � ������ ��������:
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision) //����������� �����, ����� ��������������� ���� �������� �����������.
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
    private States State //�������� �����������, ���������� = State. �������� ��������� ����� ���� �������� ��� �������� ����� ��������� get � set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    public enum States //����������� ����� ������ ���������, ������ �������� ��� � ��������� Unity
    {
        idle,
        run
    }
    public void Flip() //��� �� ������� ����� Flip ��� ������ �������� ������ ������ �����������
    {
        Vector3 theScale = transform.localScale; //��������� ������� �������
        theScale.x *= -1;//��� ���������� ��������� ����������� �������� 140 �������� �� -140 ��� ����� ��������� ������ ����������� ������� (�������� ���������������)
        transform.localScale = theScale; //������� �������������� ������������ ������������� ������� GameObjects
    }
    public void BoostSpeed() //����� ��� �������� ��������
    {
        speed *= 1.1f;
    }
    public void Block() // ������������� ����
    {
        playerIsAttack = Hero.Instance.isAttack;
        if (playerIsAttack == true && (Mathf.Abs(directionX)) < 1.5f && Mathf.Abs(directionY) < 2 && level > 3)
        {
            blockCooldown = 0;
            skeleton_block = true;
            anim.SetBool("Block", true);
        }
        if (blockCooldown > 0.4f)
        {
            skeleton_block = false;
            anim.SetBool("Block", false);
        }
    }
    public void PlayerFollow() //����� � ������� ��������� ������ ���������� �� �������
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //���������� ����������� �������� ��� ������� ������ �� ��� � - ������� ������� �� ��� �
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //���������� ����������� �������� ��� ������� ������ �� ��� y - ������� ������� �� ��� y
        if ((Mathf.Abs(directionX) < 5 && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) && !skeleton_block && !isAttack || this.gameObject.GetComponent<Entity_Skeleton>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !skeleton_block && !isAttack) //������� �� ������� ���� ��������� ��������� ��� ������� ����
        {
            Vector3 pos = transform.position; //������� �������
            Vector3 theScale = transform.localScale; //����� ��� ��������� �����������
            transform.localScale = theScale; //����� ��� ��������� �����������
            float playerFollowSpeed = Mathf.Sign(directionX) * speed * Time.deltaTime; //���������� �����������
            pos.x += playerFollowSpeed; //���������� ������� �� ��� �
            transform.position = pos; //���������� �������
            playerFollow = true;
        
            if (playerFollowSpeed < 0 && theScale.x > 0) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
            else if (playerFollowSpeed > 0 && theScale.x < 0) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
            {
                Flip();
            }
        }
        else
        {
            playerFollow = false;
        }
    }
    public void Attack()
    {
        float playerHP = Hero.Instance.hp;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && !skeleton_block && timeSinceAttack > 1)
        {
            isAttack = true;
            //Damage Deal
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 2.0f)
                currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack);
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else
        {
            isAttack = false;
        }
    }
    public void EnemyJump() //������ ���� ��������� ����� �����������
    {
        RaycastHit2D wall = Physics2D.Raycast(wallChekPoint.position, transform.localPosition, 0.04f, LayerMask.GetMask("Ground")); //�������� ��� ����� ������������ ����� ������ ����� ����� � ������� 0,04f ����� ����� ������ � ����� ����� 
        if (wall != false && isGround != false)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(rb.velocity.x, 1f), ForceMode2D.Impulse); // ������������� ���� �� Y ��� ������
        }
    }
    private void DieByFall() //����� ������� ������� ���� ��� ������� � ���������
    {
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity_Skeleton>().enemyDead == false)//���� ���������� ������ �� ��� y ������ 10 � ���� �� �����, �� ���������� ����� ������ GetDamage
        {
            this.gameObject.GetComponent<Entity_Skeleton>().TakeDamage(10);
        }
    }
    public void AnimState()//����� ��� ����������� ������ ��������
    {      
        if (playerFollow == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if(playerFollow == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0)
            this.gameObject.GetComponent<Animator>().SetInteger("State", 0);
        }
    }
    //�������� �� ��������, ����� ������ ���� �� ���� �� �������� Raycast ���� � ������� ������� groundcheck, �� 2 ������� � ��������� ���������� �� ������ � ������ (groundLayers) PlayerFollow();
    public void groundCheckPosition()
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.collider != true) //���� ������ groundcheck �� ���������� � ����� (�� ���� ��������)
        {
            speed = 0f;//�� ��������� ����� �� 0
        }
        else
        {
            speed = speedRecovery;//���� ������ ground check ����� ������������ � ����� (����� �����), �� ���������� ���������� ��������.
        }
    }
}
