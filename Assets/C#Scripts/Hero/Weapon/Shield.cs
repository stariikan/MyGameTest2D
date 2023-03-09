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
        if (lifetime > 1.5f)
        {
            this.gameObject.SetActive(false);//����� ���������� ��������� 1.5, ��������� ����� ��������
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        boxCollider.enabled = false; //��������� ���������
        this.gameObject.SetActive(false);//����� ���������� ��������� 1.5, ��������� ����� ��������
        target = GameObject.Find(TargetName);
        Debug.Log(target);
        if (target.CompareTag("Bomb")) target.GetComponent<Bomb>().PushFromPlayer();
        if (target.CompareTag("EvilWizard")) target.GetComponent<Enemy_Behavior>().Stun();
        if (target != null && target.layer == 7) target.GetComponent<Enemy_Behavior>().PushFromPlayer();
    }
    public void MeleeDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        lifetime = 0;
        gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //��������� ����������
    }
}
