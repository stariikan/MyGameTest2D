using UnityEngine;

public class Entity_Enemy : MonoBehaviour
{
    //Параметры Скелета
    public float skeletonMaxHP = 70; //Максимальные жизни скелета
    public float skeletonAttackDamage = 10; // Урон от физ атаки
    public int skeletonReward = 2;//награда за победу над врагом
    private bool isBlock; //проверка поставлен ли блок
    private float blockDMG;

    //Параметры Гриба
    public float mushroomMaxHP = 70; //Максимальные жизни Гриба
    public float mushroomAttackDamage = 10; // Урон от физ атаки
    public int mushroomReward = 2;//награда за победу над врагом

    //Параметры Летающего глаза
    public float flyingEyeMaxHP = 70; //Максимальные жизни Летающего глаза
    public float flyingEyeAttackDamage = 10; // Урон от физ атаки
    public int flyingEyeReward = 2;//награда за победу над врагом

    //Параметры Гоблина
    public float goblinMaxHP = 50; //Максимальные жизни Гоблина
    public float goblinAttackDamage = 15; // Урон от физ атаки
    public int goblinReward = 2;//награда за победу над врагом

    //Параметры Злого мага
    public float wizardMaxHP = 50; //Максимальные жизни Гоблина
    public float wizardAttackDamage = 10; // Урон от физ атаки
    public int wizardReward = 2;//награда за победу над врагом

    //Параметры Самурая
    public float martialMaxHP = 75; //Максимальные жизни Гоблина
    public float martialAttackDamage = 20; // Урон от физ атаки
    public int martialReward = 2;//награда за победу над врагом

    //Параметры Слайма
    public float slimeMaxHP = 40;//Максимальные жизни Слайма
    public float slimeAttackDamage = 15; // Урон от физ атаки
    public int slimeReward = 1;//награда за победу над врагом

    //Параметры Босс Смерть
    public float deathMaxHP = 900;//Максимальные жизни Слайма
    public float deathAttackDamage = 25; // Урон от физ атаки
    public int deathReward = 40;//награда за победу над врагом

    //Перемменая для записи разницы координат между игроком и врагом
    private float directionY; 
    private float directionX;

    //Общие параметры
    public float currentHP; //Хп обьекта
    public float takedDamage; //разница между макс хп и полученным уроном
    public float enemyAttackRange = 1.2f; //Дальность физ атаки
    public bool enemyDead = false; //Мертвый ли обьект
    public bool enemyTakeDamage = false; //Получил ли обьект урон

