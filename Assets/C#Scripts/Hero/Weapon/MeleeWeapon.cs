using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float direction;//переменная направления

    private BoxCollider2D boxCollider; //Коллайдер удара

    public float AttackDamage = 15;
    public string TargetName;
    public GameObject target;


    private void Awake() //Действие выполняется до старта игры и 1 раз
    {
        //anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // вытаскиваем информацию из компанента бокс колайдер
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
        if (target !=null && target.layer == 7) target.GetComponent<Entity_Enemy>().TakeDamage(AttackDamage); //7 это EnemyLayer
    }
    public void WeaponOff() //отключения обьекта бомбы
    {
        this.gameObject.SetActive(false);
    }
    public void MeleeDirection(Vector3 _direction)// выбор направления полета 
    {
        gameObject.SetActive(true); //активация игрового обьекта
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //активация коллайдера
    }
}
