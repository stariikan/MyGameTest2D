using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_icon : MonoBehaviour
{
    private bool isStun;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.name == "Hero")
        {
            isStun = player.GetComponent<Hero>().isStun;
        }
        else
        {
            isStun = player.GetComponent<Enemy_Behavior>().isStun;
        }
        if (isStun == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
