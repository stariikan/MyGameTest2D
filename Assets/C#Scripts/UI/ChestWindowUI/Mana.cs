using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//��� UI

public class Mana : MonoBehaviour
{
    float mana_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        mana_ui = SaveSerial.Instance.playerMP;
        GetComponent<Text>().text = $"{mana_ui}";
    }
    private void Update()
    {
        mana_ui = SaveSerial.Instance.playerMP;
        GetComponent<Text>().text = $"{mana_ui}";
    }
}
