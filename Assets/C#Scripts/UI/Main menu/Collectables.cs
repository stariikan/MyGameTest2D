using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectables : MonoBehaviour
{
    public GameObject rightButton; // assign target object in inspector
    public GameObject leftButton; // assign target object in inspector

    public bool stopRight;
    public bool stopLeft;

    private void Start()
    {

    }
    private void Update()
    {
        CheckCollectionPosition();
        if (transform.position.x < -423)
        {
            stopRight = true;
        }
        if (transform.position.x > -423)
        {
            stopRight = false;
        }
        if (transform.position.x > 25)
        {
            stopLeft = true;
        }
        if (transform.position.x < 25)
        {
            stopLeft = false;
        }
        //Debug.Log(transform.position.x);
    }
    public void ScrollRight()
    {
        if (transform.position.x > -424.77f)
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
    private void CheckCollectionPosition()
    {
        Image rightImageComponent = rightButton.GetComponent<Image>();
        Image leftImageComponent = leftButton.GetComponent<Image>();

        if (stopLeft)
        {
            leftImageComponent.enabled = false;
        }
        else
        {
            leftImageComponent.enabled = true;
        }
        if (stopRight)
        {
            rightImageComponent.enabled = false;
        }
        else
        {
            rightImageComponent.enabled = true;
        }
    }
}
