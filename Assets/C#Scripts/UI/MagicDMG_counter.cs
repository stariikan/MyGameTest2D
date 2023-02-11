using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class MagicDMG_counter : MonoBehaviour
{
    int magicDMG_ui;

    // Start is called before the first frame update
    void Start()
    {
        SaveSerial.Instance.LoadGame();
        magicDMG_ui = SaveSerial.Instance.playerMageDamage;
        GetComponent<Text>().text = $"{magicDMG_ui}";
    }
    private void Update()
    {
        magicDMG_ui = SaveSerial.Instance.playerMageDamage;
        GetComponent<Text>().text = $"{magicDMG_ui}";
    }
}
