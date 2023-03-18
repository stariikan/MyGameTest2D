using UnityEngine;

public class Enemy_Behavior : MonoBehaviour //наследование класса сущности (то есть методы которые используются в Entity будут применены и к этому обьекту)
{
    //Параметры Скелета
    public float skeletonSpeed = 2f;//скорость Скелета
    private float blockCooldown; //кулдаун блока

    //Параметры Гриба
    public float moushroomSpeed = 2f;//скорость Гриба

    //Параметры Гриба
    public float flyingEyeSpeed = 2f;//скорость Гриба

    //Параметры Гоблина
    public float goblinSpeed = 3f;//скорость Гоблина
    public int remainingBombs = 3; //всего 3 бомб
    private bool jump = false;

    //Параметры Злого мага
    public float wizardSpeed = 2f;//скорость Гоблина
    private bool stuned = false; //стан обьекта
    public float stunCooldown; //кулдаун стана
                               
    //Параметры Самурай мага
    public float martialSpeed = 4f;//скорость Гоблина

    //Параметры Слайма
    public float slimeSpeed = 2f;//скорость Слайма

    //Параметры Босс Смерть
    public float deathSpeed = 2f;//скорость Смерти

    //Перемменая для записи разницы координат между игроком и врагом
    public float directionX;
    public float directionY;

    //Снаряды для атаки врагов
    [SerializeField] private GameObject[] ammo;

    //Общие параметры
    private float jumpCooldown; //кулдаун на отскок и прыжок
    private float physicCooldown = Mathf.Infinity; //кулдаун на физ атаку
    private float magicCooldown = Mathf.Infinity; //кулдаун на маг атаку

    
    public bool block = false;
    public bool copy; //этот обьект копия или нет?
    private bool movement = false; //моб не приследует игрока
    private bool playerIsAttack; //Атакует ли игрок?
    private bool isAttack; //Атакует ли обьект (враг)
    private float speedRecovery;//нужно для восстановление скорости 
    private int currentAttack = 0; //Кулдаун на атаку обьекта
    private float timeSinceAttack = 0.0f;//время с прошлой атаки нужно для комбо анимации атаки
    private int level; //проверка какой уровень проходит игрок, нужно для подключения способностей

