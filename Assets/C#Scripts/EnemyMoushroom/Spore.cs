using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    public static Spore Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float lifetime; //������������ ����� �������
    private bool hit = false; //���������� ����� ����� �� �� ���-�� ������

    private BoxCollider2D boxCollider; //��������� �����

    private int sporeDamage = 10;
    private float sporeCooldownDmg;
    public string TargetName;
    public GameObject target;


    private void Awake() //�������� ����������� �� ������ ���� � 1 ���
    {
        //anim = GetComponent<Animator>(); // ����������� ���������� �� ���������� ��������
        boxCollider = GetComponent<BoxCollider2D>(); // ����������� ���������� �� ���������� ���� ��������
        Instance = this;
    }

    private void Update()
    {
        lifetime += Time.deltaTime; //���������� ���������� lifetime ������ ��� +1
        sporeCooldownDmg += Time.deltaTime;//������� ����� ����
        if (lifetime > 10) this.gameObject.SetActive(false);//����� ���������� ��������� 5, ��������� ����� ��������
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        hit = true; //��� ��������� ��� ��������� ������������
        if(sporeCooldownDmg > 1)
        {
            sporeCooldownDmg = 0;
            Hero.Instance.GetDamage(sporeDamage);
        }
        TargetName = string.Empty;
    }
    public void sporeDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        lifetime = 0;
        this.gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //��������� ���������� 
        hit = false; //������ �������� ������� ������� = false
    }
}