    [SerializeField] private Transform firePoint; //Позиция из которых будет выпущены снаряди
    [SerializeField] private GameObject[] blood; //кровь
    public Vector3 lossyScale;
    public Vector3 thisObjectPosition;
    private Rigidbody2D e_rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    new string tag; // к этой переменной присваивается тэг на старте
    public static Entity_Enemy Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    private void Start()
    {
        Instance = this;
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
        e_rb = this.gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        tag = this.gameObject.transform.tag;

        if (tag == "Skeleton")
        {
            skeletonMaxHP = SaveSerial.Instance.skeletonHP;
            if (skeletonMaxHP == 0) skeletonMaxHP = 70;
            currentHP = skeletonMaxHP;
            skeletonAttackDamage = SaveSerial.Instance.skeletonDamage;
            if (skeletonAttackDamage == 0) skeletonAttackDamage = 10;
        }
        if (tag == "Mushroom")
        {
            mushroomMaxHP = SaveSerial.Instance.mushroomHP;
            if (mushroomMaxHP == 0) mushroomMaxHP = 70;
            currentHP = mushroomMaxHP;
            mushroomAttackDamage = SaveSerial.Instance.mushroomDamage;
            if (mushroomAttackDamage == 0) mushroomAttackDamage = 10;
        }
        if (tag == "FlyingEye")
        {
            flyingEyeMaxHP = SaveSerial.Instance.mushroomHP;
            if (flyingEyeMaxHP == 0) flyingEyeMaxHP = 70;
            currentHP = flyingEyeMaxHP;
            flyingEyeAttackDamage = SaveSerial.Instance.flyingEyeDamage;
            if (flyingEyeAttackDamage == 0) flyingEyeAttackDamage = 10;
        }
        if (tag == "Goblin")
        {
            goblinMaxHP = SaveSerial.Instance.goblinHP;
            if (goblinMaxHP == 0) goblinMaxHP = 50;
            currentHP = goblinMaxHP;
            goblinAttackDamage = SaveSerial.Instance.goblinDamage;
            if (goblinAttackDamage == 0) goblinAttackDamage = 15;
        }
        if (tag == "EvilWizard")
        {
            wizardMaxHP = SaveSerial.Instance.wizardHP;
            if (wizardMaxHP == 0) wizardMaxHP = 50;
            currentHP = wizardMaxHP;
            wizardAttackDamage = SaveSerial.Instance.wizardDamage;
            if (wizardAttackDamage == 0) wizardAttackDamage = 10;
        }
        if (tag == "Martial")
        {
            martialMaxHP = SaveSerial.Instance.martialHP;
            if (martialMaxHP == 0) martialMaxHP = 75;
            currentHP = martialMaxHP;
            martialAttackDamage = SaveSerial.Instance.martialDamage;
            if (martialAttackDamage == 0) martialAttackDamage = 20;
        }
        if (tag == "Slime")
        {
            if (slimeMaxHP == 0) slimeMaxHP = 40;
            currentHP = slimeMaxHP;
            if (slimeAttackDamage == 0) slimeAttackDamage = 15;
        }
        if (tag == "Death")
        {
            if (deathMaxHP == 0) deathMaxHP = 900;
            currentHP = deathMaxHP;
            if (deathAttackDamage == 0) deathAttackDamage = 25;
        }
    }
    //Секция где идет уселение характеристик врагов, если добавляется новый враг, тут нужно добавить его характеристики
    public void BoostEnemyHP() 
    {
        skeletonMaxHP *= 1.2f;
        mushroomMaxHP *= 1.2f;
        goblinMaxHP *= 1.2f;
        wizardMaxHP *= 1.2f;
        martialMaxHP *= 1.2f;
        flyingEyeMaxHP *= 1.2f;
    }
    public void BoostEnemyAttackDamage() //тут усиливыем урон
    {
        skeletonAttackDamage *= 1.2f;
        mushroomAttackDamage *= 1.2f;
        goblinAttackDamage *= 1.2f;
        wizardAttackDamage *= 1.2f;
        martialAttackDamage *= 1.2f;
        flyingEyeAttackDamage *= 1.2f;
    }
    public void BoostEnemyReward() //тут увеличиваем награду за убийство
    {
        skeletonReward += 2;
        mushroomReward += 2;
        goblinReward += 2;
        wizardReward += 2;
        martialReward += 2;
        flyingEyeReward += 2;
    }

