using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFront : MonoBehaviour
{
    private float cooldownTimer = Mathf.Infinity;
    private CapsuleCollider2D capsuleCollider;
    public Vector3 frontPosition;

    public static BodyFront Instance { get; set; } // To collect and send data from this script
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    private void FixedUpdate()
    {
        cooldownTimer += Time.deltaTime; // add 1 second to cooldownTimer
        if (cooldownTimer > 1f) capsuleCollider.enabled = true;
        frontPosition = transform.position;
    }
    public void ColliderOFF()
    {
        cooldownTimer = 0f;
        capsuleCollider.enabled = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        string targetName = collision.gameObject.name;
        GameObject target = GameObject.Find(targetName);
        if (target != null && target.layer == 7)
        {
            Hero.Instance.GetDamage(1); //7 this is the Enemy layer
            Debug.Log("trigerHit");
            Hero.Instance.Push();
        }
    }
}
