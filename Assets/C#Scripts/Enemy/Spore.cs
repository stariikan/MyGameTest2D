using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    public static Spore Instance { get; set; } //to collect and send data from this script
    public float direction;//directional variable
    [SerializeField] private float lifetime; // projectile life span
    private float playerHP; // a variable marking whether a projectile has hit something

    private CircleCollider2D circleCollider; // impact collider

    private float sporeDamage = 20;
    private float sporeCooldownDmg;
    private float sporeSpeed = 1f;
    GameObject player; // the player game object and below is a method of how it is defined and assigned to this variable

    private void Start() //the action is performed before the start of the game and 1 time
    {
        player = GameObject.FindWithTag("PlayerCharacter");
        circleCollider = GetComponent<CircleCollider2D>(); // pull information from the box colider component
        Instance = this;
        playerHP = Hero.Instance.curentHP;
    }
    private void Update()
    {
        lifetime += Time.deltaTime; //increase the lifetime variable every second +1
        sporeCooldownDmg += Time.deltaTime;//cooldown attack spores
        playerHP = Hero.Instance.curentHP;
        SporeDmg();
        //SporeMovement();
        if (lifetime > 5) Destroy(this.gameObject);//destroy this game object

    }
    private void SporeMovement()
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //calculating the direction of movement is the player's position on the x-axis - fog position on the x-axis
        //int level = LvLGeneration.Instance.Level;
        if (playerHP > 0)
        {
            Vector3 pos = transform.position; //object position
            Vector3 theScale = transform.localScale; //need to understand the direction
            transform.localScale = theScale; //need to understand the direction
            float playerFollowSpeed = Mathf.Sign(directionX) * sporeSpeed * Time.deltaTime; //directional calculations
            pos.x += playerFollowSpeed; //Calculating the position along the x-axis
            transform.position = pos; //applying the position
        }
    }
    private void SporeDmg()
    {
       float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //calculating the direction of movement is the player's position on the x-axis - fog position on the x-axis
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculating the direction of movement is the player's position on the y-axis - fog position on the y-axis
        if ((Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 2f) && sporeCooldownDmg > 1 && playerHP > 0)
       {
            sporeCooldownDmg = 0;
            Hero.Instance.GetDamage(sporeDamage);
       }
    }
    public void sporeDirection(Vector3 _direction)// selecting the flight direction 
    {
        lifetime = 0;
        this.gameObject.SetActive(true); //activate the game object
        this.gameObject.transform.position = _direction;
    }
}
