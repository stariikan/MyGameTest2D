using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } // To collect and send data from this script
    public float direction;//directional variable

    private BoxCollider2D boxCollider; //Strike collider
    private float colliderTimer;
    private bool targetIsBlock;
    public float AttackDamage;
    public string TargetName;
    public GameObject target;
    public GameObject masterOfWeapon;
    private int targetDirection;
    private int masterDirection;


    private void Awake() //The action is performed before the start of the game and 1 time
    {
        //anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // pull information from the box colider component
        Instance = this;
    }
    private void Update()
    {
        if (masterOfWeapon.layer == 7)
        {
            float attackRange = masterOfWeapon.GetComponent<Enemy_Behavior>().attackDistance * 2.1f;
            Vector3 newSize = boxCollider.size;
            newSize.x = attackRange;
            boxCollider.size = newSize;
        }

        colliderTimer += Time.deltaTime;
        if (colliderTimer > 0.2f) WeaponOff();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject;
        Debug.Log(target);
        if (masterOfWeapon.layer == 8) masterDirection = masterOfWeapon.GetComponent<Hero>().m_facingDirection;
        if (masterOfWeapon.layer == 7) masterDirection = masterOfWeapon.GetComponent<Enemy_Behavior>().e_facingDirection;
        if (target.layer == 8) targetDirection = target.GetComponentInParent<Hero>().m_facingDirection;
        if (target.layer == 7) targetDirection = target.GetComponent<Enemy_Behavior>().e_facingDirection;

        if (masterOfWeapon.layer == 8 && target.CompareTag("SpellBook")) target.GetComponent<SpellBook>().TakeDamage(AttackDamage);
        if (masterOfWeapon.layer == 8 && target != null && target.layer == 7)
        {
            targetIsBlock = target.GetComponent<Enemy_Behavior>().block;
            if (targetIsBlock)
            {
                Hero.Instance.PlayerStun();
            }
            else
            {
                target.GetComponent<Enemy_Behavior>().TakeDamage(AttackDamage); //7 this is the EnemyLayer
            }
        }
        if (masterOfWeapon.layer == 7 && target != null && target.tag == "Front" && masterDirection != targetDirection)
        {
            target.GetComponentInParent<Hero>().GetDamage(AttackDamage);
        }
        if (masterOfWeapon.layer == 7 && target != null && target.tag == "Back" && masterDirection == targetDirection)
        {
            target.GetComponentInParent<Hero>().GetDamage(AttackDamage * 2);
        }
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
        colliderTimer = 0;
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //activating the collider
    }
}
