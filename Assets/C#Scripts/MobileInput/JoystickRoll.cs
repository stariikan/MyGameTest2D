using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickRoll : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(EmulateKeyPress);
    }

    private void EmulateKeyPress()
    {
        Input.simulateMouseWithTouches = true;
        KeyCode keyToEmulate = KeyCode.LeftControl;
        Input.GetKeyDown(keyToEmulate);
        Input.GetKeyUp(keyToEmulate);
    }
}
