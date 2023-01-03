using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Projectile Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    public float direction;//���������� �����������
    [SerializeField] private float Speed; //�������� �������
    [SerializeField] private float lifetime; //������������ ����� �������
    private bool hit = false; //���������� ����� ����� �� �� ���-�� ������
    
    private BoxCollider2D boxCollider; //��������� �����
    private Animator anim; //���������� ��� ���������

    public int magicAttackDamage = 30;
    public string magicTargetName;
    public GameObject target;

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
    }
    private void Update()
    {
        
        if (hit) return; //�������� ��������� �������� ���� �� ���-������
        float movementSpeed = Speed * Time.deltaTime * direction; // ���������� �������� ����������� � ������� � � ����� ����������� ������� ������
        transform.Translate(movementSpeed, 0, 0);//��� � = movementspeed, y = 0, z=0 - ��� ��� ����������� �� ��� x
        lifetime += Time.deltaTime; //���������� ���������� lifetime ������ ��� +1
        if (lifetime > 5) gameObject.SetActive(false);//����� ���������� ��������� 5, ������ ��������
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        magicTargetName = collision.gameObject.name;
        hit = true; //��� ��������� ��� ��������� ������������
        boxCollider.enabled = false; //��������� ���������
        anim.SetTrigger("explode");//��� ��������������� �������� ����� �������� ��� ���������� ������� magicAttack
        DamageObject();
        magicTargetName = string.Empty;
        //Deactivate();
    }
    public void DamageObject()
    {
        Debug.Log(magicTargetName);
        target = GameObject.Find(magicTargetName);
        if (target.CompareTag("Enemy"))
            {
            target.GetComponent<Entity>().TakeDamage(magicAttackDamage);
            }
        else 
            {
            return;
            }
    }
    public void SetDirection(float _direction)// ����� ����������� ������ 
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
