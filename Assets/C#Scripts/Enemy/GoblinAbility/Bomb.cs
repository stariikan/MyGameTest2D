using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float direction;//переменная направления
    private float playerHP; //переменная метки попал ли во что-то снаряд

    private float bombDamage = 40;
    private string enemyName;
    private GameObject enemy;
    private Animator anim;
    public Rigidbody2D rb; //Физическое тело
    public GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной

    private void Start() //Действие выполняется до старта игры и 1 раз
    {
        Instance = this;
    }
    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object) к которому привязан скрипт
    }
    private void Update()
    {
        playerHP = Hero.Instance.curentHP;
    }
    private void BombMovement() //направления и сила полета бомбы 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиции бомбы по оси х
        if (directionX > 0) rb.AddForce(new Vector2(2.7f, 0.5f), ForceMode2D.Impulse);
        if (directionX < 0) rb.AddForce(new Vector2(-2.7f, 0.5f), ForceMode2D.Impulse);
 
    }
    public void BombDestroy() //отключения обьекта бомбы
    {
        Destroy(this.gameObject);//уничтожить этот игровой обьект
    }
    public void BombExplosion() //включения анимации взрыва
    {
        rb.velocity = Vector3.zero; //для остановки обьекта
        anim.SetTrigger("explosion");
    }
    public void BombDmg() //нанесения урона
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиции бомбы по оси х
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиции бомбы по оси y
        float enemyDirectionX = enemy.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция врага по оси х - позиции бомбы по оси х 
        if ((Mathf.Abs(directionX) < 2.0f && Mathf.Abs(directionY) < 2f) && playerHP > 0)
        {
            Hero.Instance.GetDamage(bombDamage);
        }
        if (Mathf.Abs(enemyDirectionX) < 2f) enemy.GetComponent<Entity_Enemy>().TakeDamage(bombDamage/1.5f);
    }
    public void PushFromPlayer() // отскок от игрока
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиции бомбы по оси х
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиции бомбы по оси y
        if (Mathf.Abs(directionX) < 1f)
        {
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (theScale.x > 0) rb.AddForce(new Vector2(+2.7f, 0.5f), ForceMode2D.Impulse);
            if (theScale.x < 0) rb.AddForce(new Vector2(-2.7f, 0.5f), ForceMode2D.Impulse);
        }
    }
    public void bombDirection(Vector3 _direction)// выбор направления полета 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиции тумана по оси х
        this.gameObject.SetActive(true); //активация игрового обьекта
        this.gameObject.transform.position = _direction;
        BombMovement();
    }
    public void GetEnemyName(string name)
    {
        enemyName = name;
        enemy = GameObject.Find(name);
    }
}
