using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp_counter : MonoBehaviour
{
    int hp_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero

    void Update() //���������� �������� ���������� ��� ���������� ������� �����
    {
        hp_ui = Hero.Instance.hp;//��� ���������� hp_ui �������� ���� ������ ���� �������
                                          //������� ������ ����� FindObjectOfType � ������� hero � ���������� hp
        GetComponent<Text>().text = $"{hp_ui}"; //��� �� �������� ���������� text � game.Object � �������� � ��� ����������� ���� ������
                                              //� ������ ����� �� ���������� val ������� ������������� � string
    }
}
