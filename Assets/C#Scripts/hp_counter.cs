using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp_counter : MonoBehaviour
{
    int val; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero
    void Update() //���������� �������� ���������� ��� ���������� ������� �����
    {
        val = FindObjectOfType<Hero>().hp;//��� ���������� val �������� ���� ������ ���� �������
                                          //������� ������ ����� FindObjectOfType � ������� hero � ���������� hp
        GetComponent<Text>().text = $"{val}"; //��� �� �������� ���������� text � game.Object � �������� � ��� ����������� ���� ������
                                              //� ������ ����� �� ���������� val ������� ������������� � string
    }
}
