using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float direction;//переменная направления
    [SerializeField] private float lifetime; //длительность жизни снаряда

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

    private void Update()
    {
        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        if (lifetime > 1) 
        {
            gameObject.SetActive(false);//когда переменная достигает 1.5, коллайдер атаки исчезает
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        boxCollider.enabled = false; //отключаем коллайдер
        target = GameObject.Find(TargetName);
    }
    public void meleeDirection(Vector3 _direction)// выбор направления полета 
    {
        lifetime = 0;
        gameObject.SetActive(true); //активация игрового обьекта
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //активация коллайдера

        if (target.CompareTag("Skeleton"))
        {
            target.GetComponent<Entity_Skeleton>().TakeDamage(AttackDamage);
        }
        if (target.CompareTag("Mushroom"))
        {
            target.GetComponent<Entity_Mushroom>().TakeDamage(AttackDamage);
        }
        if (target.CompareTag("Chest"))
        {
            target.GetComponent<Chest>().TakeDamage(AttackDamage);
        }
        if (target.CompareTag("Door"))
        {
            target.GetComponent<door>().TryToOpen();
        }
    }
    private void Deactivate() //деактивация снаряда после завершения анимации взрывал
    {
        gameObject.SetActive(false);
    }
}
