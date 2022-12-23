using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHP = 100;
    public static Entity Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    int currentHP;

    private Animator anim;

    private void Start()
    {
        currentHP = maxHP;
        Instance = this;
        anim = GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
                                         //� �������� �������� ������
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        anim.SetTrigger("damage");//�������� ��������� ��������
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die() //��������� ��������� ����� Die
    {
        //anim.SetTrigger("death");//�������� ������
        Debug.Log("Enemy Defeat");
        Destroy(this.gameObject);//���������� ���� ������� ������
    }

    private void Update()
    {
        //Debug.Log(currentHP);
    }

}
