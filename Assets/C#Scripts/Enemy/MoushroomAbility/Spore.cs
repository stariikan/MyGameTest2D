using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    public static Spore Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float direction;//переменная направления
    [SerializeField] private float lifetime; //длительность жизни снаряда
    private float playerHP; //переменная метки попал ли во что-то снаряд

    private CircleCollider2D circleCollider; //Коллайдер удара

    private float sporeDamage = 20;
    private float sporeCooldownDmg;
    private float sporeSpeed = 1f;
    GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной

    private void Start() //Действие выполняется до старта игры и 1 раз
    {
        player = GameObject.FindWithTag("PlayerCharacter");
        circleCollider = GetComponent<CircleCollider2D>(); // вытаскиваем информацию из компанента бокс колайдер
        Instance = this;
        playerHP = Hero.Instance.curentHP;
    }
    private void Update()
    {
        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        sporeCooldownDmg += Time.deltaTime;//кулдаун атаки спор
        playerHP = Hero.Instance.curentHP;
        SporeDmg();
        SporeMovement();
        if (lifetime > 5) Destroy(this.gameObject);//уничтожить этот игровой обьект

    }
    private void SporeMovement()
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиции тумана по оси х
        //int level = LvLGeneration.Instance.Level;
        if (playerHP > 0)
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float playerFollowSpeed = Mathf.Sign(directionX) * sporeSpeed * Time.deltaTime; //вычесление направления
            pos.x += playerFollowSpeed; //вычесление позиции по оси х
            transform.position = pos; //применение позиции
        }
    }
    private void SporeDmg()
    {
       float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиции тумана по оси х
       float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиции тумана по оси y
        if ((Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 2f) && sporeCooldownDmg > 1 && playerHP > 0)
       {
            sporeCooldownDmg = 0;
            Hero.Instance.GetDamage(sporeDamage);
       }
    }
    public void sporeDirection(Vector3 _direction)// выбор направления полета 
    {
        lifetime = 0;
        this.gameObject.SetActive(true); //активация игрового обьекта
        this.gameObject.transform.position = _direction;
    }
}
