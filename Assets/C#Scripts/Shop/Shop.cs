using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    int intToSave;
    float floatToSave;
    string stringToSave = "";
    void OnGUI()
    {
        if (GUI.Button(new Rect(275, 50, 125, 50), "Raise Integer"))
            intToSave++;
        if (GUI.Button(new Rect(275, 150, 125, 50), "Raise Float"))
            floatToSave += 0.1f;
        if (GUI.Button(new Rect(275, 250, 125, 50), "Raise Float"))
            floatToSave += 0.1f;
        if (GUI.Button(new Rect(275, 350, 125, 50), "Raise Float"))
            floatToSave += 0.1f;
        if (GUI.Button(new Rect(275, 450, 125, 50), "Raise Float"))
            floatToSave += 0.1f;

        GUI.Label(new Rect(425, 65, 125, 50), "Integer value is "
          + intToSave);
        GUI.Label(new Rect(425, 165, 125, 50), "Float value is "
          + floatToSave.ToString("F1"));
        GUI.Label(new Rect(425, 265, 125, 50), "String value is "
          + stringToSave);
        GUI.Label(new Rect(425, 365, 125, 50), "String value is "
  + stringToSave);
        GUI.Label(new Rect(425, 465, 125, 50), "String value is "
  + stringToSave);

    }

}

