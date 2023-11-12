using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrolCheck : MonoBehaviour
{
    public GameObject masterOfPoint; //For identifying the player on the scene
    private float direction;
    private string objectName; //Name of the debug object
    // Start is called before the first frame update
    void Start()
    {
        objectName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        //VisualFormForDebug();
    }
    private void VisualFormForDebug()
    {
        Vector3 pos = transform.position;
        Vector3 theScale = transform.localScale;
        if (objectName == "PatrolLeft")
        {
            direction = masterOfPoint.GetComponent<Enemy_Behavior>().patrolDirectionLeft;

            pos.x = direction; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
        }
        if (objectName == "PatrolRight")
        {
            direction = masterOfPoint.GetComponent<Enemy_Behavior>().patrolDirectionRight;
            pos.x = direction; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
        }
        if (objectName == "sightDistanceLeft")
        {
            direction = masterOfPoint.GetComponent<Enemy_Behavior>().sightDistanceLeft;
            pos.x = direction; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
        }
        if (objectName == "AttackDistance")
        {
            direction = masterOfPoint.GetComponent<Enemy_Behavior>().attackDistance;
            float scaleDirection = direction / 5.8f;
            theScale.x = scaleDirection; //Calculating
            theScale.y = scaleDirection; //Calculating
            transform.localScale = theScale; //Apply
        }
        if (objectName == "sightDistance")
        {
            direction = masterOfPoint.GetComponent<Enemy_Behavior>().sightDistance;
            float scaleDirection = direction / 5.8f;
            theScale.x = scaleDirection; //Calculating
            theScale.y = scaleDirection; //Calculating
            transform.localScale = theScale; //Apply
        }
        if (objectName == "PatrolDistance")
        {
            direction = masterOfPoint.GetComponent<Enemy_Behavior>().patrolDistance;
            float scaleDirection = direction / 5.8f;
            theScale.x = scaleDirection; //Calculating
            theScale.y = scaleDirection; //Calculating
            transform.localScale = theScale; //Apply

            Vector3 position = masterOfPoint.GetComponent<Enemy_Behavior>().startPosition;
            transform.position = position;
        }
    }
}
