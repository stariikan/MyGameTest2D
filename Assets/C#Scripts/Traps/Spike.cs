using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int trapDmg = 30;
    private void OnCollisionEnter2D(Collision2D collision) //���� ���������� ��������������� ���
    { 
       if (collision.gameObject == Hero.Instance.gameObject)//���� ������� ������������� ������ � ������
                                                            //(��� ���������� ������ �� ������ Hero � ������ ������� gameObject)
        {
            Hero.Instance.GetDamage(trapDmg);//�� ������� Hero ���������� ��������� ����� ������� ������ ���������� hp -= 10.
        }
    }

}
