using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public int how_many_key_need;
    public bool door_open = false;
    bool player_key;
    public static door Instance { get; set; } //��� ����� � �������� ������ �� ����� �������
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = "Door" + Random.Range(1, 999);
        Instance = this;
        if (how_many_key_need == 0)
        {
            how_many_key_need = 1;
        }
    }
    void Update()
    {
        player_key = LvLGeneration.Instance.key;
        TryToOpen();
        OpenDoor();
    }
    public void TryToOpen() //����� ��� ��������� ������ ��� (int dmg) ��� �������� ����� ����� ������� ��� ������ ������ (�� ���� ���� ����� ����� ������� ����)
    {
        if (player_key == true)
        {
            door_open = true;
            LvLGeneration.Instance.UseKey();
            Debug.Log("Door open!");
        }
        else
        {
            //Info.Instance.infoNeedKey();
            return;
        }
    }
    public void OpenDoor()
    {
        if (door_open == true) 
        {
            Destroy(this.gameObject);
        }
    }
}
