
using UnityEngine;
using UnityEngine.UI;//��� UI

public class coin_counter : MonoBehaviour
{
    int coin_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero
    void Update() //���������� �������� ���������� ��� ���������� ������� �����
    {
        coin_ui = LvLGeneration.Instance.coin;//��� ���������� hp_ui �������� ���� ������ ���� �������
                                 //������� ������ ����� FindObjectOfType � ������� hero � ���������� hp
        GetComponent<Text>().text = $"{coin_ui}"; //��� �� �������� ���������� text � game.Object � �������� � ��� ����������� ���� ������
                                                //� ������ ����� �� ���������� ������� ������������� � string
    }
}
