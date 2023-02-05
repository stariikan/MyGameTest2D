using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } //ƒл€ сбора и отправки данных из этого скрипта
    public float direction;//переменна€ направлени€
    [SerializeField] private float lifetime; //длительность жизни снар€да
    private bool hit = false; //переменна€ метки попал ли во что-то снар€д

    private BoxCollider2D boxCollider; // оллайдер удара

    public int AttackDamage = 20;
    public string TargetName;
    public GameObject target;


    private void Awake() //ƒействие выполн€етс€ до старта игры и 1 раз
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
            AttackDamage = 20;
        }
    }

    private void Update()
    {
        if (hit) return; //проверка попадани€ физатакой во что-нибудь
        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        if (lifetime > 1) gameObject.SetActive(false);//когда переменна€ достигает 1.5, коллайдер атаки исчезает
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        hit = true; //тут указываем что произошло столкновение
        boxCollider.enabled = false; //отключаем коллайдер
        meleeDamageObject();
        TargetName = string.Empty;
    }
    public void meleeDamageObject()
    {
        Debug.Log(TargetName);
        target = GameObject.Find(TargetName);
        if (target.CompareTag("Mushroom"))
        {
            target.GetComponent<Entity_Mushroom>().TakeDamage(AttackDamage);
        }
        else if (target.CompareTag("Chest"))
        {
            target.GetComponent<Chest>().TakeDamage(AttackDamage);
        }
        else if (target.CompareTag("Door"))
        {
            target.GetComponent<door>().TryToOpen();
        }
        else
        {
            return;
        }
    }
    public void meleeDirection(Vector3 _direction)// выбор направлени€ полета 
    {
        lifetime = 0;
        gameObject.SetActive(true); //активаци€ игрового обьекта
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //активаци€ коллайдера 
        hit = false; //обьект коснулс€ другого обьекта = false
    }
    private void Deactivate() //деактиваци€ снар€да после завершени€ анимации взрывал
    {
        gameObject.SetActive(false);
    }
}
