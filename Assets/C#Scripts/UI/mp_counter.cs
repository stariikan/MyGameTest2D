using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//��� UI

public class mp_counter : MonoBehaviour
{
    int mp_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero
    void Update() //���������� �������� ���������� ��� ���������� ������� �����
    {
        mp_ui = HeroAttack.Instance.currentMP;//��� ���������� hp_ui �������� ���� ������ ���� �������
        GetComponent<Text>().text = $"{mp_ui}"; //��� �� �������� ���������� text � game.Object � �������� � ��� ����������� ���� ������
                                                //� ������ ����� �� ���������� ������� ������������� � string
    }
}
