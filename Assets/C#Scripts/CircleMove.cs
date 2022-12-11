using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    
    public bool isGrounded;
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        isGrounded = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.05f, 0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(0.05f, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 500, 0));
        }
        }
}
