using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//��� UI

public class Level_counter : MonoBehaviour
{
    int level_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero
    void Update() //���������� �������� ���������� ��� ���������� ������� �����
    {
        level_ui = LvLGeneration.Instance.Level;//��� ���������� hp_ui �������� ���� ������ ���� �������
                                              //������� ������ ����� FindObjectOfType � ������� hero � ���������� hp
        GetComponent<Text>().text = $"{level_ui}"; //��� �� �������� ���������� text � game.Object � �������� � ��� ����������� ���� ������
                                                  //� ������ ����� �� ���������� ������� ������������� � string
    }
}
