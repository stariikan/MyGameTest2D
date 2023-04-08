using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance { get; set; } // To collect and send data from this script
    public float direction;// To collect and send data from this script
    private float playerHP; // a variable marking whether a projectile has hit something

    private float bombDamage = 40;
    private string enemyName;
    private GameObject enemy;
    private Animator anim;
    public Rigidbody2D rb; //Physical body
    public GameObject player; // the player game object and below is a method of how it is defined and assigned to this variable

    private void Start() //The action is performed before the start of the game and 1 time
    {
        Instance = this;
    }
    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>(); // The anim variable gets information from the Animator component (animation.Object) to which the script is bound
    }
    private void Update()
    {
        playerHP = Hero.Instance.curentHP;
    }
    private void BombMovement() // bomb flight directions and strength 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //calculate direction of movement is Player position on the x-axis - Bomb position on the x-axis
        if (directionX > 0) rb.AddForce(new Vector2(2.7f, 0.5f), ForceMode2D.Impulse);
        if (directionX < 0) rb.AddForce(new Vector2(-2.7f, 0.5f), ForceMode2D.Impulse);
 
    }
    public void BombDestroy() // deactivating the bomb object
    {
        Destroy(this.gameObject);//destroy this game object
    }
    public void BombExplosion() //activating the explosion animation
    {
        rb.velocity = Vector3.zero; //to stop the object
        anim.SetTrigger("explosion");
    }
    public void BombDmg() // damage
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //calculate direction of movement is Player position on the x-axis - Bomb position on the x-axis
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Bomb position on the y-axis
        float enemyDirectionX = enemy.transform.position.x - this.gameObject.transform.localPosition.x; // calculating the direction of travel is Enemy position along the x-axis - Bomb position along the x-axis 
        if ((Mathf.Abs(directionX) < 2.0f && Mathf.Abs(directionY) < 2f) && playerHP > 0)
        {
            Hero.Instance.GetDamage(bombDamage);
        }
        if (Mathf.Abs(enemyDirectionX) < 2f) enemy.GetComponent<Enemy_Behavior>().TakeDamage(bombDamage / 1.5f);
    }
    public void PushFromPlayer() // отскок от игрока
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //calculate direction of movement is Player position on the x-axis - Bomb position on the x-axis
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //calculate direction of movement is Player position on the y-axis - Bomb position on the y-axis
        if (Mathf.Abs(directionX) < 1f)
        {
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (theScale.x > 0) rb.AddForce(new Vector2(+2.7f, 0.5f), ForceMode2D.Impulse);
            if (theScale.x < 0) rb.AddForce(new Vector2(-2.7f, 0.5f), ForceMode2D.Impulse);
        }
    }
    public void bombDirection(Vector3 _direction)// selecting a flight direction 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //calculate direction of movement is Player position on the x-axis - Fog position on the x-axis
        this.gameObject.SetActive(true); //activate the game object
        this.gameObject.transform.position = _direction;
        BombMovement();
    }
    public void GetEnemyName(string name)
    {
        enemyName = name;
        enemy = GameObject.Find(name);
    }
}
