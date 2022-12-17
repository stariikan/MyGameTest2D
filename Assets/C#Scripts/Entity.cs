using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector3 dir;
    private bool flipRight = true; //������� ������� �� �����, ��������� = ������, ����� ��� �������� ������� �� ����� ����� ��������
    public virtual void GetDamage()
    {
        
    }
    public void Flip() //��� �� ������� ����� Flip ��� ������ �������� ������ ������ �����������
    {
        flipRight = !flipRight; //����� ����������� ����� Flip ���������� flipRight �������� �� false
        Vector3 theScale = transform.localScale; //��������� ������� �������
        theScale.x *= -1;//��� ���������� ��������� ����������� �������� 140 �������� �� -140 ��� ����� ��������� ������ ����������� ������� (�������� ���������������)
        transform.localScale = theScale; //������� �������������� ������������ ������������� ������� GameObjects
    }
    public void Move() //������� ��������� ����� Move
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f); //�������� ����������� �����������
                                                                                                                                              //����� ����� �����������

        if (colliders.Length > -3) dir *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);//� ������� ������ �������
        if (dir.x > 0 && !flipRight) //���� �������� ������ ���� � �������� flipRight =�� true �� ����� ������� ����� Flip (������� �������)
        {
            Flip();
        }
        else if (dir.x < 0 && flipRight) //���� �������� ������ ���� � �������� flipRight = true �� ����� ������� ����� Flip (������� �������)
        {
            Flip();
        }
    }
    public virtual void Die() //��������� ��������� ����� Die
    {
        Destroy(this.gameObject);//���������� ���� ������� ������
    }
}
