using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        boxCollider.enabled = false; //��������� ���������
        this.gameObject.SetActive(false);//����� ���������� ��������� 1.5, ��������� ����� ��������
        target = GameObject.Find(TargetName);
        if (target.CompareTag("SpellBook")) target.GetComponent<SpellBook>().TakeDamage(AttackDamage);
        if (target !=null && target.layer == 7) target.GetComponent<Entity_Enemy>().TakeDamage(AttackDamage); //7 ��� EnemyLayer
    }
    public void WeaponOff() //���������� ������� �����
    {
        this.gameObject.SetActive(false);
    }
    public void MeleeDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //��������� ����������
    }
    private void Deactivate() //����������� ������� ����� ���������� �������� �������
    {
        gameObject.SetActive(false);
    }
}
