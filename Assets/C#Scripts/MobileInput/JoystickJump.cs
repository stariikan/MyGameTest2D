using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickJump : MonoBehaviour
{
    public Button Jump;
    private void Start()
    {
        Jump.onClick.AddListener(EmulateKeyPress);
    }

    private void EmulateKeyPress()
    {
        Input.simulateMouseWithTouches = true;
        Hero.Instance.Push();
    }
}
