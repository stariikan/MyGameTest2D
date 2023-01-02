using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public static MeleeWeapon Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float direction;//переменная направления
    [SerializeField] private float lifetime; //длительность жизни снаряда
    private bool hit = false; //переменная метки попал ли во что-то снаряд

    private BoxCollider2D boxCollider; //Коллайдер удара
    //private Animator anim; //переменная для аниматора

    public int AttackDamage = 20;
    public string TargetName;
    public GameObject target;


    private void Awake() //Действие выполняется до старта игры и 1 раз
    {
        //anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // вытаскиваем информацию из компанента бокс колайдер
        Instance = this;
    }

    private void Update()
    {
        if (hit) return; //проверка попадания физатакой во что-нибудь
        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        if (lifetime > 1) gameObject.SetActive(false);//когда переменная достигает 1.5, коллайдер атаки исчезает
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        hit = true; //тут указываем что произошло столкновение
        boxCollider.enabled = false; //отключаем коллайдер
        //anim.SetTrigger("explode");//для воспроизведения анимации атаки снарядом при выполнения тригера magicAttack
        meleeDamageObject();
        TargetName = string.Empty;
        //Deactivate();
    }
    public void meleeDamageObject()
    {
        Debug.Log(TargetName);
        target = GameObject.Find(TargetName);
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<Entity>().TakeDamage(AttackDamage);
        }
        if (target.CompareTag("Chest"))
        {
            target.GetComponent<Chest>().TakeDamage(AttackDamage);
        }
    }
    public void meleeDirection(float _direction)// выбор направления полета 
    {
        lifetime = 0;
        gameObject.SetActive(true); //активация игрового обьекта
        direction = _direction;
        boxCollider.enabled = true; //активация коллайдера 
        hit = false; //обьект коснулся другого обьекта = false
        float localScaleX = transform.localScale.x; //этот весь код про то чтобы менялся x на -x в зависимости в какую сторону мы стреляем, тоесть был переворот спрайта 
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);//смена направления снаряда
    }
    private void Deactivate() //деактивация снаряда после завершения анимации взрывал
    {
        gameObject.SetActive(false);
    }
}
