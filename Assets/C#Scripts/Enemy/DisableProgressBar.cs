using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableProgressBar : MonoBehaviour
{
    private bool enemyDead;
    // Update is called once per frame
    void Update()
    {
        string parentTag = transform.parent.tag;
        if (parentTag == "Skeleton")
        {
            enemyDead = this.gameObject.GetComponentInParent<Entity_Enemy>().enemyDead;
        }
        if (parentTag == "Mushroom")
        {
            enemyDead = this.gameObject.GetComponentInParent<Entity_Enemy>().enemyDead;
        }
        if (parentTag == "Goblin")
        {
            enemyDead = this.gameObject.GetComponentInParent<Entity_Enemy>().enemyDead;
        }
        if (parentTag == "EvilWizard")
        {
            enemyDead = this.gameObject.GetComponentInParent<Entity_Enemy>().enemyDead;
        }
        if (parentTag == "Slime")
        {
            enemyDead = this.gameObject.GetComponentInParent<Entity_Enemy>().enemyDead;
        }
        if (parentTag == "Death")
        {
            enemyDead = this.gameObject.GetComponentInParent<Entity_Enemy>().enemyDead;
        }
        if (enemyDead == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
