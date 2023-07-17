using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Item_heal : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private GameObject target;
    public float healPower;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        if (healPower <= 0)
        {
            healPower = 15;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject;
        Debug.Log(target);
        if (target.layer == 8)
        {
            Hero.Instance.HealFrom(healPower);
            Destroy(this.gameObject);
        }
    }
}
