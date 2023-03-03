using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    public void ScrollRight()
    {
        if(transform.position.x > -424.77f)
        {
            transform.position += new Vector3(-15f, 0f, 0f);
        }
        
    }
    public void ScrollLeft()
    {
        if (transform.position.x < 25f)
        {
            transform.position += new Vector3(15f, 0f, 0f);
        }      
    }
}
