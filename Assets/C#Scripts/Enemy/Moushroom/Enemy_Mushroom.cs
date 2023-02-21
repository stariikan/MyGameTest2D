using UnityEngine;

public class Enemy_Mushroom : MonoBehaviour //наследование класса сущности (то есть методы которые используются в Entity будут применены и к этому обьекту)
{
    // Start is called before the first frame update
    [SerializeField] public float speed = 2f;//параметр скорости скелета
    [SerializeField] private float speedRecovery;//параметр скорости скелета 2 параметр нужен для восстановление скорости по умолчанию (после остановки перед пропастью и наверное после замедлений которых я еще не придумал))

    GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной
    public Rigidbody2D rb; //Физическое тело
    public Transform groundcheck;// проверка соприкасается ли метка (которую мы создали с землей)
    private Animator anim; //Переменная благодаря которой анимирован обьект, переменная = skelet_anim
    private float e_delayToIdle = 0.0f;
    public Transform wallChekPoint; //Тут мы ссылаемся на точку которая является дочерним обьектом 
    private bool isGround; // находиться ли обьект на земле
    private bool playerFollow = false; //моб не приследует игрока
    RaycastHit2D hit; //тут будем получать информацию с чем сталкивается обьект

    public float directionX; //переменная для понимания разницы между игроком и врагом
    public float directionY; //переменная для понимания разницы между игроком и врагом

    private bool playerIsAttack; //Атакует ли игрок?
    private int currentAttack = 0; //Кулдаун на атаку обьекта
    private float timeSinceAttack = 0.0f;//время с прошлой атаки нужно для комбо анимации атаки

    private float jumpCooldown; //кулдаун на отскок и прыжок
    private float sporesCooldown = 10f; //кулдаун атаки спор
    private int level; //проверка какой уровень проходит игрок, нужно для подключения способностей
    public static Enemy_Mushroom Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    private void Start()
    {
        player = GameObject.FindWithTag("PlayerCharacter"); //тут при старте игры скелет находит игрока по тегу Player и присваивает найденную и информацию переменной player
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object) к которому привязан скрипт
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object) к которому привязан скрипт
        speed = SaveSerial.Instance.moushroomSpeed;
        if (speed < 2f)
        {
            speed = 2f;
        }
        speedRecovery = speed;
        Instance = this;
        level = LvLGeneration.Instance.Level;
    }
    void Update() //тут складывать буду основные действия методы (который должен использовать враг)
    {
        jumpCooldown += Time.deltaTime;
        sporesCooldown += Time.deltaTime;
        timeSinceAttack += Time.deltaTime;
        if (this.gameObject.GetComponent<Entity_Mushroom>().currentHP > 0)
        {
            PlayerFollow(); //движение за игроком
            DieByFall(); // Смерть при падении
            AnimState(); //Стейтмашина Анимации
            groundCheckPosition(); //проверка пропасти
            EnemyJump(); //прыжок перед препятсвием
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
    private States State //Создание стейтмашины, переменная = State. Значение состояния может быть передано или изминено извне благодаря get и set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
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
        if (level >= 3) //способность активируется на 3 уровне
        {
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            jumpCooldown = 0;
            float jumpToPlayer = Mathf.Sign(directionX) * 3000 * Time.deltaTime;
            rb.AddForce(new Vector2(jumpToPlayer, 2.5f), ForceMode2D.Impulse);
        }
    }
    public void ReboundFromTarget() // отскок от игрока
    {
        playerIsAttack = Hero.Instance.isAttack;
        if(playerIsAttack == true && level >= 2)
        {

            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            jumpCooldown = 0;
            float jumpToPlayer = Mathf.Sign(directionX) * 6;
            rb.AddForce(new Vector2(-jumpToPlayer, 1f), ForceMode2D.Impulse);
        }
    }
    public void MushroomSpores() //создает облако спор которая дамажит игрока
    {
        if (level >= 5)
        {
            sporesCooldown = 0;
            Spore.Instance.sporeDirection(this.gameObject.transform.position);
        }
    }
    public void PlayerFollow() //Метод в котором описываем логику следования за игроком
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y
        if ((Mathf.Abs(directionX) < 3 && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) || this.gameObject.GetComponent<Entity_Mushroom>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f) //следует за игроком если маленькое растояние или получил урон
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float playerFollowSpeed = Mathf.Sign(directionX) * speed * Time.deltaTime; //вычесление направления
            pos.x += playerFollowSpeed; //вычесление позиции по оси х
            transform.position = pos; //применение позиции
            playerFollow = true;
        
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
            playerFollow = false;
        }
    }
    public void Attack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2)
        {
            JumpToPlayer();
        }
        if ((Mathf.Abs(directionX)) < 1.5f && jumpCooldown > 2 && Mathf.Abs(directionY) < 2)
        {
            //ReboundFromTarget();
        }
        if ((Mathf.Abs(directionX)) < 1f && sporesCooldown > 10)
        {
            MushroomSpores();
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
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
        if (playerFollow == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if(playerFollow == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0)
            this.gameObject.GetComponent<Animator>().SetInteger("State", 0);
        }
    }
    //Проверка на пропость, чтобы скелет туда не упал мы стреляем Raycast вниз с позиции обьекта groundcheck, на 2 еденицы и проверяем столкнулся ли обьект с землей (groundLayers) PlayerFollow();
    public void groundCheckPosition()
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
