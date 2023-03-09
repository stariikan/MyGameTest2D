using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance { get; set; } //ƒл€ сбора и отправки данных из этого скрипта
    public float direction;//переменна€ направлени€
    private float playerHP; //переменна€ метки попал ли во что-то снар€д

    private float bombDamage = 40;
    private Animator anim;
    public Rigidbody2D rb; //‘изическое тело
    public GameObject player; //геймобьект игрок и ниже будет метод как он определ€етс€ и присваиваетс€ этой переменной

    private void Start() //ƒействие выполн€етс€ до старта игры и 1 раз
    {
        Instance = this;
    }
    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>(); //ѕеременна€ anim получает информацию из компонента Animator (јнимаци€ game.Object) к которому прив€зан скрипт
    }
    private void Update()
    {
        playerHP = Hero.Instance.hp;
    }
    private void BombMovement() //направлени€ и сила полета бомбы 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движени€ это ѕозици€ игрока по оси х - позиции тумана по оси х
        if (directionX > 0) rb.AddForce(new Vector2(2.7f, 0.5f), ForceMode2D.Impulse);
        if (directionX < 0) rb.AddForce(new Vector2(-2.7f, 0.5f), ForceMode2D.Impulse);
 
    }
    public void BombDestroy() //отключени€ обьекта бомбы
    {
        Destroy(this.gameObject);//уничтожить этот игровой обьект
    }
    public void BombExplosion() //включени€ анимации взрыва
    {
        rb.velocity = Vector3.zero; //дл€ остановки обьекта
        anim.SetTrigger("explosion");
    }
    public void BombDmg() //нанесени€ урона
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движени€ это ѕозици€ игрока по оси х - позиции тумана по оси х
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движени€ это ѕозици€ игрока по оси y - позиции тумана по оси y
        if ((Mathf.Abs(directionX) < 2.0f && Mathf.Abs(directionY) < 2f) && playerHP > 0)
        {
            Hero.Instance.GetDamage(bombDamage);
        }
    }
    public void PushFromPlayer() // отскок от игрока
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движени€ это ѕозици€ игрока по оси х - позиции тумана по оси х
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движени€ это ѕозици€ игрока по оси y - позиции тумана по оси y
        if (Mathf.Abs(directionX) < 1f)
        {
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (theScale.x > 0) rb.AddForce(new Vector2(+2.7f, 0.5f), ForceMode2D.Impulse);
            if (theScale.x < 0) rb.AddForce(new Vector2(-2.7f, 0.5f), ForceMode2D.Impulse);
        }
    }
    public void bombDirection(Vector3 _direction)// выбор направлени€ полета 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движени€ это ѕозици€ игрока по оси х - позиции тумана по оси х
        this.gameObject.SetActive(true); //активаци€ игрового обьекта
        this.gameObject.transform.position = _direction;
        BombMovement();
    }
}
