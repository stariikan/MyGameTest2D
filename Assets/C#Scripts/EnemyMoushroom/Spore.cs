using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    public static Spore Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float direction;//переменная направления
    [SerializeField] private float lifetime; //длительность жизни снаряда
    private bool hit = false; //переменная метки попал ли во что-то снаряд

    private BoxCollider2D boxCollider; //Коллайдер удара

    private int sporeDamage = 10;
    private float sporeCooldownDmg;
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
        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        sporeCooldownDmg += Time.deltaTime;//кулдаун атаки спор
        if (lifetime > 10) this.gameObject.SetActive(false);//когда переменная достигает 5, коллайдер атаки исчезает
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        hit = true; //тут указываем что произошло столкновение
        if(sporeCooldownDmg > 1)
        {
            sporeCooldownDmg = 0;
            Hero.Instance.GetDamage(sporeDamage);
        }
        TargetName = string.Empty;
    }
    public void sporeDirection(Vector3 _direction)// выбор направления полета 
    {
        lifetime = 0;
        this.gameObject.SetActive(true); //активация игрового обьекта
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //активация коллайдера 
        hit = false; //обьект коснулся другого обьекта = false
    }
}
