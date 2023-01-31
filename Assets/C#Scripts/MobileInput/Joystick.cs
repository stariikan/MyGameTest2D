using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer ||
    Application.platform == RuntimePlatform.LinuxPlayer) // PC platform
        {
            Debug.Log("PC platform");
            this.gameObject.SetActive(false);
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) // Mobile platform
        {
            Debug.Log("Mobile Platform");
            
        }
        else //Unbity editor
        {
            Debug.Log("Unity platform");
            //this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
