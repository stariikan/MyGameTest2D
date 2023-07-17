using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_trap : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private GameObject target;
    public float trapPower;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        if (trapPower <= 0)
        {
            trapPower = 15;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject;
        Debug.Log(target);
        if (target.layer == 8)
        {
            Hero.Instance.GetDamage(trapPower);
        }
    }
}
