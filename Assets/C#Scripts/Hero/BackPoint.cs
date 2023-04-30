using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

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
            direction.x = theHeroPosition.x - 1.565f;
            transform.position = direction;
        }
        if (playerDirection == -1)
        {
            direction.x = theHeroPosition.x + 1.565f;
            transform.position = direction;
        }
    }
}
