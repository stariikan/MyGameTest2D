using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{

    private float maxHP = 1; //������������ ����� �������
    public float currentHP;
    public static SpellBook Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    private Animator anim;
    public bool chestOpen = false;
    public int rewardForKill = 20;//������� �� ������ ��� ������
    public enum States //����������� ����� ������ ���������, ������ �������� ��� � ��������� Unity
    {
        idle,
        open
    }
    private States State //�������� �����������, ���������� = State. �������� ��������� ����� ���� �������� ��� �������� ����� ��������� get � set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    private void Start()
    {
        currentHP = maxHP;
        Instance = this;
        chestOpen = false;
        anim = GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object)
                                         //� �������� �������� ������
    }
    public void TakeDamage(float dmg) //����� ��� ��������� ������ ��� (int dmg) ��� �������� ����� ����� ������� ��� ������ ������ (�� ���� ���� ����� ����� ������� ����)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("open");//�������� ��������� ��������
            currentHP -= dmg;
            Debug.Log(currentHP + " " + gameObject.name);
        }
        else
        {
            return;
        }

        if (currentHP <= 0)
        {
            LvLGeneration.Instance.PlusCoin(rewardForKill); //����� ������ ��� ���������� �����
            anim.SetTrigger("open");//�������� ������
            chestOpen = true;
            Debug.Log("Open" + gameObject.name);
        }
    }
    public virtual void Die() //����� ������� ���� ������� ������, ���������� ����� �������� ����� ����� ���������� �������� ������
    {
        Destroy(this.gameObject); ;//���������� ���� ������� ������
    }
}
