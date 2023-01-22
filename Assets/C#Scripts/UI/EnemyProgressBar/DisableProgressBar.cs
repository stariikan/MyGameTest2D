using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableProgressBar : MonoBehaviour
{
    private bool enemyDead;
    // Update is called once per frame
    void Update()
    {
        enemyDead = this.gameObject.GetComponentInParent<Entity>().enemyDead;
        if (enemyDead == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
