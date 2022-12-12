using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float maxSpeed = 10f;
    private bool flipRight = true;
    public bool isGrounded = true;
    public Rigidbody2D rb;
    public float gravityScale = 10;
    public float fallingGravityScale = 40;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    void Start()
    {
            
    }
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        if (move > 0 && !flipRight)
        {
            Flip();
        }
        //отражение перса влево
        else if (move < 0 && flipRight)
        {
            Flip();
        }
        //прыжок, если стоит на земле
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 1000, 0));
        }
        if(rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }
        
    }

    private void Flip()
    {
        flipRight = !flipRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}

