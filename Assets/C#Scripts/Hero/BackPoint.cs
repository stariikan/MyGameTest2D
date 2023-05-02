using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackPoint : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 theHeroPosition;
        Vector3 direction = transform.position;
        int playerDirection;
        theHeroPosition = Hero.Instance.transform.position;
        playerDirection = Hero.Instance.m_facingDirection;
        if (playerDirection == 1)
        {
            direction.x = theHeroPosition.x - 1;
            transform.position = direction;
        }
        if (playerDirection == -1)
        {
            direction.x = theHeroPosition.x + 1;
            transform.position = direction;
        }
    }
}
