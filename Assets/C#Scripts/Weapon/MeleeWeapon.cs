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

    public int AttackDamage = 10;
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
            AttackDamage = 10;
        }
    }

    private void Update()
    {
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
        if (target.CompareTag("Mushroom"))
        {
            target.GetComponent<Entity_Mushroom>().TakeDamage(AttackDamage);
        }
        else if (target.CompareTag("Chest"))
        {
            target.GetComponent<Chest>().TakeDamage(AttackDamage);
        }
        else if (target.CompareTag("Door"))
        {
            target.GetComponent<door>().TryToOpen();
        }
        else
        {
            return;
        }
    }
    public void meleeDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        lifetime = 0;
        gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //��������� ���������� 
        hit = false; //������ �������� ������� ������� = false
    }
    private void Deactivate() //����������� ������� ����� ���������� �������� �������
    {
        gameObject.SetActive(false);
    }
}
