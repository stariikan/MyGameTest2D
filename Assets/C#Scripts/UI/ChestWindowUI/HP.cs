using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//��� UI

public class HP : MonoBehaviour
{
    float hp_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        hp_ui = SaveSerial.Instance.playerHP;
        GetComponent<Text>().text = $"{hp_ui}";
    }
    private void Update()
    {
        hp_ui = SaveSerial.Instance.playerHP;
        GetComponent<Text>().text = $"{hp_ui}";
    }
}
