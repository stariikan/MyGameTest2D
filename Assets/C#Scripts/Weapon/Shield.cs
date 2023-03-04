using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static Shield Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float lifetime; //������������ ����� �������

    private BoxCollider2D boxCollider; //��������� �����

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
        if (lifetime > 1)
        {
            gameObject.SetActive(false);//����� ���������� ��������� 1.5, ��������� ����� ��������
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        boxCollider.enabled = false; //��������� ���������
        this.gameObject.SetActive(false);//����� ���������� ��������� 1.5, ��������� ����� ��������
        target = GameObject.Find(TargetName);
        if (target.CompareTag("Skeleton"))
        {
            target.GetComponent<Enemy_Skeleton>().PushFromPlayer();
        }
        if (target.CompareTag("Mushroom"))
        {
            target.GetComponent<Enemy_Mushroom>().PushFromPlayer();
        }
        if (target.CompareTag("Goblin"))
        {
            target.GetComponent<Enemy_Goblin>().PushFromPlayer();
        }
        if (target.CompareTag("Bomb"))
        {
            target.GetComponent<Bomb>().PushFromPlayer();
        }
    }
    public void MeleeDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        lifetime = 0;
        gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //��������� ����������
    }
}