using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//��� UI

public class key_counter : MonoBehaviour
{
    bool key_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero
    private void ShowKey()
    {
        if (key_ui == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    void Update() //���������� �������� ���������� ��� ���������� ������� �����
    {
        key_ui = LvLGeneration.Instance.key;//��� ���������� hp_ui �������� ���� ������ ���� �������
        ShowKey();
    }
}
