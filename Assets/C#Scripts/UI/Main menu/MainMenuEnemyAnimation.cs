using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class MainMenuEnemyAnimation : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        this.gameObject.GetComponent<Animator>().SetInteger("state", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