    //Общие методы и поведения
    public void DamageDeealToPlayer() // Метод для нанесения урона Игроку
    {
        directionX = Enemy_Behavior.Instance.directionX;
        directionY = Enemy_Behavior.Instance.directionY;
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Skeleton")
        {
            Hero.Instance.GetDamage(skeletonAttackDamage);//тут мы получаем доступ к скрипту игрока и активируем оттуда функцию GetDamage
            float heal = skeletonAttackDamage * 0.5f; //Скелет ворует половину урона который наносит скелет игроку к себе в хп
            currentHP += heal;
            float healBar = heal / (float)skeletonMaxHP; //на сколько надо увеличить прогресс бар
            if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//обновление прогресс бара
        }
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Mushroom") Hero.Instance.GetDamage(mushroomAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "FlyingEye") Hero.Instance.GetDamage(mushroomAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Goblin") Hero.Instance.GetDamage(goblinAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Slime") Hero.Instance.GetDamage(slimeAttackDamage);
        if (directionX < 1.5f && currentHP > 0 && directionY < 1f && tag == "Martial") Hero.Instance.GetDamage(martialAttackDamage);
        if (directionX < 1.8f && currentHP > 0 && directionY < 1f && tag == "Death")
        {
            Hero.Instance.GetDamage(deathAttackDamage);
            float heal = deathAttackDamage * 0.5f; //Смерть ворует половину урона который наносит скелет игроку к себе в хп
            currentHP += heal;
            float healBar = heal / (float)deathMaxHP; //на сколько надо увеличить прогресс бар
            this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//обновление прогресс бара
        }
    }
    public void Push() //Метод для отталкивания тела
    {
        if (transform.lossyScale.x < 0) this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(-0.5f, e_rb.velocity.y), ForceMode2D.Impulse);
        else this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0.5f, e_rb.velocity.y), ForceMode2D.Impulse);
    }
    public void TakeDamage(float dmg) //Получение урона (в dmg указывается значение, в Hero скрипте при вызове метода TakeDamage в dmg записывается переменная дамага от оружия ) 
    {
        float maxHP = 1;
        if (tag == "Skeleton") maxHP = skeletonMaxHP;
        if (tag == "Mushroom") maxHP = mushroomMaxHP;
        if (tag == "FlyingEye") maxHP = flyingEyeMaxHP;
        if (tag == "Goblin") maxHP = goblinMaxHP;
        if (tag == "EvilWizard") maxHP = wizardMaxHP;
        if (tag == "Martial") maxHP = martialMaxHP;
        if (tag == "Slime") maxHP = slimeMaxHP;
        if (tag == "Death") maxHP = deathMaxHP;

        isBlock = this.gameObject.GetComponent<Enemy_Behavior>().block;
        //Debug.Log(isBlock);
        if (currentHP > 0 && !isBlock)
        {
            if (tag != "Skeleton")
            {
                GameObject bloodSpawn = Instantiate(blood[Random.Range(0, blood.Length)], new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity); //Клонирования обьекта
                bloodSpawn.gameObject.SetActive(true);
            }
            
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / maxHP; //на сколько надо уменьшаить прогресс бар
            anim.SetTrigger("damage");//анимация получения демейджа
            Enemy_Behavior.Instance.TakeDamageSound();
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//обновление прогресс бара
        }
        if (currentHP > 0 && isBlock)
        {
            int level = LvLGeneration.Instance.Level;
            if (level <= 4) blockDMG = dmg * 0.5f;//если Игрок ниже 5 уровня то 50% блокирования урона
            if (level >= 5) blockDMG = dmg * 0.1f;//если Игрок выше чем 4 уровеня то 90% блокирования урона
            currentHP -= blockDMG;
            Debug.Log(blockDMG);
            Enemy_Behavior.Instance.ShieldDamageSound();
            enemyTakeDamage = true;
            takedDamage = blockDMG / maxHP; //на сколько надо уменьшаить прогресс бар
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//обновление прогресс бара
        }
        if (currentHP <= 0)
        {
            int reward = 2;
            if (tag == "Skeleton") reward = skeletonReward;
            if (tag == "Mushroom") reward = mushroomReward;
            if (tag == "FlyingEye") reward = mushroomReward;
            if (tag == "Goblin") reward = goblinReward;
            if (tag == "Martial") reward = martialReward;
            if (tag == "Slime") reward = 1;
            if (tag == "Death") reward = 40;
            LvLGeneration.Instance.PlusCoin(reward);//вызов метода для увелечения очков
            e_rb.gravityScale = 0;
            e_rb.velocity = Vector2.zero;
            capsuleCollider.enabled = false;
            anim.StopPlayback();
            anim.SetBool("dead", true);
            anim.SetTrigger("m_death");//анимация смерти
            enemyDead = true;
        }
    }
    public virtual void Die() //Метод удаляет этот игровой обьект, вызывается через аниматор сразу после завершения анимации смерти
    {
        bool copy = this.gameObject.GetComponent<Enemy_Behavior>().copy;
        Destroy(this.gameObject);//уничтожить этот игровой обьект
        if (tag == "Skeleton") LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        if (tag == "Mushroom") LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        if (tag == "FlyingEye" && !copy) LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        if (tag == "Goblin") LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        if (tag == "EvilWizard") LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        if (tag == "Martial") LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        if (tag == "Slime")
        {
            GameObject[] deathObjects = GameObject.FindGameObjectsWithTag("Death");
            foreach (GameObject obj in deathObjects)
            {
                if (obj.name != "BossDeath")
                {
                    obj.GetComponent<Entity_Enemy>().BossDeathDamage(50);
                }
            }
        }
        if (tag == "Death") LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
    }

    //Методы атаки у разных мобов
    public void BossDeathHeal(float heal)
    {
        currentHP += heal;
        float healBar = heal / deathMaxHP; //на сколько надо увеличить прогресс бар
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//обновление прогресс бара
    }
    public void BossDeathDamage(float dmg)
    {
        currentHP -= dmg;
        enemyTakeDamage = true;
        takedDamage = dmg / deathMaxHP; //на сколько надо уменьшаить прогресс бар
        if (currentHP > 0) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//обновление прогресс бара
    }

}

