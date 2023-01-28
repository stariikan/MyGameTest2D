using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float lifetime; //������������ ����� �������
    private bool hit = false; //���������� ����� ����� �� �� ���-�� ������

    private BoxCollider2D boxCollider; //��������� �����
    //private Animator anim; //���������� ��� ���������

    public int AttackDamage = 20;
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
            AttackDamage = 20;
        }
    }

    private void Update()
    {
        if (hit) return; //�������� ��������� ��������� �� ���-������
        lifetime += Time.deltaTime; //���������� ���������� lifetime ������ ��� +1
        if (lifetime > 1) gameObject.SetActive(false);//����� ���������� ��������� 1.5, ��������� ����� ��������
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        hit = true; //��� ��������� ��� ��������� ������������
        boxCollider.enabled = false; //��������� ���������
        meleeDamageObject();
        TargetName = string.Empty;
    }
    public void meleeDamageObject()
    {
        Debug.Log(TargetName);
        target = GameObject.Find(TargetName);
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<Entity>().TakeDamage(AttackDamage);
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
    public void meleeDirection(float _direction)// ����� ����������� ������ 
    {
        lifetime = 0;
        gameObject.SetActive(true); //��������� �������� �������
        direction = _direction;
        boxCollider.enabled = true; //��������� ���������� 
        hit = false; //������ �������� ������� ������� = false
        float localScaleX = transform.localScale.x; //���� ���� ��� ��� �� ����� ������� x �� -x � ����������� � ����� ������� �� ��������, ������ ��� ��������� ������� 
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);//����� ����������� �������
    }
    private void Deactivate() //����������� ������� ����� ���������� �������� �������
    {
        gameObject.SetActive(false);
    }
}
