using System.Collections;
using UnityEngine;

public class SummonSlime : MonoBehaviour
{
    public GameObject[] guards;
    public float direction;//���������� �����������
    public Rigidbody2D rb; //���������� ����
    private Animator anim; //���������� ��������� ������� ���������� ������
    public static SummonSlime Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    private void Start()
    {
        Instance = this;
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //���������� rb �������� ��������� Rigidbody2D (������ game.Object) � �������� �������� ������
        anim = this.gameObject.GetComponent<Animator>(); //���������� anim �������� ���������� �� ���������� Animator (�������� game.Object) � �������� �������� ������
    }
    public void SummonGuards()
    {
        Vector3 pos = transform.position;
        GameObject guard1 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 1.5f, pos.y, pos.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
        guard1.name = "Enemy" + Random.Range(1, 999);
        GameObject guard2 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 1f, pos.y, pos.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
        guard2.name = "Enemy" + Random.Range(1, 999);
        GameObject guard3 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 2f, pos.y, pos.z), Quaternion.identity); //������������ ������� (����) � ��� ����������)
        guard3.name = "Enemy" + Random.Range(1, 999);

    }
    public void SummonDirection(Vector3 _direction)// ����� ����������� ������ 
    {
        this.gameObject.SetActive(true); //��������� �������� �������
        this.gameObject.transform.position = _direction;
        anim.SetTrigger("summon");
    }
    public void SummonOff()
    {
        this.gameObject.SetActive(false);
    }
}
