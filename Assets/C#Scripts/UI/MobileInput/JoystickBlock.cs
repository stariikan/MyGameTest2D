using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickBlock : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(EmulateKeyPress);
    }

    private void EmulateKeyPress()
    {
        Input.simulateMouseWithTouches = true;
        HeroAttack.Instance.Block();
        Hero.Instance.CheckBlock();
        bool block = Hero.Instance.block;
        Debug.Log(block);
    }
}
