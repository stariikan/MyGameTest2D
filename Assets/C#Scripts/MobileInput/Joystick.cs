using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    private Transform joystick;
    public int platform; //PC = 1, AN = 2, Editor = 0
    public static Joystick Instance { get; set; }
    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer ||
    Application.platform == RuntimePlatform.LinuxPlayer) // PC platform
        {
            Debug.Log("PC platform");
            this.gameObject.SetActive(false);
            platform = 1;
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) // Mobile platform
        {
            Debug.Log("Mobile Platform");
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.Landscape;
            platform = 2;

        }
        else //Unbity editor
        {
            Debug.Log("Unity platform");
            platform = 0;
            //this.gameObject.SetActive(false);
        }
    }
}
