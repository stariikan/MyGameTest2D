using UnityEngine;

public class Enemy_Behavior : MonoBehaviour //наследование класса сущности (то есть методы которые используются в Entity будут применены и к этому обьекту)
{
    public float skeletonSpeed = 2f;//скорость Скелета
    private float blockCooldown;
    public bool skeleton_block = false;

    public float moushroomSpeed = 2f;//скорость Гриба
    
    private float sporesCooldown = 10f; //кулдаун атаки спор

    public float goblinSpeed = 3f;//скорость Гоблина
    private float bombCooldown = 4f; //кулдаун броска бомбы
    public int remainingBombs = 3; //всего 3 бомб
    private bool jump = false;

    public float slimeSpeed = 2f;//скорость Слайма

    public float deathSpeed = 2f;//скорость Смерти

    private float speedRecovery;//нужно для восстановление скорости 

    private float jumpCooldown; //кулдаун на отскок и прыжок
    private bool movement = false; //моб не приследует игрока
    private bool playerIsAttack; //Атакует ли игрок?
    private bool isAttack; //Атакует ли обьект (враг)

    public float directionX; //переменная для понимания разницы между игроком и врагом
    public float directionY; //переменная для понимания разницы между игроком и врагом
    private int currentAttack = 0; //Кулдаун на атаку обьекта
    private float timeSinceAttack = 0.0f;//время с прошлой атаки нужно для комбо анимации атаки
    private int level; //проверка какой уровень проходит игрок, нужно для подключения способностей

    GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной
    public Rigidbody2D rb; //Физическое тело
    private Animator anim; //Переменная благодаря которой анимирован обьект, переменная = skelet_anim
    private float e_delayToIdle = 0.0f;
    string tag; // к этой переменной присваивается тэг на старте

