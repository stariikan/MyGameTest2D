using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance { get; set; } //ƒл€ сбора и отправки данных из этого скрипта
    public float direction;//переменна€ направлени€
    [SerializeField] private float lifetime; //длительность жизни снар€да
    private float playerHP; //переменна€ метки попал ли во что-то снар€д

    private float bombDamage = 40;
    private Animator anim;
    public Rigidbody2D rb; //‘изическое тело
    GameObject player; //геймобьект игрок и ниже будет метод как он определ€етс€ и присваиваетс€ этой переменной

    private void Start() //ƒействие выполн€етс€ до старта игры и 1 раз
    {
        player = GameObject.FindWithTag("PlayerCharacter");
        Instance = this;
        playerHP = Hero.Instance.hp;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>(); //ѕеременна€ anim получает информацию из компонента Animator (јнимаци€ game.Object) к которому прив€зан скрипт
    }
    private void Update()
    {
        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        playerHP = Hero.Instance.hp;
        //if (lifetime > 3) this.gameObject.SetActive(false);//когда переменна€ достигает 5, коллайдер атаки исчезает
    }
    private void BombMovement() //направлени€ и сила полета бомбы 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движени€ это ѕозици€ игрока по оси х - позиции тумана по оси х
        if (directionX > 0)
        {
            rb.AddForce(new Vector2(3, 1.5f), ForceMode2D.Impulse);
        }
        if (directionX < 0)
        {
            rb.AddForce(new Vector2(-3, 1.5f), ForceMode2D.Impulse);
        }
    }
    public void BombDestroy() //отключени€ обьекта бомбы
    {
        this.gameObject.SetActive(false);
    }
    public void BombExplosion() //включени€ анимации взрыва
    {
        anim.SetTrigger("explosion");
    }
    public void BombDmg() //нанесени€ урона
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движени€ это ѕозици€ игрока по оси х - позиции тумана по оси х
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движени€ это ѕозици€ игрока по оси y - позиции тумана по оси y
        if ((Mathf.Abs(directionX) < 3f && Mathf.Abs(directionY) < 2f) && playerHP > 0)
        {
            Hero.Instance.GetDamage(bombDamage);
        }
    }
    public void bombDirection(Vector3 _direction)// выбор направлени€ полета 
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движени€ это ѕозици€ игрока по оси х - позиции тумана по оси х
        lifetime = 0;
        this.gameObject.SetActive(true); //активаци€ игрового обьекта
        this.gameObject.transform.position = _direction;
        BombMovement();
    }
}
