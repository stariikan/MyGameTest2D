using UnityEngine;

public class Enemy_Behavior : MonoBehaviour //наследование класса сущности (то есть методы которые используются в Entity будут применены и к этому обьекту)
{
    //Параметры Скелета
    public float skeletonSpeed = 2f;//скорость Скелета
    private float blockCooldown; //кулдаун блока

    //Параметры Гриба
    public float moushroomSpeed = 2f;//скорость Гриба
    private float sporesCooldown = 10f; //кулдаун атаки спор

    //Параметры Гоблина
    public float goblinSpeed = 3f;//скорость Гоблина
    private float bombCooldown = 4f; //кулдаун броска бомбы
    public int remainingBombs = 3; //всего 3 бомб
    private bool jump = false;

    //Параметры Слайма
    public float slimeSpeed = 2f;//скорость Слайма

    //Параметры Босс Смерть
    public float deathSpeed = 2f;//скорость Смерти
    private float summonCooldown; //кулдаун вызова Слайма
    private float drainHPCooldown; //кулдаун Магии

    //Перемменая для записи разницы координат между игроком и врагом
    public float directionX;
    public float directionY;

    //Общие параметры
    private float jumpCooldown; //кулдаун на отскок и прыжок
    public bool block = false;
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
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        blockCooldown += Time.deltaTime;
        jumpCooldown += Time.deltaTime;
        sporesCooldown += Time.deltaTime;
        bombCooldown += Time.deltaTime;
        summonCooldown += Time.deltaTime;
        drainHPCooldown += Time.deltaTime;

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
            MoushroomAttack();
        }
        if (tag == "Goblin")
        {
            GoblinMovement();
            GoblinAttack();
            Block();
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
    public void MoushroomJumpToPlayer() //прыжок к игроку (Гриб и Слайм)
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
    public void MushroomSpores() //создает облако спор которая дамажит игрока (Гриб)
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
            bombCooldown = 0; // сброс таймера бомб
            Vector3 goblinScale = transform.localScale; //взятие параметра поворота спрайта гоблина
            transform.localScale = goblinScale; //взятие параметра поворота спрайта гоблина
            Vector3 bombSpawnPosition = this.gameObject.transform.position; //взятие позиции гоблина
            if (goblinScale.x < 0) bombSpawnPosition.x -= 1f; //перемещения бомбы вперед гоблина в зависимости от поворота спрайта
            if (goblinScale.x > 0) bombSpawnPosition.x += 1f;
            Bomb.Instance.bombDirection(bombSpawnPosition); //передача координаты для спавна бомбы
        }
        if (level < 5) remainingBombs = 0;
    }
    public void DeathSummonMinioins() //призыв Слаймов (Босс Смерть)
    {
        if (summonCooldown >= 8)
        {
            summonCooldown = 0; // сброс таймера
            anim.SetTrigger("cast1");
            Vector3 spellSpawnPosition = this.gameObject.transform.position; //взятие позиции Игрока
            spellSpawnPosition.x -= 2f;
            SummonSlime.Instance.SummonDirection(spellSpawnPosition); //передача координаты для спавна магии
        }
    }
    public void SpellDrainHP() //исользования магии Кража жизней (Босс Смерть)
    {
        if (drainHPCooldown >= 3)
        {
            drainHPCooldown = 0; // сброс таймера
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
        if ((Mathf.Abs(directionX) < 5 && Mathf.Abs(directionX) > 1f && Mathf.Abs(directionY) < 2) && !block && !isAttack || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && Mathf.Abs(directionX) > 1f && !block && !isAttack) //следует за игроком если маленькое растояние или получил урон
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
    public void MoushroomAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2) MoushroomJumpToPlayer();
        if ((Mathf.Abs(directionX)) < 0.8f && sporesCooldown > 10) MushroomSpores();
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
        if ((Mathf.Abs(directionX)) < 4.5 && bombCooldown > 3 && !jump && remainingBombs >= 1 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true && bombCooldown > 3 && !jump && remainingBombs >= 1)
        {
            Vector3 theScale = transform.localScale; //нужно для понимания направления
            transform.localScale = theScale; //нужно для понимания направления
            Debug.Log(directionX);
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
    public void SlimeAttack()
    {
        float playerHP = Hero.Instance.hp;
        if ((Mathf.Abs(directionX)) < 4.5f && (Mathf.Abs(directionX)) > 2 && jumpCooldown >= 3 && Mathf.Abs(directionY) < 2) MoushroomJumpToPlayer();
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
        if ((Mathf.Abs(directionX)) < 8f && (Mathf.Abs(directionX)) > 2 && Mathf.Abs(directionY) < 2 || this.gameObject.GetComponent<Entity_Enemy>().enemyTakeDamage == true)
        {
            SpellDrainHP();
            DeathSummonMinioins();
        }
    }
}