    public static Enemy_Behavior Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    private void Start()
    {
        Instance = this;
        player = GameObject.FindWithTag("PlayerCharacter"); //тут при старте игры скелет находит игрока по тегу Player и присваивает найденную и информацию переменной player
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object) к которому привязан скрипт
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object) к которому привязан скрипт
        tag = this.gameObject.transform.tag;
        level = LvLGeneration.Instance.Level;

        if (tag == "Skeleton")
        {
            skeletonSpeed = SaveSerial.Instance.skeletonSpeed;
            if (skeletonSpeed < 2f) skeletonSpeed = 2f;
            speedRecovery = skeletonSpeed;

        }
        if (tag == "Mushroom")
        {
            moushroomSpeed = SaveSerial.Instance.moushroomSpeed;
            if (moushroomSpeed < 2f) moushroomSpeed = 2f;
            speedRecovery = moushroomSpeed;
        }
        if (tag == "Goblin")
        {
            goblinSpeed = SaveSerial.Instance.goblinSpeed;
            if (goblinSpeed < 2f) goblinSpeed = 2f;
            speedRecovery = goblinSpeed;
        }
        if (tag == "Slime")
        {
            if (slimeSpeed < 2f) slimeSpeed = 2f;
            speedRecovery = slimeSpeed;
        }
        if (tag == "Death")
        {
            if (deathSpeed < 2f) deathSpeed = 2f;
            speedRecovery = deathSpeed;
        }
    }
    void Update() //тут складывать буду основные действия методы (который должен использовать враг)
    {
        timeSinceAttack += Time.deltaTime;
        blockCooldown += Time.deltaTime;
        jumpCooldown += Time.deltaTime;
        sporesCooldown += Time.deltaTime;
        bombCooldown += Time.deltaTime;

        if (this.gameObject.GetComponent<Entity_Enemy>().currentHP > 0)
        {
            DieByFall(); // Смерть при падении
            AnimState(); //Стейтмашина Анимации
            EnemyBehavior(); //Поведения врага
        }
        else
        {
            return;
        }
    }
    public void EnemyBehavior()
    {
        if (tag == "Skeleton")
        {
            EnemyMovement();
            SkeletonAttack();
            Block();
        }
        if (tag == "Mushroom")
        {
            EnemyMovement();
            MoushroomAttack();
        }
        if (tag == "Goblin")
        {
            GoblinMovement();
            GoblinAttack();
        }
        if (tag == "Slime")
        {
            EnemyMovement();
            SlimeAttack();
        }
        if (tag == "Death")
        {
            EnemyMovement();
            DeathAttack();
        }
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
    public void BoostEnemySpeed() //метод для усиления скорости врагов
    {
        skeletonSpeed *= 1.1f;
        moushroomSpeed *= 1.1f;
        goblinSpeed *= 1.1f;
    }
    public void Block() // Использование щита
    {
        playerIsAttack = Hero.Instance.isAttack;
        if (playerIsAttack == true && (Mathf.Abs(directionX)) < 1.5f && Mathf.Abs(directionY) < 2 && level > 1)
        {
            blockCooldown = 0;
            skeletonSpeed = 0;
            skeleton_block = true;
            anim.SetBool("Block", true);
        }
        if (blockCooldown > 0.4f || directionX > 2f)
        {
            skeletonSpeed = speedRecovery;
            skeleton_block = false;
            anim.SetBool("Block", false);
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
    public void EnemyMovement() //Метод в котором описываем логику следования за игроком
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y
        if ((Mathf.Abs(directionX) < 5 && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) && !skeleton_block && !isAttack || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !skeleton_block && !isAttack) //следует за игроком если маленькое растояние или получил урон
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float playerFollowSpeed = Mathf.Sign(directionX) * Time.deltaTime;
            if (tag == "Skeleton")
            {
                playerFollowSpeed = Mathf.Sign(directionX) * skeletonSpeed * Time.deltaTime; //вычесление направления

            }
            if (tag == "Mushroom")
            {
                playerFollowSpeed = Mathf.Sign(directionX) * moushroomSpeed * Time.deltaTime; //вычесление направления
            }
            if (tag == "Slime")
            {
                playerFollowSpeed = Mathf.Sign(directionX) * slimeSpeed * Time.deltaTime; //вычесление направления
            }
            if (tag == "Death")
            {
                playerFollowSpeed = Mathf.Sign(directionX) * deathSpeed * Time.deltaTime;
            }
            pos.x += playerFollowSpeed; //вычесление позиции по оси х
            transform.position = pos; //применение позиции
            movement = true;
        
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
            movement = false;
        }
    }
    public void SkeletonAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && !skeleton_block && timeSinceAttack > 1)
        {
            isAttack = true;
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
            isAttack = false;
        }
    }
    private void DieByFall() //Метод который наносит урон при падении с платформы
    {
        if (rb.transform.position.y < -100 && this.gameObject.GetComponent<Entity_Enemy>().enemyDead == false)//если координаты игрока по оси y меньше 10 и враг не мертв, то происходит вызов метода GetDamage
        {
            this.gameObject.GetComponent<Entity_Enemy>().TakeDamage(10);
        }
    }
    public void AnimState()//Метод для определения стейта анимации
    {      
        if (movement == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if(movement == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0)
            this.gameObject.GetComponent<Animator>().SetInteger("State", 0);
        }
    }
    public void MoushroomJumpToPlayer() //прыжок к игроку
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
    public void MushroomSpores() //создает облако спор которая дамажит игрока
    {
        if (level >= 5)
        {
            sporesCooldown = 0; // сброс таймера спор
            Vector3 MoushroomScale = transform.localScale; //взятие параметра поворота спрайта грибочка
            transform.localScale = MoushroomScale; //взятие параметра поворота спрайта грибочка
            Vector3 sporeSpawnPosition = this.gameObject.transform.position; //взятие позиции грибочка
            if (MoushroomScale.x < 0) sporeSpawnPosition.x -= 0.8f; //перемещения сбор вперед грибочка в зависимости от поворота спрайта
            if (MoushroomScale.x > 0) sporeSpawnPosition.x += 0.8f; //перемещения сбор вперед грибочка в зависимости от поворота спрайта
            Spore.Instance.sporeDirection(sporeSpawnPosition); //передача координаты для спавна облака спор
        }
    }
    public void MoushroomAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2)
        {
            MoushroomJumpToPlayer();
        }
        if ((Mathf.Abs(directionX)) < 0.8f && sporesCooldown > 10)
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
    public void GoblinJumpToPlayer() //прыжок к игроку
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
    public void GoblinJumpFromPlayer() // отскок от игрока
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
        if (level < 5)
        {
            remainingBombs = 0;
        }
    }
    public void GoblinMovement() //Метод в котором описываем логику следования за игроком
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 3f && Mathf.Abs(directionY) < 2) && remainingBombs < 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) //следует за игроком если маленькое растояние или получил урон
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float playerFollowSpeed = Mathf.Sign(directionX) * goblinSpeed * Time.deltaTime; //вычесление направления
            pos.x += playerFollowSpeed; //вычесление позиции по оси х
            transform.position = pos; //применение позиции
            movement = true;
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
            movement = false;
        }
    }
    public void GoblinAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 5f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs < 1)
        {
            GoblinJumpToPlayer();
        }
        if ((Mathf.Abs(directionX)) < 2f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs >= 1)
        {
            GoblinJumpFromPlayer();
        }
        if ((Mathf.Abs(directionX)) < 4.5 && bombCooldown > 3 && !jump && remainingBombs >= 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && bombCooldown > 3 && !jump && remainingBombs >= 1)
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float RunSpeed = Mathf.Sign(directionX) * goblinSpeed * Time.deltaTime; //вычесление направления
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
    public void SlimeAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2)
        {
            MoushroomJumpToPlayer();
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            anim.SetTrigger("spin");
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else
        {
            return;
        }
    }
    public void DeathAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2)
        {
            
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            anim.SetTrigger("attack1");
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else
        {
            return;
        }
    }
}
