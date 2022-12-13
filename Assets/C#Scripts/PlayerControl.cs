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
    // [SerializeReference] public int _state = 0;// IDLE = 0, RUN = 1, Jump = 2.
    private Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public enum States
    {
        idle,
        run,
        jump //названия как в аниматоре
    }

    [SerializeField] private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    // Update is called once per frame
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
           // _state = 1;
        }
        //отражение перса влево
        else if (move < 0 && flipRight)
        {
            Flip();
            //_state = 1;
        }
        //прыжок, если стоит на земле
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 1000, 0));
           // _state = 2;
        }
        if(rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }
        if (isGrounded) State = States.idle;
        if (Input.GetButton("Horizontal")) State = States.run;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) State = States.jump;
        if (!isGrounded) State = States.jump;

    }

    private void Flip()
    {
        flipRight = !flipRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}

