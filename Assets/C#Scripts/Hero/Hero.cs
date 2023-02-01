using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour
{
    public static Hero Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float speed = 4f; //Скорость
    public float jumpForce = 90f; //Сила прыжка
    public float rollForce = 40f;

    public bool isGrounded = false; //Находиться ли обьект на земле, а точнее соприкосается ли он с другим обьектом имеющим Collision2D 
    public bool isRoll = true; // персонаж стоит

    public bool flipRight; //Поворот спрайта на право, состояние = правда, нужно для поворота спрайта во время смены движения
    public Vector3 lossyScale; //переменная позиции обьекта

    public float maxHP;
    public float hp; //Количество жизней
    public float stamina;

    public bool playerDead = false; //мертв игрок или нет, пока нужно для того чтобы при смерти игрока делать рестарт
    public int mageAttackDamage;

    public bool block = false;

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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
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
    public void CheckBlock()
    {
        block = HeroAttack.Instance.block;
        if (block == true)
        {
            speed = 2f;
        }
        else
        {
            speed = 4f;
        }
    }
    public void GetDamage(int dmg) //Мы создаем новый метод GetDamage() 
    {
        if (block == false)
        {
            hp -= dmg;//Отнимает int 10 из переменной hp (жизни).
            anim.SetTrigger("damage");
            Push();
        }
        if (block == true)
        {
            hp -= dmg * 0.15f;//Отнимает int из переменной hp (жизни) и при активном блоке уменьшает урон в 3 раза
            HeroAttack.Instance.DecreaseStamina(20);
            anim.SetTrigger("damage");
            Push();
        }
        if (hp <= 0) //Если жизней меньше 0
        {
            speed = 0;
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
    public void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
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
        if (!isGrounded) State = States.jump; //если мы нажимаем Space и мы на земле то State = jump
    }//Метод для поворота спрайта персонажа
    public void Jump()
    {
        if (isGrounded && stamina > 20)// если происходит нажатие и отпускания (GetKeyDown, а не просто GetKey) кнопки Space и если isGrounded = true 
        {
            HeroAttack.Instance.DecreaseStamina(20);
            Vector2 jump = new Vector2(0, 1f);
            rb.velocity = jump * jumpForce;
            Debug.Log("JUMP!");
            isGrounded = false;            
        }
    }
    public void Roll()
    {
        if (isGrounded && isRoll && stamina > 15) //кувырок
        {
            HeroAttack.Instance.DecreaseStamina(15);
            if (!flipRight)
            {
                rb.AddForce(new Vector2(rollForce, 0), ForceMode2D.Impulse);
                isRoll = false;
                Debug.Log("LeftROLL");
            }
            if (flipRight)
            {
                rb.AddForce(new Vector2(rollForce * -1, 0), ForceMode2D.Impulse);
                isRoll = false;
                Debug.Log("RightRoll");
            }
        }
    }
    public void PlayerMovement()
    {
        float move = Input.GetAxis("Horizontal");//Используем Float потому-что значение 0.111..., тут берется ввод по Горизонтали (стрелки и A D)
        float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical"); //потом для лестниц нужно будет

        Vector2 movement = new Vector2(horizontal, 0); //, vertical//);
        rb.velocity = movement * speed;

        if (move > 0 && !flipRight) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }
        else if (move < 0 && flipRight) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && stamina > 20)// если происходит нажатие и отпускания (GetKeyDown, а не просто GetKey)
                                                          // кнопки Space и если isGrounded = true 
        {
            HeroAttack.Instance.DecreaseStamina(20);
            Vector2 jump = new Vector2(0, 1f);
            rb.velocity = jump * jumpForce;
            isGrounded = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && isRoll && stamina > 15) //кувырок
        {
            HeroAttack.Instance.DecreaseStamina(15);
            if(!flipRight)
            {
                rb.AddForce(new Vector2(rollForce, 0), ForceMode2D.Impulse);
                isRoll = false;
            }
            if(flipRight)
            {
                rb.AddForce(new Vector2(rollForce * -1, 0), ForceMode2D.Impulse);
                isRoll = false;
            }
            
        }
        else
        {
            isRoll = true;
        }
    } 

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
        if (maxHP == 0)
        {
            maxHP = 100;
        }
        hp = maxHP;
        mageAttackDamage = SaveSerial.Instance.playerMageDamage;
        if (mageAttackDamage == 0)
        {
            mageAttackDamage = 30;
        }

        stamina = SaveSerial.Instance.playerStamina;
        if (stamina == 0)
        {
            stamina = 100;
        }

        rb = GetComponent<Rigidbody2D>();

    }
    private void FixedUpdate()
    {
        stamina = HeroAttack.Instance.currentStamina; //проверка стамины
        if (hp > 0)
        {
            PlayerMovement();//Метод для движения и поворота спрайта персонажа
            AnimState();//Метод для передачи состояния в аниматор
            DieByFall();//Метод для смерти от падения
            CheckBlock(); //Проверка блока
        }
        else
        {
            return;
        }
    }
}

