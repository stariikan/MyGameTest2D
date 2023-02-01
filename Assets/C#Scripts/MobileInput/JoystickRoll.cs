using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickRoll : MonoBehaviour
{
    public Button roll;

    private void Start()
    {
        roll.onClick.AddListener(EmulateKeyPress);
    }

    private void EmulateKeyPress()
    {
        Input.simulateMouseWithTouches = true;
        Hero.Instance.Roll();
    }
}
