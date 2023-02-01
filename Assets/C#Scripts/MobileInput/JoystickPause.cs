using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickPause : MonoBehaviour
{
    public Button button;
    public bool isPause;
    private void Start()
    {
        button.onClick.AddListener(EmulateKeyPress);
    }

    private void EmulateKeyPress()
    {
        Input.simulateMouseWithTouches = true;
        isPause = Pause.Instance.ispause;
        if (isPause == true)
        {
            Pause.Instance.PauseOFF();
        }
        else
        {
            Pause.Instance.PauseON();
        }
    }
}
