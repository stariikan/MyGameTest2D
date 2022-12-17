using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skelet : Entity //наследование класса сущности (то есть методы которые используются в Entity будут применены и к этому обьекту)
{
    // Start is called before the first frame update
    [SerializeField] public int hp = 30; //жизни скелета
    [SerializeField] private float speed = 2f;//параметр скорости скелета
    [SerializeField] private float speed2 = 2f;//параметр скорости скелета 2 параметр нужен для восстановление скорости по умолчанию (после остановки перед пропастью и наверное после замедлений которых я еще не придумал))
    GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной
    public Rigidbody2D rb; //Физическое тело
    public LayerMask groundLayers;//это будут слои которые будут проверятся
    public Transform groundcheck;// проверка соприкасается ли метка (которую мы создали с землей)

    public static Enemy_Skelet Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    private bool isMoving = false;
    private Animator skelet_anim; //Переменная благодаря которой анимирован обьект, переменная = skelet_anim

    private bool flipRight = true; //Поворот спрайта на право, состояние = правда, нужно для поворота спрайта во время смены движения
    RaycastHit2D hit; //тут будем получать информацию с чем сталкивается обьект

    public void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
    {
        flipRight = !flipRight; //Когда запускается метод Flip переменная flipRight меняется на false
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
    }
    public void PlayerFollow() //Метод в котором описываем логику следования за игроком
    {

        float direction = player.transform.position.x - transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х

        if (Mathf.Abs(direction) < 4) //если меньше разница меньше 4 метров
        {
            Vector3 pos = transform.position; //то происходит изминение позиции
            pos.x += Mathf.Sign(direction) * speed * Time.deltaTime;// тут высчитывается направление и скорость в секунду скелета
                                                                    // (PS: MathF. Sign(Single) - это метод класса MathF, который возвращает целое число, определяющее знак числа)
            transform.position = pos;//тут меняется позиция на ту что посчиталась в прошлой строке в формуле
            isMoving = true;//Если этот метод выполняется переменная isMoving становиться правдой
        }
        else
        {
            isMoving = false;//Если этот метод перестает выполняется переменная isMoving становиться не правдой
        }
        if (direction > 0 && !flipRight) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }
        else if (direction < 0 && flipRight) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }
        
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player"); //тут при старте игры скелет находит игрока по тегу Player и присваивает найденную и информацию переменной player
    }
    
       
    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 1f, groundLayers);//мы стреляем Raycast вниз с позиции обьекта groundcheck, на 1 еденицу
                                                                                       //и проверяем столкнулся ли обьект с землей (groundLayers)
                                                                                       //PlayerFollow();
                       
        if (hit.collider != true) //если обьект groundcheck не столкнулся с полом (то есть пропасть)
        {
            speed = 0f;//то уменьшаем скоро до 0
        }
        else
        {
            speed = speed2;//если обьект ground check вновь сталкивается с полом (видит землю), то возвращаем показатель скорости.
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) //тут будет описан как и по кому будет наносить урон скелет
    {
        if (collision.gameObject == Hero.Instance.gameObject) //Если скелет соприкасается именно с героем 
                                                              //(тут получается ссылка на скрипт Hero и оттуда берется gameObject)
        {
            Hero.Instance.GetDamage(); //Из скрипта Hero вызывается публичный метод который меняет переменную hp -= 10.
            //hp -= 10; //но при этом и у скелета тратятся 10 жизней
            Debug.Log("Скелет потерял 10 жизней, осталось" + hp);//написание в логах количества жизней у скелета
        }
    }
    void Update() //тут складывать буду основные действия методы (который должен использовать враг)
    {
       
        PlayerFollow();

        if (hp < 0)//если hp меньше или равно 0
            Die();//то смерть и уничтожение gameObject, это публичный метод из скрипта Entity

        //Стейты анимации
        if (isMoving == false) State = States.idle;//если не двигается значит анимации ожидания
        if (isMoving) State = States.run;//если координаты скелета поменялись, то State = run
        if (!hit.collider) State = States.jump; //и если мы не на земле State = jump. Это все нужно чтобы менялась анимация

    }
    //Блок с анимацией скелета
    public enum States //Определения какие бывают состояния, указал названия как в Аниматоре Unity
    {
        idle,
        run,
        jump
    }
    private States State //Создание стейтмашины, переменная = State. Значение состояния может быть передано или изминено извне благодаря get и set
    {
        get { return (States)skelet_anim.GetInteger("State"); }
        set { skelet_anim.SetInteger("State", (int)value); }
    }
    void Awake() //Awake используется для инициализации любых переменных или игрового состояния перед началом игры.
                 //Awake вызывается только один раз за все время существования экземпляра сценария.
                 //Вызов Awake происходит после инициализации всех объектов, поэтому можно безопасно обращаться к другим объектам
                 //или запрашивать их, используя, например, GameObject.
    {
        rb = GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object)
                                          //к которому привязан скрипт
        skelet_anim = GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
                                         //к которому привязан скрипт
        Instance = this; //'this' - это ключевое слово, обозначающее класс, в котором выполняется код.
                         //Насколько мне известно, оно никогда не требуется, но делает код более читабельным this. transform. position and transform.
    }

}
