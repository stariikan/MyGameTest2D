using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//��� UI

public class Coin : MonoBehaviour
{
    int coin_ui; //��� ������� ���������� ����� ��� ����� �������� �������� ���������� hp �� ������� Hero

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        coin_ui = SaveSerial.Instance.playerCoin;
        GetComponent<Text>().text = $"{coin_ui}";
    }
    private void Update()
    {
        GetComponent<Text>().text = $"{coin_ui}";
    }
}
