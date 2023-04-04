using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } // To collect and send data from this script
    public float direction;//directional variable

    private BoxCollider2D boxCollider; //Strike collider

    public float AttackDamage = 15;
    public string TargetName;
    public GameObject target;


    private void Awake() //The action is performed before the start of the game and 1 time
    {
        //anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // pull information from the box colider component
        Instance = this;
    }
    private void Start()
    {
        SaveSerial.Instance.LoadGame();
        AttackDamage = SaveSerial.Instance.playerAttackDamage;
        if (AttackDamage == 0)
        {
            AttackDamage = 15;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        target = GameObject.Find(TargetName);
        //Debug.Log(target);
        if (target.CompareTag("SpellBook")) target.GetComponent<SpellBook>().TakeDamage(AttackDamage);
        if (target !=null && target.layer == 7) target.GetComponent<Entity_Enemy>().TakeDamage(AttackDamage); //7 this is the EnemyLayer
    }
    public void WeaponOff() // deactivating the bomb object
    {
        this.gameObject.SetActive(false);
    }
    public void MeleeDirection(Vector3 _direction)// selecting a flight direction 
    {
        gameObject.SetActive(true); //activate the game object
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //activating the collider
    }
}
