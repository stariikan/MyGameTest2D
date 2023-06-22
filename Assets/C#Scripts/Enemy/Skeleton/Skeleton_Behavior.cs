using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Behavior : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float enemyHP = this.gameObject.GetComponent<Enemy_Behavior>().currentHP;
        if (enemyHP > 0)
        {
            this.gameObject.GetComponent<Enemy_Behavior>().AnimState();
            this.gameObject.GetComponent<Enemy_Behavior>().EnemyMovement();
            this.gameObject.GetComponent<Enemy_Behavior>().MeleeAttack();
            this.gameObject.GetComponent<Enemy_Behavior>().SkeletonAttack();
        }
    }
}
