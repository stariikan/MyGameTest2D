using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float direction;//���������� �����������
    [SerializeField] private float Speed; //�������� �������
    [SerializeField] private float lifetime; //������������ ����� �������
    private bool hit; //���������� ����� ����� �� �� ���-�� ������
    
    private BoxCollider2D boxCollider; //��������� �����
    private Animator anim; //���������� ��� ���������

    private void Awake() //�������� ����������� �� ������ ���� � 1 ���
    {
        anim = GetComponent<Animator>(); // ����������� ���������� �� ���������� ��������
        boxCollider = GetComponent<BoxCollider2D>(); // ����������� ���������� �� ���������� ���� ��������
    }

    private void Update()
    {
        if (hit) return; //�������� ��������� �������� ���� �� ���-������
        float movementSpeed = Speed * Time.deltaTime * direction; // ���������� �������� ����������� � ������� � � ����� ����������� ������� ������
        transform.Translate(movementSpeed, 0, 0);//��� � = movementspeed, y = 0, z=0 - ��� ��� ����������� �� ��� x

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true; //��� ��������� ��� ��������� ������������
        boxCollider.enabled = false; //��������� ���������
        anim.SetTrigger("explode");//��� ��������������� �������� ����� �������� ��� ���������� ������� magicAttack
        if (collision.gameObject == Enemy_Skelet.Instance.gameObject) //���� ������ ������������� ������ � ������ 
                                                                        //(��� ���������� ������ �� ������ Hero � ������ ������� gameObject)
        {
            Enemy_Skelet.Instance.Damaged(); //�� ������� Hero ���������� ��������� ����� ������� ������ ���������� hp -= 10.         
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
