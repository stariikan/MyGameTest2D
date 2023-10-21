using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Behavior : MonoBehaviour
{
    void Update()
    {
        float enemyHP = this.gameObject.GetComponent<Enemy_Behavior>().currentHP;
        float directionX = this.gameObject.GetComponent<Enemy_Behavior>().directionX;
        if (enemyHP > 0 && (Mathf.Abs(directionX) < 10f))
        {
            this.gameObject.GetComponent<Enemy_Behavior>().AnimState();
            this.gameObject.GetComponent<Enemy_Behavior>().EnemyMovement();
            this.gameObject.GetComponent<Enemy_Behavior>().MeleeAttack();
            this.gameObject.GetComponent<Enemy_Behavior>().GoblinAttack();
        }
    }
}