using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    float cooldown;
    
    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        //if(cooldown > 50) Destroy(this.gameObject);//уничтожить этот игровой обьект
    }
}
