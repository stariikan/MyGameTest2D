using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public static FireBall Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float Speed; //�������� �������
    [SerializeField] private float lifetime; //������������ ����� �������
    private bool hit = false; //���������� ����� ����� �� �� ���-�� ������

    public Rigidbody2D rb; //���������� ����

    private BoxCollider2D boxCollider; //��������� �����
    private Animator anim; //���������� ��� ���������

    public int lifeTimeOfprojectile = 10; //����� ����� �������� ������ ������������
    public string magicTargetName; //��� ���� �� �������� ����� ������
    public GameObject target; //������ �� �������� ����� ������

    private float shootingForce = 0.015f; //�������� �������

    private void Awake() //�������� ����������� �� ������ ���� � 1 ���
    {
        anim = GetComponent<Animator>(); // ����������� ���������� �� ���������� ��������
        boxCollider = GetComponent<BoxCollider2D>(); // ����������� ���������� �� ���������� ���� ��������
        Instance = this;
    }

    private void Start()
    {
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
        float magicDamage = Entity_Enemy.Instance.wizardAttackDamage;
        magicTargetName = collision.gameObject.name;
        hit = true; //��� ��������� ��� ��������� ������������
        boxCollider.enabled = false; //��������� ���������
        anim.SetTrigger("explode");//��� ��������������� �������� ����� �������� ��� ���������� ������� magicAttack
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "PlayerCharacter") Hero.Instance.GetDamage(magicDamage); //��������� ����� ������
        magicTargetName = string.Empty;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        
        
    }
    private void Flip() //��� �� ������� ����� Flip ��� ������ �������� ������ ������ �����������
    {
        Vector3 theScale = transform.localScale; //��������� ������� �������
        theScale.x *= -1;//��� ���������� ��������� ����������� �������� 140 �������� �� -140 ��� ����� ��������� ������ ����������� ������� (�������� ���������������)
        transform.localScale = theScale; //������� �������������� ������������ ������������� ������� GameObjects
    }
    public void SetDirection(Vector3 shootingDirection)// ����� ����������� ������ 
    {
        lifetime = 0;
        this.gameObject.SetActive(true); //��������� �������� �������
        if (shootingDirection.x == -1 && transform.localScale.x > 0) Flip();
        if (shootingDirection.x == 1 && transform.localScale.x < 0) Flip();
        boxCollider.enabled = true; //��������� ����������
        hit = false; //������ �������� ������� ������� = false
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>(); //��������� ���������� RigidBody2D
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(shootingDirection * shootingForce); //���������� ���� � ������� = ����������� ��������� �� �������� �������
    }
    private void Deactivate() //����������� ������� ����� ���������� �������� �������
    {
        Destroy(this.gameObject);//���������� ���� ������� ������
    }
}
