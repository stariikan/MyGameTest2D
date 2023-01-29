using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//��� UI

public class Info : MonoBehaviour
{
    public static Info Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    string text; // ����� ������� ����� ���������
    private float CooldownTimer = Mathf.Infinity;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this; //'this' - ��� �������� �����, ������������ �����, � ������� ����������� ���.
    }
    public void infoNeedKey() 
    {
        GetComponent<Text>().text = "Need key!";
        CooldownTimer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        CooldownTimer += Time.deltaTime;
        
        if (CooldownTimer > 4)
        {
            GetComponent<Text>().text = " ";
        }

    }
}
