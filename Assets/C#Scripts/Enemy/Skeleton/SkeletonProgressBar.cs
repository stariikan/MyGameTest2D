using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class SkeletonProgressBar : MonoBehaviour
{
    private GameObject hpBar;
    private Vector3 scaleChange, positionChange;
    
    void Awake()
    {
        hpBar = this.gameObject;
        scaleChange = new Vector3(0.1f, 0.0f, 0.0f);
        positionChange = new Vector3(0.1f, 0.0f, 0.0f);
    }
    public void UpdateEnemyProgressBar(float dmg2)
    {
        if (hpBar.transform.localScale.x > 0.01f)
        {
            hpBar.transform.localScale -= new Vector3(dmg2, 0.0f, 0.0f);
        }
        else
        {
            return;
        }
    }
    public void UpdateEnemyProgressBarPlusHP(float heal)
    {
        if (hpBar.transform.localScale.x > 0.01f)
        {
            hpBar.transform.localScale += new Vector3(heal, 0.0f, 0.0f);
        }
        else
        {
            return;
        }
    }
}
