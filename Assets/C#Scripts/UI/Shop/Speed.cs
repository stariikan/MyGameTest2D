using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//��� UI

public class Speed : MonoBehaviour
{
    float speed_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        speed_ui = SaveSerial.Instance.playerSpeed;
        GetComponent<Text>().text = $"{speed_ui}";
    }
    private void Update()
    {
        speed_ui = SaveSerial.Instance.playerSpeed;
        GetComponent<Text>().text = $"{speed_ui}";
    }
}
