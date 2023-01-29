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
    public Transform groundcheck;// проверка соприкасается ли метка (которую мы создали с землей)

    private bool isMoving = false;
    private Animator anim; //Переменная благодаря которой анимирован обьект, переменная = skelet_anim

    public Transform wallChekPoint; //Тут мы ссылаемся на точку которая является дочерним обьектом 

    private bool isGround; // находиться ли обьект на земле

    private bool playerFollow = false; //моб не приследует игрока
    private float patrolCouldown = 0; //кулдаун направления потрулирования
    RaycastHit2D hit; //тут будем получать информацию с чем сталкивается обьект

    private void OnCollisionEnter2D(Collision2D collision) //срабатывает тогда, когда наш объект соприкасается с другим объектом:
    {
            isGround = true;
    }
    private void OnCollisionExit2D(Collision2D collision) //срабатывает тогда, когда соприкосновение двух объектов разрушается.
    {
            isGround = false;
    }

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
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
    }
    public void BoostSpeed() //метод для усиления скорости
    {
        speed += 0.1f;
    }
    public void Patrol() //потрулирование врага
    {
        isMoving = true;
        float patroldirectionX = player.transform.position.x - transform.localPosition.x;
        if (patrolCouldown >= 6f)
        {
            patrolCouldown = 0;
        }

        if (playerFollow == false && (patrolCouldown <= 3f) && Mathf.Abs(patroldirectionX) > 0.5f)
        {
            Vector3 patrolPos = transform.position;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            patrolPos.x -= Mathf.Sign(patroldirectionX) * speed * Time.deltaTime;
            transform.position = patrolPos;
            Debug.Log(patrolPos.x);
            
            if (theScale.x > 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            {
                Flip();
            }
        }
        if ((patrolCouldown > 3f) && (patrolCouldown <= 6f) && playerFollow == false && Mathf.Abs(patroldirectionX) > 0.5f)
        {
            Vector3 patrolPos = transform.position;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            patrolPos.x += Mathf.Sign(patroldirectionX) * speed * Time.deltaTime;
            transform.position = patrolPos;
            
            if (theScale.x < 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            {
                Flip();
            }
        }
    }
    public void PlayerFollow() //Метод в котором описываем логику следования за игроком
    {
            float directionX = player.transform.position.x - transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
            float directionY = player.transform.position.y - transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y

        if (Mathf.Abs(directionX) < 6 && Mathf.Abs(directionX) > 0.5f && Mathf.Abs(directionY) < 2)
        {
                Vector3 pos = transform.position;
                Vector3 theScale = transform.localScale;
                transform.localScale = theScale;
                float playerFollowSpeed = Mathf.Sign(directionX) * speed * Time.deltaTime;
                pos.x += playerFollowSpeed;
                transform.position = pos;
                isMoving = true;//Если этот метод выполняется переменная isMoving становиться правдой
                playerFollow = true;
                
                if (playerFollowSpeed < 0 && theScale.x < 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
                {
                    Flip();
                }
                else if (playerFollowSpeed > 0 && theScale.x > 0) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
                {
                    Flip();
                }
        }
        else
        {
            playerFollow = false;
        }                       
    }
    public void EnemyJump() //Прыжок если противник видит препятствие
    {
        RaycastHit2D wall = Physics2D.Raycast(wallChekPoint.position, transform.localPosition, 0.04f, LayerMask.GetMask("Ground")); //проверка что точка выставленная перед врагом точка видит в радиусе 0,04f перед собой обьект с слоем Земля 
        if (wall != false && isGround != false)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 20f), ForceMode2D.Impulse); // прикладывание силы по Y для пржыка
        }
    }
    private void DieByFall() //Метод который наносит урон при падении с платформы
    {
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity>().enemyDead == false)//если координаты игрока по оси y меньше 10 и враг не мертв, то происходит вызов метода GetDamage
        {
            this.gameObject.GetComponent<Entity>().TakeDamage(10);
        }
    }
    public void AnimState()//Метод для определения стейта анимации
    {
        if (isMoving != true) State = States.idle;//если не двигается значит анимации ожидания
        if (isMoving == true) State = States.run;//если координаты скелета поменялись, то State = run
    }
    public void groundCheckPosition()//проверка на пропость, чтобы скелет туда не упал
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 0.1f, LayerMask.GetMask("Ground"));//мы стреляем Raycast вниз с позиции обьекта groundcheck, на 2 еденицы
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
    }
    private void Start()
    {
        speed = SaveSerial.Instance.enemySpeed;
        if (speed < 1f)
        {
            speed = 1f;
        }
        speedRecovery = speed;
    }
    void Update() //тут складывать буду основные действия методы (который должен использовать враг)
    {
        patrolCouldown += Time.deltaTime;
        if (this.gameObject.GetComponent<Entity>().currentHP > 0)
        {
            PlayerFollow(); //движение за игроком
            DieByFall(); // Смерть при падении
            AnimState(); //Стейтмашина Анимации
            groundCheckPosition(); //проверка пропасти
            EnemyJump(); //прыжок перед препятсвием
          //  Patrol();//патрулирование
        }
        else
        {
            return;
        }
    }
    

   
    

}
