using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Projectile Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float Speed; //�������� �������
    [SerializeField] private float lifetime; //������������ ����� �������
    private bool hit = false; //���������� ����� ����� �� �� ���-�� ������

    public Rigidbody2D rb; //���������� ����

    private BoxCollider2D boxCollider; //��������� �����
    private Animator anim; //���������� ��� ���������

    public int lifeTimeOfprojectile = 10; //����� ����� �������� ������ ������������
    public float magicAttackDamage = 20;
    public string magicTargetName; //��� ���� �� �������� ����� ������
    public GameObject target; //������ �� �������� ����� ������

    private float shootingForce = 0.03f; //�������� �������

    private void Awake() //�������� ����������� �� ������ ���� � 1 ���
    {
        anim = GetComponent<Animator>(); // ����������� ���������� �� ���������� ��������
        boxCollider = GetComponent<BoxCollider2D>(); // ����������� ���������� �� ���������� ���� ��������
        Instance = this;
    }

    private void Start()
    {
        SaveSerial.Instance.LoadGame();
        magicAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (magicAttackDamage == 0)
        {
            magicAttackDamage = 30;
        }
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (hit) return; //�������� ��������� �������� ���� �� ���-������
        float movementSpeed = Speed * Time.deltaTime * direction; // ���������� �������� ����������� � ������� � � ����� ����������� ������� ������
        transform.Translate(movementSpeed, 0, 0);//��� � = movementspeed, y = 0, z=0 - ��� ��� ����������� �� ��� x
        lifetime += Time.deltaTime; //���������� ���������� lifetime ������ ��� +1
        if (lifetime > lifeTimeOfprojectile) gameObject.SetActive(false);//����� ���������� ��������� 5, ������ ��������
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            return;
        }
        else
        {
            magicTargetName = collision.gameObject.name;
            hit = true; //��� ��������� ��� ��������� ������������
            boxCollider.enabled = false; //��������� ���������
            anim.SetTrigger("explode");//��� ��������������� �������� ����� �������� ��� ���������� ������� magicAttack
            DamageObject();
            magicTargetName = string.Empty;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    public void DamageObject()
    {
        //Debug.Log(magicTargetName);
        target = GameObject.Find(magicTargetName);
        target.GetComponent<Entity_Enemy>().TakeDamage(magicAttackDamage);
    }
    public void SetDirection(Vector3 shootingDirection)// ����� ����������� ������ 
    {
            lifetime = 0;
            gameObject.SetActive(true); //��������� �������� �������
            boxCollider.enabled = true; //��������� ���������� 
            hit = false; //������ �������� ������� ������� = false
            Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>(); //��������� ���������� RigidBody2D
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(shootingDirection * shootingForce); //���������� ���� � ������� = ����������� ��������� �� �������� �������
    }
    private void Deactivate() //����������� ������� ����� ���������� �������� �������
    {
        gameObject.SetActive(false);
    }
}
