using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skelet : Entity
{
    // Start is called before the first frame update
    [SerializeField] private int hp = 30;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            hp -= 10;
            Debug.Log("Скелет потерял 10 жизней, осталось" + hp);
        }

        if (hp < 0)
            Die();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
