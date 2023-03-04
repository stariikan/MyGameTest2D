using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Goblin : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3f;//параметр скорости скелета
    [SerializeField] private float speedRecovery;//параметр скорости скелета 2 параметр нужен для восстановление скорости по умолчанию (после остановки перед пропастью и наверное после замедлений которых я еще не придумал))

    GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной
    public Rigidbody2D rb; //Физическое тело
    public Transform groundcheck;// проверка соприкасается ли метка (которую мы создали с землей)
    private Animator anim; //Переменная благодаря которой анимирован обьект, переменная = skelet_anim
    private float e_delayToIdle = 0.0f;
    public Transform wallChekPoint; //Тут мы ссылаемся на точку которая является дочерним обьектом 
    private bool isGround; // находиться ли обьект на земле
    private bool goblinRun = false; //моб не убегает от игрока
    RaycastHit2D hit; //тут будем получать информацию с чем сталкивается обьект

    public float directionX; //переменная для понимания разницы между игроком и врагом
    public float directionY; //переменная для понимания разницы между игроком и врагом

    private int currentAttack = 0; //Кулдаун на атаку обьекта
    private float timeSinceAttack = 0.0f;//время с прошлой атаки нужно для комбо анимации атаки

    private float jumpCooldown; //кулдаун на отскок и прыжок
    private int level; //проверка какой уровень проходит игрок, нужно для подключения способностей

    private bool jump = false;
    private float bombCooldown = 4f; //кулдаун броска бомбы
    public int remainingBombs = 3; //всего 3 бомб
    public static Enemy_Goblin Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    private void Start()
    {
        player = GameObject.FindWithTag("PlayerCharacter"); //тут при старте игры скелет находит игрока по тегу Player и присваивает найденную и информацию переменной player
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object) к которому привязан скрипт
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object) к которому привязан скрипт
        speed = SaveSerial.Instance.moushroomSpeed;
        if (speed < 3f)
        {
            speed = 3f;
        }
        speedRecovery = speed;
        Instance = this;
        level = LvLGeneration.Instance.Level;
    }
    void Update() //тут складывать буду основные действия методы (который должен использовать враг)
    {
        jumpCooldown += Time.deltaTime;
        bombCooldown += Time.deltaTime;
        timeSinceAttack += Time.deltaTime;
        if (this.gameObject.GetComponent<Entity_Goblin>().currentHP > 0)
        {
            GoblinMovement(); //движение от игрока
            DieByFall(); // Смерть при падении
            AnimState(); //Стейтмашина Анимации
            GroundCheckPosition(); //проверка пропасти
            //EnemyJump(); //прыжок перед препятсвием
            Attack(); //Атака
        }
        else
        {
            return;
        }
    }
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
    public void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
    {
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
    }
    public void BoostSpeed() //метод для усиления скорости
    {
        speed *= 1.1f;
    }
    public void JumpToPlayer() //прыжок к игроку
    {
        if (level >= 1) //способность активируется на 3 уровне
        {
            jumpCooldown = 0;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (directionX > 0)
            {
                if (theScale.x < 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
                {
                    Flip();
                }
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                if (theScale.x > 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
                {
                    Flip();
                }
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
            }
        }
    }
    public void JumpFromPlayer() // отскок от игрока
    {
        if (level >= 1) //способность активируется на 3 уровне
        {
            jumpCooldown = 0;
            if (directionX > 0)
            {
                jump = true;
                anim.SetTrigger("roll");
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                jump = true;
                anim.SetTrigger("roll");
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
        }
    }
    public void PushFromPlayer() // отскок от игрока
    {
        if (Mathf.Abs(directionX) < 1f)
        {
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (theScale.x > 0) rb.AddForce(new Vector2(-5, 1.5f), ForceMode2D.Impulse);
            if (theScale.x < 0) rb.AddForce(new Vector2(5, 1.5f), ForceMode2D.Impulse);
        }
    }
    public void GoblinBomb() //бросок бомбы
    {
        if (level >= 5 && remainingBombs >= 1)
        {
            remainingBombs -= 1;
            bombCooldown = 0; // сброс таймера бомб
            Vector3 goblinScale = transform.localScale; //взятие параметра поворота спрайта гоблина
            transform.localScale = goblinScale; //взятие параметра поворота спрайта гоблина
            Vector3 bombSpawnPosition = this.gameObject.transform.position; //взятие позиции гоблина
            if (goblinScale.x < 0) bombSpawnPosition.x -= 1f; //перемещения бомбы вперед гоблина в зависимости от поворота спрайта
            if (goblinScale.x > 0) bombSpawnPosition.x += 1f; 
            Bomb.Instance.bombDirection(bombSpawnPosition); //передача координаты для спавна бомбы
        }
        if(level < 5)
        {
            remainingBombs = 0;
        }
    }
    public void GoblinMovement() //Метод в котором описываем логику следования за игроком
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 3f && Mathf.Abs(directionY) < 2) && remainingBombs < 1 || this.gameObject.GetComponent<Entity_Goblin>().enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) //следует за игроком если маленькое растояние или получил урон
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float playerFollowSpeed = Mathf.Sign(directionX) * speed * Time.deltaTime; //вычесление направления
            pos.x += playerFollowSpeed; //вычесление позиции по оси х
            transform.position = pos; //применение позиции
            goblinRun = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            {
                Flip();
            }
            else if (playerFollowSpeed > 0 && theScale.x < 0) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
            {
                Flip();
            }
        }
        else
        {
            goblinRun = false;
        }
    }
    public void Attack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 5f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs < 1)
        {
            JumpToPlayer();
        }
        if ((Mathf.Abs(directionX)) < 2f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs >= 1)
        {
            JumpFromPlayer();
        }
        if ((Mathf.Abs(directionX)) < 6f && bombCooldown > 3 && !jump && remainingBombs >=1 || this.gameObject.GetComponent<Entity_Goblin>().enemyTakeDamage == true && bombCooldown > 3 && !jump && remainingBombs >= 1)
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float RunSpeed = Mathf.Sign(directionX) * speed * Time.deltaTime; //вычесление направления
            if (theScale.x > 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            {
                Flip();
                GoblinBomb();
            }
            else if (theScale.x < 0) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
            {
                Flip();
                GoblinBomb();
            }
        }
        if (jumpCooldown > 2.1f)
        {
            jump = false;
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.5f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            //Damage Deal
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 2.0f)
                currentAttack = 1;
            anim.SetTrigger("attack" + currentAttack);
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else
        {
            return;
        }
    }
    public void EnemyJump() //Прыжок если противник видит препятствие
    {
        RaycastHit2D wall = Physics2D.Raycast(wallChekPoint.position, transform.localPosition, 0.04f, LayerMask.GetMask("Ground")); //проверка что точка выставленная перед врагом точка видит в радиусе 0,04f перед собой обьект с слоем Земля 
        if (wall != false && isGround != false)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(rb.velocity.x, 1f), ForceMode2D.Impulse); // прикладывание силы по Y для пржыка
        }
    }
    private void DieByFall() //Метод который наносит урон при падении с платформы
    {
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity_Mushroom>().enemyDead == false)//если координаты игрока по оси y меньше 10 и враг не мертв, то происходит вызов метода GetDamage
        {
            this.gameObject.GetComponent<Entity_Mushroom>().TakeDamage(10);
        }
    }
    public void AnimState()//Метод для определения стейта анимации
    {
        if (goblinRun == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if (goblinRun == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0)
                this.gameObject.GetComponent<Animator>().SetInteger("State", 0);
        }
    }
    //Проверка на пропость, чтобы скелет туда не упал мы стреляем Raycast вниз с позиции обьекта groundcheck, на 2 еденицы и проверяем столкнулся ли обьект с землей (groundLayers) PlayerFollow();
    public void GroundCheckPosition()
    {
        hit = Physics2D.Raycast(groundcheck.position, -transform.up, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.collider != true) //если обьект groundcheck не столкнулся с полом (то есть пропасть)
        {
            speed = 0f;//то уменьшаем скоро до 0
        }
        else
        {
            speed = speedRecovery;//если обьект ground check вновь сталкивается с полом (видит землю), то возвращаем показатель скорости.
        }
    }
}
