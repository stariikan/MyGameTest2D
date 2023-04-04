using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Projectile Instance { get; set; } // To collect and send data from this script
    public float direction;//directional variable
    [SerializeField] private float Speed;  //The velocity of the projectile
    [SerializeField] private float lifetime; // projectile life span
    private bool hit = false; // a variable marking whether a projectile has hit something

    public Rigidbody2D rb; //Physical body

    private BoxCollider2D boxCollider; // Magic Collider
    private Animator anim; //variable for the animator

    public int lifeTimeOfprojectile = 10; // time after which the projectile is destroyed
    public float magicAttackDamage = 20;
    public string magicTargetName; //target name of the projectile hit
    public GameObject target; //the object hit by the projectile

    private float shootingForce = 0.03f; // projectile speed

    //Sound
    public GameObject magicHitSound;

    private void Awake() //The action is performed before the start of the game and 1 time
    {
        anim = GetComponent<Animator>(); // pull information from the animator component
        boxCollider = GetComponent<BoxCollider2D>(); // pull information from the box colider component
        Instance = this;
    }

    private void Start()
    {
        SaveSerial.Instance.LoadGame();
        magicAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (magicAttackDamage == 0)
        {
            magicAttackDamage = 30;
        }
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (hit) return; //testing a fireball hitting something
        float movementSpeed = Speed * Time.deltaTime * direction; // calculate travel speed per second and in which direction the projectile will fly
        transform.Translate(movementSpeed, 0, 0);// x-axis = movementspeed, y = 0, z=0 - all this movement along the x-axis
        lifetime += Time.deltaTime; //increase the lifetime variable every second +1
        if (lifetime > lifeTimeOfprojectile) gameObject.SetActive(false);//когда переменная достигает 5, снаряд исчезает
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        magicTargetName = collision.gameObject.name;
        if (collision.gameObject.tag == "PlayerCharacter") return;
        hit = true; // here we indicate that a collision has occurred
        boxCollider.enabled = false; //disconnect the collider
        anim.SetTrigger("explode");//to play the projectile attack animation when the magicAttack trigger is executed
        magicHitSound.GetComponent<SoundOfObject>().StopSound();
        magicHitSound.GetComponent<SoundOfObject>().PlaySound();
        DamageObject();
        magicTargetName = string.Empty;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
    private void Flip() //This is where we create the Flip method which, when called, reverses the direction of the sprite
    {
        Vector3 theScale = transform.localScale; //receive the scale of the object
        theScale.x *= -1;//this flips the image e.g. 140 changes to -140, thus completely changing the direction of the sprite (the image is mirrored)
        transform.localScale = theScale; // scale conversion relative to the parent GameObjects object
    }
    public void DamageObject()
    {
        //Debug.Log(magicTargetName);
        target = GameObject.Find(magicTargetName);
        Debug.Log(target);
        if (target != null && target.layer == 7) target.GetComponent<Entity_Enemy>().TakeDamage(magicAttackDamage);
    }
    public void SetDirection(Vector3 shootingDirection)// selecting a flight direction 
    {
        lifetime = 0;
        gameObject.SetActive(true); //activate the game object
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
        Destroy(this.gameObject);// destroy this game object
    }
}
