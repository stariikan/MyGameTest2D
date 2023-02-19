using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float lifetime; //������������ ����� �������

    private BoxCollider2D boxCollider; //��������� �����

    public float AttackDamage = 15;
    public string TargetName;
    public GameObject target;


    private void Awake() //�������� ����������� �� ������ ���� � 1 ���
    {
        //anim = GetComponent<Animator>(); // ����������� ���������� �� ���������� ��������
        boxCollider = GetComponent<BoxCollider2D>(); // ����������� ���������� �� ���������� ���� ��������
        Instance = this;
    }
    private void Start()
    {
        SaveSerial.Instance.LoadGame();
        AttackDamage = SaveSerial.Instance.playerAttackDamage;
        if (AttackDamage == 0)
        {
            AttackDamage = 15;
        }
    }

    private void Update()
    {
        lifetime += Time.deltaTime; //���������� ���������� lifetime ������ ��� +1
        if (lifetime > 1) 
        {
            gameObject.SetActive(false);//����� ���������� ��������� 1.5, ��������� ����� ��������
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        boxCollider.enabled = false; //��������� ���������
        target = GameObject.Find(TargetName);
    }
    public void meleeDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        lifetime = 0;
        gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //��������� ����������

        if (target.CompareTag("Skeleton"))
        {
            target.GetComponent<Entity_Skeleton>().TakeDamage(AttackDamage);
        }
        if (target.CompareTag("Mushroom"))
        {
            target.GetComponent<Entity_Mushroom>().TakeDamage(AttackDamage);
        }
        if (target.CompareTag("Chest"))
        {
            target.GetComponent<Chest>().TakeDamage(AttackDamage);
        }
        if (target.CompareTag("Door"))
        {
            target.GetComponent<door>().TryToOpen();
        }
    }
    private void Deactivate() //����������� ������� ����� ���������� �������� �������
    {
        gameObject.SetActive(false);
    }
}
