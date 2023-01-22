using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour
{
    public static Hero Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public int movement_scalar = 60; //нужно для движение 
    public float maxSpeed = 3f; //Максимальная скорость
    private bool flipRight; //Поворот спрайта на право, состояние = правда, нужно для поворота спрайта во время смены движения
    public Vector3 lossyScale; //переменная позиции обьекта
    public bool isGrounded = false; //Находиться ли обьект на земле, а точнее соприкосается ли он с другим обьектом имеющим Collision2D 
    public float gravityScale = 10; //Сила притяжения или чем ниже тем выше прыжок
    public float fallingGravityScale = 40; //Сила притяжение при падении чем выше тем сильнее игровой обьекс тянет вниз
    public int maxHP = 100;
    public int hp = 100; //Количество жизней
    public bool playerDead = false; //мертв игрок или нет, пока нужно для того чтобы при смерти игрока делать рестарт
    public int mageAttackDamage = 30;

    private Rigidbody2D rb; //Тело с физической переменной к которому принадлежит скрипт, переменная = rb
    private Animator anim; //Переменная благодаря которой анимирован обьект, переменная = anim
    private States State //Создание стейтмашины, переменная = State. Значение состояния может быть передано или изминено извне благодаря get и set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    public enum States //Определения какие бывают состояния, указал названия как в Аниматоре Unity
    {
        idle,
        run,
        jump
    }
    private void OnCollisionEnter2D(Collision2D collision) //OnCollisionEnter вызывается, когда этот колайдер/тело начинает касаться другого тела/коллайдера.
                                                           //В отличие от OnTriggerEnter, OnCollisionEnter передается класс Collision, а не Collider.
                                                           //Класс Collision содержит информацию, например, о точках контакта и скорости удара.
    {
        isGrounded = true; //Если персонаж касается другого тела, считается что он на земле
    }
    public void Push() //Метод для отталкивания тела во время получения урона
    {
        if (transform.lossyScale.x < 0) //смотрим в трансформе в какую сторону повернут по х обьект 
        {
            rb.AddForce(new Vector2(2.5f, 2.5f), ForceMode2D.Impulse);//Импульс это значит что сила приложиться всего 1 раз
        }
        else
        {
            rb.AddForce(new Vector2(-2.5f, 2.5f), ForceMode2D.Impulse);//Импульс это значит что сила приложиться всего 1 раз
        }
    }
    public void GetDamage(int dmg) //Мы создаем новый метод GetDamage() 
                            //Пишет изменившееся значение в лог и 
    {
        hp -= dmg;//Отнимает int 10 из переменной hp (жизни).
        anim.SetTrigger("damage");
        Push();
        if (hp <= 0) //Если жизней меньше 0
        {
            maxSpeed = 0;
            anim.SetTrigger("death");
            playerDead = true;
        }
    }
    private void Deactivate() //деактивация игрока после завершения анимации смерти (благодоря метки в аниматоре выполняется этот метод
    {
        gameObject.SetActive(false);
    }
    public void Hero_hp() //Метод который просто вызывает значение переменной HP, нужен мне был для передачи этого числа в скрипт с каунтером жизней
    {
        Debug.Log(hp);
    }
    private void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
    {
        flipRight = !flipRight; //Когда запускается метод Flip переменная flipRight меняется на false
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
    }
    private void DieByFall() //Метод который наносит урон при падении с платформы
    {
        if (rb.transform.position.y < -100)//если координаты игрока по оси y меньше 10, то происходит вызов метода GetDamage
        {
            GetDamage(100);
        }
    }
    public void AnimState()
    {
        if (!Input.GetButton("Horizontal")) State = States.idle;//если мы на земле State = idle
        if (Input.GetButton("Horizontal")) State = States.run;//если мы нажимаем на кнопки (стрелки или A D) то State = run
        if (Input.GetKeyDown(KeyCode.Space)) State = States.jump; //если мы нажимаем Space и мы на земле то State = jump
    }//Метод для поворота спрайта персонажа
    public void PlayerMovement()
    {
        float move = Input.GetAxis("Horizontal");//Используем Float потому-что значение 0.111..., тут берется ввод по Горизонтали (стрелки и A D)
        if (rb.velocity.magnitude < maxSpeed)
        {
            Vector2 movement = new Vector2(move, 0);
            rb.AddForce(new Vector2 (movement_scalar * move, 0), ForceMode2D.Force);//Тут указно что берется компонент Rigidbody2D
        }
        
                                                                                                                    //у нашего game.Object и благодоря new Vector2
                                                                                                                    //изминяетя позиция game.Object помноженая (*)
                                                                                                                    //максимальную скорость которую мы указали в переменной
                                                                                                                    //по оси x
                                                                                                                    //velocity = Единица часто считаются метрами, но могут быть миллиметрами или световыми годами.
                                                                                                                    //Также имеет скорость в X, Y и Z, определяя направление.
        if (move > 0 && !flipRight) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }
        else if (move < 0 && flipRight) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)// если происходит нажатие и отпускания (GetKeyDown, а не просто GetKey)
                                                          // кнопки Space и если isGrounded = true 
        {
            isGrounded = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 20f), ForceMode2D.Impulse); //ForceMode2D.Impulse  It may seem like your object is pushed once in Y axis and it will fall down automatically due to gravity.

        }
        if (rb.velocity.y >= 0) //Если скорость тела по оси Y больше или равно 0, то
        {
            rb.gravityScale = gravityScale; //тут как раз получается что от параметра задданного в переменной будет зависеть сила прыжка
        }
        else if (rb.velocity.y < 0) //если скорость персонажа по оси Y меньше 0 то
        {
            rb.gravityScale = fallingGravityScale; //гравитация начинает притягивать обьект в низ в зависимости
                                                   //от числа который мы ввели в переменной fallingGravityScale
        }
    } //Метод для поворота спрайта персонажа

    void Awake() //Awake используется для инициализации любых переменных или игрового состояния перед началом игры.
                 //Awake вызывается только один раз за все время существования экземпляра сценария.
                 //Вызов Awake происходит после инициализации всех объектов, поэтому можно безопасно обращаться к другим объектам
                 //или запрашивать их, используя, например, GameObject.
    {
        rb = GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object)
                                          //к которому привязан скрипт
        anim = GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
                                         //к которому привязан скрипт
        Instance = this; //'this' - это ключевое слово, обозначающее класс, в котором выполняется код.
                         //Насколько мне известно, оно никогда не требуется, но делает код более читабельным this. transform. position and transform.
        flipRight = true;
    }
    private void Start()
    {
        SaveSerial.Instance.LoadGame();
        maxHP = SaveSerial.Instance.playerHP;
        mageAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (maxHP == 0)
        {
            maxHP = 100;
        }
        hp = maxHP;
        if (mageAttackDamage == 0)
        {
            mageAttackDamage = 30;
        }
    }
    private void FixedUpdate()
    {
        if (hp > 0)
        {
            PlayerMovement();//Метод для движения и поворота спрайта персонажа
            AnimState();//Метод для передачи состояния в аниматор
            DieByFall();//Метод для смерти от падения
        }
        else
        {
            return;
        }
    }
}

