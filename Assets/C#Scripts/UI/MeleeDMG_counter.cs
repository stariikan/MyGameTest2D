using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class MeleeDMG_counter : MonoBehaviour
{
    int meleeDMG_ui;

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        meleeDMG_ui = SaveSerial.Instance.playerAttackDamage;
        GetComponent<Text>().text = $"{meleeDMG_ui}";
    }
    private void Update()
    {
        meleeDMG_ui = SaveSerial.Instance.playerAttackDamage;
        GetComponent<Text>().text = $"{meleeDMG_ui}";
    }
}
