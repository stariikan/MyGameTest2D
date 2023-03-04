using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable_progress_bar_Goblin : MonoBehaviour
{
    private bool enemyDead;
    // Update is called once per frame
    void Update()
    {
        enemyDead = this.gameObject.GetComponentInParent<Entity_Goblin>().enemyDead;
        if (enemyDead == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
