using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : Enemy_Behavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }
    private void EnemyMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of movement is Player position on the x-axis - Enemy position on the x-axis
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Enemy position on the y-axis

        patrolDirectionLeft = startPosition.x - patrolDistance;
        patrolDirectionRight = startPosition.x + patrolDistance;

        sightDistanceLeft = transform.position.x - sightDistance;
        sightDistanceRight = transform.position.x + sightDistance;

        bool patrol = false;
        bool follow = false;

        Vector3 pos = transform.position; //object position
        Vector3 theScale = transform.localScale; // needed to understand the direction

        if (isFlying && !isAttack && !inAttackState)
        {
            Jump();
        }
        if (patrolDirectionRight < transform.position.x) patrolFlip = 2;
        if (patrolDirectionLeft > transform.position.x) patrolFlip = 1;

        if (Mathf.Abs(directionX) > sightDistance && !isAttack && !stuned && !isAttacked && alarmPatrolTimer > alarmPause && !inAttackState)
        {
            alarmFollowTimer = 0;
            if (patrolDirectionLeft != transform.position.x && patrolFlip == 1)
            {
                float patrolSpeed = 1 * enemySpeed * Time.deltaTime; //calculating direction
                pos.x += patrolSpeed; //Calculating the position along the x-axis
                if (patrolSpeed > 0 && transform.localScale.x > 0)
                {
                    transform.position = pos; //applying the position
                    patrol = true;
                }

                if (patrolSpeed < 0 && transform.localScale.x > 0) Flip();
                else if (patrolSpeed > 0 && transform.localScale.x < 0) Flip();
            }

            if (patrolDirectionRight != transform.position.x && patrolFlip == 2)
            {
                float patrolSpeed = -1 * enemySpeed * Time.deltaTime; //calculating direction
                pos.x += patrolSpeed; //Calculating the position along the x-axis
                if (patrolSpeed < 0 && transform.localScale.x < 0)
                {
                    transform.position = pos; //applying the position
                    patrol = true;
                }


                if (patrolSpeed < 0 && transform.localScale.x > 0) Flip();
                else if (patrolSpeed > 0 && transform.localScale.x < 0) Flip();
            }
        }
        if (Mathf.Abs(directionX) < sightDistance && Mathf.Abs(directionX) >= attackDistance && !isAttack && !stuned && !playerGodMode && !inAttackState && alarmFollowTimer > alarmPause || isAttacked && Mathf.Abs(directionX) > attackDistance && !block && !isAttack && !stuned && !playerGodMode && !inAttackState || copy && !playerGodMode && !inAttackState)
        {
            alarmPatrolTimer = 0;
            transform.localScale = theScale; // needed to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * enemySpeed * Time.deltaTime; //calculating direction

            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();// if movement is greater than zero and flipRight = not true, then the Flip method must be called (sprite rotation)

            pos.x += playerFollowSpeed; //Calculating the position along the x-axis

            if (playerFollowSpeed < 0 && theScale.x < 0)
            {
                transform.position = pos; //applying the position
                follow = true;
            }
            if (playerFollowSpeed > 0 && theScale.x > 0)
            {
                transform.position = pos; //applying the position
                follow = true;
            }
        }
        if (patrol || follow)
        {
            movement = true;
        }
        else
        {
            movement = false;
        }
    }
}
