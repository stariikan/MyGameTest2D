using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skelet : MonoBehaviour //наследование класса сущности (то есть методы которые используются в Entity будут применены и к этому обьекту)
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 1f;//параметр скорости скелета
    [SerializeField] private float speedRecovery;//параметр скорости скелета 2 параметр нужен для восстановление скорости по умолчанию (после остановки перед пропастью и наверное после замедлений которых я еще не придумал))
    public int attackDamage = 7;

    GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной
    public Rigidbody2D rb; //Физическое тело
    //public LayerMask groundLayers;//это будут слои которые будут проверятся
    public Transform groundcheck;// проверка соприкасается ли метка (которую мы создали с землей)

    private bool isMoving = false;
    private Animator anim; //Переменная благодаря которой анимирован обьект, переменная = skelet_anim

    private bool flipRight; //Поворот спрайта на право, состояние = правда, нужно для поворота спрайта во время смены движения
    RaycastHit2D hit; //тут будем получать информацию с чем сталкивается обьект

    public enum States //Определения какие бывают состояния, указал названия как в Аниматоре Unity
    {
        idle,
        run
    }
    private States State //Создание стейтмашины, переменная = State. Значение состояния может быть передано или изминено извне благодаря get и set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    public void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
    {
        flipRight = !flipRight; //Когда запускается метод Flip переменная flipRight меняется на false
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
    }
    public void BoostSpeed()
    {
        speed += 0.1f;
    }
    public void PlayerFollow() //Метод в котором описываем логику следования за игроком
    {
        if (player)
        {
            float directionX = player.transform.position.x - transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
            float directionY = player.transform.position.y - transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х

            if (Mathf.Abs(directionX) < 4 && Mathf.Abs(directionX) > 0.4f && Mathf.Abs(directionY) < 2) //если меньше разница меньше 4 метров по х и 2 метров по y
            {
                Vector3 pos = transform.position; //то происходит изминение позиции
                pos.x += Mathf.Sign(directionX) * speed * Time.deltaTime;// тут высчитывается направление и скорость в секунду скелета
                                                                        // (PS: MathF. Sign(Single) - это метод класса MathF, который возвращает целое число, определяющее знак числа)
                transform.position = pos;//тут меняется позиция на ту что посчиталась в прошлой строке в формуле
                isMoving = true;//Если этот метод выполняется переменная isMoving становиться правдой
            }
            else
            {
                isMoving = false;//Если этот метод перестает выполняется переменная isMoving становиться не правдой
            }
            if (directionX < 0 && flipRight) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            {
                Flip();
            }
            else if (directionX > 0 && !flipRight) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
            {
                Flip();
            }
        }
                       
    }
    private void DieByFall() //Метод который наносит урон при падении с платформы
    {
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity>().enemyDead == false)//если координаты игрока по оси y меньше 10 и враг не мертв, то происходит вызов метода GetDamage
        {
            Entity.Instance.TakeDamage(10);
        }
    }
    public void AnimState()//Метод для определения стейта анимации
    {
        if (isMoving == false) State = States.idle;//если не двигается значит анимации ожидания
        if (isMoving) State = States.run;//если координаты скелета поменялись, то State = run
    }
    public void groundCheckPosition()//проверка на пропость, чтобы скелет туда не упал
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 2f, Physics.DefaultRaycastLayers);//мы стреляем Raycast вниз с позиции обьекта groundcheck, на 1 еденицу
                                                                                       //и проверяем столкнулся ли обьект с землей (groundLayers)
                                                                                       //PlayerFollow();

        if (hit.collider != true) //если обьект groundcheck не столкнулся с полом (то есть пропасть)
        {
            speed = 0f;//то уменьшаем скоро до 0
        }
        else
        {
            speed = speedRecovery;//если обьект ground check вновь сталкивается с полом (видит землю), то возвращаем показатель скорости.
        }
    }
    private void Awake() //События которые должны произойти при старте игры
    {
        player = GameObject.FindWithTag("Player"); //тут при старте игры скелет находит игрока по тегу Player и присваивает найденную и информацию переменной player
        rb = GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object)
                                          //к которому привязан скрипт
        anim = GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
                                         //к которому привязан скрипт
        flipRight = true;
    }
    private void Start()
    {
        speed = SaveSerial.Instance.enemySpeed;
        if (speed == 0f)
        {
            speed = 1f;
        }
        speedRecovery = speed;
    }
    void Update() //тут складывать буду основные действия методы (который должен использовать враг)
    {
        if (this.gameObject.GetComponent<Entity>().currentHP > 0)
        {
            PlayerFollow();
            DieByFall();
            AnimState();
            groundCheckPosition();
        }
        else
        {
            return;
        }
    }
    

   
    

}
