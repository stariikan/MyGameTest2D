using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skelet : Entity
{
    // Start is called before the first frame update
    [SerializeField] private int hp = 30; //����� �������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject) //���� ������ ������������� ������ � ������ 
                                                              //(��� ���������� ������ �� ������ Hero � ������ ������� gameObject)
        {
            Hero.Instance.GetDamage(); //�� ������� Hero ���������� ��������� ����� ������� ������ ���������� hp -= 10.
            hp -= 10; //�� ��� ���� � � ������� �������� 10 ������
            Debug.Log("������ ������� 10 ������, ��������" + hp);//��������� � ����� ���������� ������ � �������
        }

        if (hp < 0)//���� hp ������ ��� ����� 0
            Die();//�� ������ � ����������� gameObject, ��� ��������� ����� �� ������� Entity  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
