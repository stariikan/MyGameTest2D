using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public static FireBall Instance { get; set; } // To collect and send data from this script
    public float direction;//directional variable
    [SerializeField] private float Speed; //The velocity of the projectile
    [SerializeField] private float lifetime; // projectile life span
    private bool hit = false; // a variable marking whether a projectile has hit something

    public Rigidbody2D rb; //Physical body

    private BoxCollider2D boxCollider; // Magic Collider
    private Animator anim; //variable for the animator

    public int lifeTimeOfprojectile = 10; // time after which the projectile is destroyed
    public string magicTargetName; //target name of the projectile hit
    public GameObject target; //the object hit by the projectile

    private float shootingForce = 0.015f; // projectile speed

    private void Awake() //The action is performed before the start of the game and 1 time
    {
        anim = GetComponent<Animator>(); // pull information from the animator component
        boxCollider = GetComponent<BoxCollider2D>(); // pull information from the box colider component
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (hit) return; //testing a fireball hitting something
        float movementSpeed = Speed * Time.deltaTime * direction; // calculate travel speed per second and in which direction the projectile will fly
        transform.Translate(movementSpeed, 0, 0);//axis x = movementspeed, y = 0, z = 0 - all this movement along the axis x
        lifetime += Time.deltaTime; //increase the lifetime variable every second +1
        if (lifetime > lifeTimeOfprojectile) gameObject.SetActive(false);// when the variable reaches 5, the projectile disappears
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float magicDamage = Enemy_Behavior.Instance.enemyAttackDamage;
        magicTargetName = collision.gameObject.name;
        hit = true; // here we indicate that a collision has occurred
        boxCollider.enabled = false; //disconnect the collider
        anim.SetTrigger("explode");//to play the projectile attack animation when the magicAttack trigger is executed
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "PlayerCharacter") Hero.Instance.GetDamage(magicDamage); // dealing damage to a player
        magicTargetName = string.Empty;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        
        
    }
    private void Flip() //This is where we create the Flip method which, when called, reverses the direction of the sprite
    {
        Vector3 theScale = transform.localScale; //receive the scale of the object
        theScale.x *= -1;//this flips the image e.g. 140 changes to -140, thus completely changing the direction of the sprite (the image is mirrored)
        transform.localScale = theScale; // scale conversion relative to the parent GameObjects object
    }
    public void SetDirection(Vector3 shootingDirection)// selecting a flight direction 
    {
        lifetime = 0;
        this.gameObject.SetActive(true); //activate the game object
        if (shootingDirection.x == -1 && transform.localScale.x > 0) Flip();
        if (shootingDirection.x == 1 && transform.localScale.x < 0) Flip();
        boxCollider.enabled = true; //activating the collider
        hit = false; // object touched another object = false
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>(); //receiving RigidBody2D component
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(shootingDirection * shootingForce); // force application to object = directions multiplied by projectile velocity
    }
    private void Deactivate() // deactivate the projectile after the blast animation is complete
    {
        Destroy(this.gameObject);//destroy this game object
    }
}
