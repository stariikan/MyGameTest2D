using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainHP : MonoBehaviour
{
    
    public float direction;//directional variable
    private float playerHP; // a variable marking whether a projectile has hit something

    private float drainHPDamage = 15f;

    GameObject player; // the player game object and below is a method of how it is defined and assigned to this variable
    public Rigidbody2D rb; //Physical body
    private Animator anim; //Variable by which the object is animated
    private BoxCollider2D boxCollider; // Magic Collider
    public static DrainHP Instance { get; set; } // To collect and send data from this script
    private void Start()
    {
        Instance = this;
        player = GameObject.FindWithTag("PlayerCharacter");
        rb = this.gameObject.GetComponent<Rigidbody2D>(); // The rb variable gets the Rigidbody2D component (physics.Object) to which the script is bound
        anim = this.gameObject.GetComponent<Animator>(); // The anim variable gets information from the Animator component (animation.Object) to which the script is bound
        boxCollider = GetComponent<BoxCollider2D>();
        playerHP = Hero.Instance.curentHP;
    }
    private void Update()
    {
        playerHP = Hero.Instance.curentHP;
    }
    public void DrainHPDmg()
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //calculate direction of movement is Player position on the x-axis - Skeleton position on the x-axis
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Skeleton position on the y-axis
        if (Mathf.Abs(directionX) < 1f && Mathf.Abs(directionY) < 2f && playerHP > 0) 
        {
            Hero.Instance.GetDamage(drainHPDamage);
            GameObject[] deathObjects = GameObject.FindGameObjectsWithTag("Death");
            foreach (GameObject obj in deathObjects)
            {
                if (obj.name != "BossDeath")
                {
                    obj.GetComponent<Entity_Enemy>().BossDeathHeal(50);
                }
            }
        }
    }
    public void DrainHPDirection(Vector3 _direction)// selecting a flight direction 
    {
        this.gameObject.SetActive(true); //activate the game object
        this.gameObject.transform.position = _direction;
        anim.SetTrigger("drain_hp");
    }
    public void DrainHPOff()
    {
        this.gameObject.SetActive(false);
    }
}
