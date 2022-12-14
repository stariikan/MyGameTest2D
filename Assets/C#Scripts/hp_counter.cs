using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp_counter : MonoBehaviour
{
    int val;
    void Update()
    {
        val = FindObjectOfType<Hero>().hp;
        GetComponent<Text>().text = $"{val}"; 
    }
}
