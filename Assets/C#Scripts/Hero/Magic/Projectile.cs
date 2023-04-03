using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Projectile Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float direction;//переменная направления
    [SerializeField] private float Speed; //Скорость снаряда
    [SerializeField] private float lifetime; //длительность жизни снаряда
    private bool hit = false; //переменная метки попал ли во что-то снаряд

    public Rigidbody2D rb; //Физическое тело

    private BoxCollider2D boxCollider; //Коллайдер магии
    private Animator anim; //переменная для аниматора

    public int lifeTimeOfprojectile = 10; //время после которого снаряд уничтожается
    public float magicAttackDamage = 20;
    public string magicTargetName; //имя цели по которому попал снаряд
    public GameObject target; //обьект по которому попал снаряд

    private float shootingForce = 0.03f; //скорость снаряда

    //Sound
    public GameObject magicHitSound;

    private void Awake() //Действие выполняется до старта игры и 1 раз
    {
        anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // вытаскиваем информацию из компанента бокс колайдер
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
        if (hit) return; //проверка попадания огненого шара во что-нибудь
        float movementSpeed = Speed * Time.deltaTime * direction; // вычисление скорости перемещения в секунду и в каком направлении полетит снаряд
        transform.Translate(movementSpeed, 0, 0);//ось х = movementspeed, y = 0, z=0 - все это перемещение по оси x
        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        if (lifetime > lifeTimeOfprojectile) gameObject.SetActive(false);//когда переменная достигает 5, снаряд исчезает
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        magicTargetName = collision.gameObject.name;
        if (collision.gameObject.tag == "PlayerCharacter") return;
        hit = true; //тут указываем что произошло столкновение
        boxCollider.enabled = false; //отключаем коллайдер
        anim.SetTrigger("explode");//для воспроизведения анимации атаки снарядом при выполнения тригера magicAttack
        magicHitSound.GetComponent<SoundOfObject>().StopSound();
        magicHitSound.GetComponent<SoundOfObject>().PlaySound();
        DamageObject();
        magicTargetName = string.Empty;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
    private void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
    {
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
    }
    public void DamageObject()
    {
        //Debug.Log(magicTargetName);
        target = GameObject.Find(magicTargetName);
        Debug.Log(target);
        if (target != null && target.layer == 7) target.GetComponent<Entity_Enemy>().TakeDamage(magicAttackDamage);
    }
    public void SetDirection(Vector3 shootingDirection)// выбор направления полета 
    {
        lifetime = 0;
        gameObject.SetActive(true); //активация игрового обьекта
        if (shootingDirection.x == -1 && transform.localScale.x > 0) Flip();
        if (shootingDirection.x == 1 && transform.localScale.x < 0) Flip();
        boxCollider.enabled = true; //активация коллайдера 
        
        hit = false; //обьект коснулся другого обьекта = false
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>(); //получения компонента RigidBody2D
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(shootingDirection * shootingForce); //приложение силы к обьекту = направления умноження на скорость снаряда
    }
    private void Deactivate() //деактивация снаряда после завершения анимации взрывал
    {
        Destroy(this.gameObject);//уничтожить этот игровой обьект
    }
}
