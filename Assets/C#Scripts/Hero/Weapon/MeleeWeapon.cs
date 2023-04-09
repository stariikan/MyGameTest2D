using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } // To collect and send data from this script
    public float direction;//directional variable

    private BoxCollider2D boxCollider; //Strike collider

    public float AttackDamage;
    public string TargetName;
    public GameObject target;
    public GameObject masterOfWeapon;


    private void Awake() //The action is performed before the start of the game and 1 time
    {
        //anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // pull information from the box colider component
        Instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        target = GameObject.Find(TargetName);
        Debug.Log(target);
        if (masterOfWeapon.layer == 8 &&target.CompareTag("SpellBook")) target.GetComponent<SpellBook>().TakeDamage(AttackDamage);
        if (masterOfWeapon.layer == 8 && target != null && target.layer == 7) target.GetComponent<Enemy_Behavior>().TakeDamage(AttackDamage); //7 this is the EnemyLayer
        if (masterOfWeapon.layer == 7 && target != null && target.layer == 8) target.GetComponentInParent<Hero>().GetDamage(AttackDamage); //8 this is the PlayerLayer
    }
    public void GetAttackDamageInfo(float damageInfo) //Getting a damage score from an object
    {
        AttackDamage = damageInfo;
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
