using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAttack : MonoBehaviour
{
    public Button Attack;
    private void Start()
    {
        Attack.onClick.AddListener(EmulateKeyPress);
    }

    private void EmulateKeyPress()
    {
        Input.simulateMouseWithTouches = true;
        HeroAttack.Instance.Attack();
    }
}