    public GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной
    public Rigidbody2D rb; //Физическое тело
    private Animator anim; //Переменная благодаря которой анимирован обьект
    private float e_delayToIdle = 0.0f;
    new string tag; // к этой переменной присваивается тэг обьекта на старте
    public static Enemy_Behavior Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    private void Start()
    {
        Instance = this;
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
            moushroomSpeed = SaveSerial.Instance.mushroomSpeed;
            if (moushroomSpeed < 2f) moushroomSpeed = 2f;
            speedRecovery = moushroomSpeed;
        }
        if (tag == "Goblin")
        {
            goblinSpeed = SaveSerial.Instance.goblinSpeed;
            if (goblinSpeed < 2f) goblinSpeed = 2f;
            speedRecovery = goblinSpeed;
        }
        if (tag == "Martial")
        {
            martialSpeed = SaveSerial.Instance.martialSpeed;
            if (martialSpeed < 4f) martialSpeed = 4f;
            speedRecovery = martialSpeed;
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
    void Update()
    {
        timeSinceAttack += Time.deltaTime; //КД Атаки
        blockCooldown += Time.deltaTime; //КД Блока
        jumpCooldown += Time.deltaTime; //КД Прыжка
        magicCooldown += Time.deltaTime; //КД Маг умения
        physicCooldown += Time.deltaTime; //КД Физ умения
        stunCooldown += Time.deltaTime; //КД Стана



        if (this.gameObject.GetComponent<Entity_Enemy>().currentHP > 0) EnemyBehavior(); 
    }
    //Метод описывающий разное поведение для разных врагов. Выбор поведения завист от тега Обьекта
    public void EnemyBehavior()
    {
        AnimState();
        if (tag == "Skeleton")
        {
            EnemyMovement();
            SkeletonAttack();
            Block();
        }
        if (tag == "Mushroom")
        {
            EnemyMovement();
            MushroomAttack();
        }
        if (tag == "FlyingEye")
        {
            EnemyMovement();
            FlyingEyeAttack();
        }
        if (tag == "Goblin")
        {
            GoblinMovement();
            GoblinAttack();
            Block();
        }
        if (tag == "EvilWizard")
        {
            EnemyMovement();
            EvilWizardAttack();
        }
        if (tag == "Martial")
        {
            EnemyMovement();
            MartialAttack();
        }
        if (tag == "Slime")
        {
            SlimeMovement();
            SlimeAttack();
        }
        if (tag == "Death")
        {
            DeathMovement();
            DeathAttack();
        }
    }
    
    //Общие методы и поведения
    public enum States //Определения какие бывают состояния, указал названия как в Аниматоре Unity
    {
        idle,
        run
    }
    public void AnimState()//Метод для определения стейта анимации
    {
        if (movement == true)
        {
            e_delayToIdle = 0.05f;
            this.gameObject.GetComponent<Animator>().SetInteger("State", 1);
        }
        if (movement == false)
        {
            e_delayToIdle -= Time.deltaTime;
            if (e_delayToIdle < 0) this.gameObject.GetComponent<Animator>().SetInteger("State", 0);

        }
    }
    public void BoostEnemySpeed() //метод для усиления скорости врагов
    {
        skeletonSpeed *= 1.1f;
        moushroomSpeed *= 1.1f;
        goblinSpeed *= 1.1f;
    }
    public void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
    {
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
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
    public void Stun()
    {
        stunCooldown = 0;
        stuned = true;
        anim.SetBool("stun", true);
    }
    public void JumpToPlayer() //прыжок к игроку (Гриб / Слайм / Летающий глаз)
    {
        if (level >= 1) //способность активируется на 3 уровне
        {
            jumpCooldown = 0;
            Vector3 theScale = transform.localScale;
            transform.localScale = theScale;
            if (directionX > 0)
            {
                if (theScale.x < 0) Flip();//если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
                rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            }
            if (directionX < 0)
            {
                if (theScale.x > 0) Flip();//если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
                rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
            }
        }
    }
    
    //Особые скилы у мобов
    public void Block() // Использование щита (Скелет)
    {
        playerIsAttack = Hero.Instance.isAttack;
        if (playerIsAttack == true && (Mathf.Abs(directionX)) < 1.5f && Mathf.Abs(directionY) < 2 && level > 1)
        {
            blockCooldown = 0;
            skeletonSpeed = 0;
            block = true;
            anim.SetBool("Block", true);
        }
        if (blockCooldown > 0.4f || directionX > 2f)
        {
            skeletonSpeed = speedRecovery;
            block = false;
            anim.SetBool("Block", false);
        }
    }

    public void MushroomSpores() //создает облако спор которая дамажит игрока (Гриб)
    {
        if (level > 4)
        {
            magicCooldown = 0; // сброс таймера спор
            Vector3 MoushroomScale = transform.localScale; //взятие параметра поворота спрайта грибочка
            transform.localScale = MoushroomScale; //взятие параметра поворота спрайта грибочка
            Vector3 sporeSpawnPosition = this.gameObject.transform.position; //взятие позиции грибочка
            GameObject newSpore = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(sporeSpawnPosition.x, sporeSpawnPosition.y, sporeSpawnPosition.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            newSpore.name = "spore" + Random.Range(1, 999);
            if (MoushroomScale.x < 0) sporeSpawnPosition.x -= 0.8f; //перемещения сбор вперед грибочка в зависимости от поворота спрайта
            if (MoushroomScale.x > 0) sporeSpawnPosition.x += 0.8f; //перемещения сбор вперед грибочка в зависимости от поворота спрайта
            newSpore.GetComponent<Spore>().sporeDirection(sporeSpawnPosition); //передача координаты для спавна облака спор
        }
    }
    public void SummonCopy() //создает копии Летающего глаза
    {
        if (level > 4 && !copy)
        {
            magicCooldown = 0;
            Vector3 pos = transform.position;
            GameObject guard1 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1.5f, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            guard1.name = "Enemy" + Random.Range(1, 999);
            GameObject guard2 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 1f, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            guard2.name = "Enemy" + Random.Range(1, 999);
            GameObject guard3 = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(pos.x - 2f, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            guard3.name = "Enemy" + Random.Range(1, 999);
        }
        else return;

    }
    public void GoblinJumpToPlayer() //прыжок к игроку (Гоблин)
    {
        if (level >= 1) //способность активируется на 3 уровне
        {
            jumpCooldown = 0;
            if (directionX > 0) rb.AddForce(new Vector2(10, 2.5f), ForceMode2D.Impulse);
            if (directionX < 0) rb.AddForce(new Vector2(-10, 2.5f), ForceMode2D.Impulse);
        }
    }
    public void GoblinJumpFromPlayer() // отскок от игрока (Гоблин)
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
    public void GoblinBomb() //бросок бомбы (Гоблин)
    {
        if (level >= 5 && remainingBombs >= 1)
        {
            remainingBombs -= 1;
            magicCooldown = 0; // сброс таймера бомб
            Vector3 goblinScale = transform.localScale; //взятие параметра поворота спрайта гоблина
            transform.localScale = goblinScale; //взятие параметра поворота спрайта гоблина
            Vector3 bombSpawnPosition = this.gameObject.transform.position; //взятие позиции гоблина
            GameObject bombBall = Instantiate(ammo[Random.Range(0, ammo.Length)], new Vector3(bombSpawnPosition.x, bombSpawnPosition.y, bombSpawnPosition.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            bombBall.name = "Bomb" + Random.Range(1, 999);
            if (goblinScale.x < 0) bombSpawnPosition.x -= 1f; //перемещения бомбы вперед гоблина в зависимости от поворота спрайта
            if (goblinScale.x > 0) bombSpawnPosition.x += 1f;
            bombBall.GetComponent<Bomb>().GetEnemyName(this.gameObject.name);
            bombBall.GetComponent<Bomb>().bombDirection(bombSpawnPosition);  
        }
        if (level < 5) remainingBombs = 0;
    }
    public void MagicAttack() // EvilWizard FireBall
    {
        Vector3 shootingDirection = new Vector3(1, 0, 109);
        Vector3 pos = this.gameObject.transform.position;
        Debug.Log(pos);
        if (transform.localScale.x > 0)
        {
            shootingDirection = new Vector3(1, 0, 109);
            pos.x += 1;
        }
        if (transform.localScale.x < 0)
        {
            shootingDirection = new Vector3(-1, 0, 109);
            pos.x -= 1;
        }
        GameObject fireBall = Instantiate(ammo[0], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
        fireBall.name = "fireball" + Random.Range(1, 999);

        fireBall.GetComponent<FireBall>().SetDirection(shootingDirection);
    }
    public void DeathSummonMinioins() //призыв Слаймов (Босс Смерть)
    {
        if (physicCooldown >= 8)
        {
            physicCooldown = 0; // сброс таймера
            anim.SetTrigger("cast1");
            Vector3 spellSpawnPosition = this.gameObject.transform.position; //взятие позиции Игрока
            spellSpawnPosition.x -= 2f;
            SummonSlime.Instance.SummonDirection(spellSpawnPosition); //передача координаты для спавна магии
        }
    }
    public void SpellDrainHP() //исользования магии Кража жизней (Босс Смерть)
    {
        if (magicCooldown >= 3)
        {
            magicCooldown = 0; // сброс таймера
            anim.SetTrigger("cast1");
            Vector3 spellSpawnPosition = player.transform.position; //взятие позиции Игрока
            spellSpawnPosition.y += 1.7f; // нужно чтобы магия спавнилась чуть выше игрока
            DrainHP.Instance.DrainHPDirection(spellSpawnPosition); //передача координаты для спавна магии
        }
    }

    //Методы передвижения у разных врагов
    public void EnemyMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y
        if ((Mathf.Abs(directionX) < 5 && Mathf.Abs(directionX) > 1.5f && Mathf.Abs(directionY) < 2) && !block && !isAttack && !stuned || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack && !stuned || copy) //следует за игроком если маленькое растояние или получил урон
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float playerFollowSpeed = Mathf.Sign(directionX) * Time.deltaTime;
            if (tag == "Skeleton") playerFollowSpeed = Mathf.Sign(directionX) * skeletonSpeed * Time.deltaTime; //вычесление направления
            if (tag == "Mushroom") playerFollowSpeed = Mathf.Sign(directionX) * moushroomSpeed * Time.deltaTime; //вычесление направления
            if (tag == "FlyingEye") playerFollowSpeed = Mathf.Sign(directionX) * flyingEyeSpeed * Time.deltaTime; //вычесление направления
            if (tag == "Martial") playerFollowSpeed = Mathf.Sign(directionX) * martialSpeed * Time.deltaTime; //вычесление направления
            if (tag == "Slime") playerFollowSpeed = Mathf.Sign(directionX) * slimeSpeed * Time.deltaTime; //вычесление направления
            pos.x += playerFollowSpeed; //вычесление позиции по оси х
            transform.position = pos; //применение позиции
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();//если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();//если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        }
        else movement = false;
    }
    public void GoblinMovement()
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
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();//если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();//если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        }
        else movement = false;
    }
    public void DeathMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y
        if ((Mathf.Abs(directionX) < 4f && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 5f) //следует за игроком если маленькое растояние или получил урон
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float playerFollowSpeed = Mathf.Sign(directionX) * deathSpeed * Time.deltaTime; //вычесление направления
            pos.x -= playerFollowSpeed; //вычесление позиции по оси х
            transform.position = pos; //применение позиции
            movement = true;
        }
        else movement = false;
    }
    public void SlimeMovement()
    {
        directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
        directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y
        if (Mathf.Abs(directionX) > 1f && !block && !isAttack || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack) //следует за игроком если маленькое растояние или получил урон
        {
            Vector3 pos = transform.position; //позиция обьекта
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float playerFollowSpeed = Mathf.Sign(directionX) * Time.deltaTime;
            if (tag == "Skeleton") playerFollowSpeed = Mathf.Sign(directionX) * skeletonSpeed * Time.deltaTime; //вычесление направления
            if (tag == "Mushroom") playerFollowSpeed = Mathf.Sign(directionX) * moushroomSpeed * Time.deltaTime; //вычесление направления
            if (tag == "Slime") playerFollowSpeed = Mathf.Sign(directionX) * slimeSpeed * Time.deltaTime; //вычесление направления
            pos.x += playerFollowSpeed; //вычесление позиции по оси х
            transform.position = pos; //применение позиции
            movement = true;
            if (playerFollowSpeed < 0 && theScale.x > 0) Flip();//если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            else if (playerFollowSpeed > 0 && theScale.x < 0) Flip();//если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        }
        else movement = false;
    }
    //Методы атаки у разных мобов
    public void MushroomAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (stunCooldown > 3f) //выход из стана
        {
            stuned = false;
        }
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2 && !stuned) JumpToPlayer();
        if ((Mathf.Abs(directionX)) < 0.8f && magicCooldown > 10) MushroomSpores();
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
        else isAttack = false;
    }
    public void FlyingEyeAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (stunCooldown > 3f) //выход из стана
        {
            stuned = false;
        }
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2 && !stuned) JumpToPlayer();
        if ((Mathf.Abs(directionX)) < 5f && magicCooldown > 5) SummonCopy(); 
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
        else isAttack = false;
    }
    public void SkeletonAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && !block && timeSinceAttack > 1)
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
        else isAttack = false;
    }
    public void GoblinAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 5f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs < 1) GoblinJumpToPlayer();
        if ((Mathf.Abs(directionX)) < 2f && (Mathf.Abs(directionX)) > 1f && jumpCooldown >= 2 && Mathf.Abs(directionY) < 2 && remainingBombs >= 1) GoblinJumpFromPlayer();
        if ((Mathf.Abs(directionX)) < 4.5 && magicCooldown > 3 && !jump && remainingBombs >= 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && magicCooldown > 3 && !jump && remainingBombs >= 1)
        {
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            if (directionX < 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            {
                if (theScale.x > 0) Flip();
                GoblinBomb();
            }
            else if (directionX > 0) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
            {
                if (theScale.x < 0) Flip();
                GoblinBomb();
            }
        }
        if (jumpCooldown > 1.2f) jump = false;
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
        else isAttack = false;
    }
    public void EvilWizardAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (stunCooldown > 3f) //выход из стана
        {
            stuned = false;
            anim.SetBool("stun", false);
        }

        if (playerHP > 0 && Mathf.Abs(directionX) < 6f && (Mathf.Abs(directionX)) > 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2 && !stuned)
        {
            anim.SetTrigger("attack1");
            timeSinceAttack = 0.0f;
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            if (directionX < 0) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
            {
                if (theScale.x > 0) Flip();
                MagicAttack();
            }
            else if (directionX > 0) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
            {
                if (theScale.x < 0) Flip();
                MagicAttack();
            }
        }
        else isAttack = false;
        if (playerHP > 0 && (Mathf.Abs(directionX)) < 2f && Mathf.Abs(directionY) < 2 && !stuned)
        {
            anim.SetTrigger("attack2");
            timeSinceAttack = 0.0f;
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиции тумана по оси х
            float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиции тумана по оси y
            if ((Mathf.Abs(directionX) < 2f && Mathf.Abs(directionY) < 2f) && magicCooldown > 0.5 && playerHP > 0)
            {
                if (directionX < 0 && theScale.x > 0) Flip();
                else if (directionX > 0 && theScale.x < 0) Flip();
                timeSinceAttack = 0.0f;
                magicCooldown = 0;
                float fireDMG = 100f * (Entity_Enemy.Instance.wizardAttackDamage) * Time.deltaTime; 
                Hero.Instance.GetDamage(fireDMG);
            }
        }
    }
    public void MartialAttack()
    {
        float playerHP = Hero.Instance.hp;
        if (stunCooldown > 2f) //выход из стана
        {
            stuned = false;
        }
        if (playerHP > 0 && Mathf.Abs(directionX) < 2.5f && Mathf.Abs(directionY) < 1.5f && timeSinceAttack > 1 && !stuned)
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
        else isAttack = false;
    }
    public void SlimeAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2) JumpToPlayer();
        if (playerHP > 0 && Mathf.Abs(directionX) < 1.1f && Mathf.Abs(directionY) < 1f && timeSinceAttack > 1)
        {
            anim.SetTrigger("spin");
            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
    }
    public void DeathAttack()
    {
        float playerHP = Hero.Instance.hp;

        if (playerHP > 0 && Mathf.Abs(directionX) < 2f && Mathf.Abs(directionY) < 2f && timeSinceAttack > 2)
        {
            anim.SetTrigger("attack1");
            timeSinceAttack = 0.0f;
        }
        else isAttack = false;
        if ((Mathf.Abs(directionX)) < 8f && (Mathf.Abs(directionX)) > 2 && Mathf.Abs(directionY) < 2f || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true)
        {
            SpellDrainHP();
            DeathSummonMinioins();
        }
    }
}
