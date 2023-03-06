using UnityEngine;

public class Entity_Enemy : MonoBehaviour
{
    public float skeletonMaxHP = 50; //Максимальные жизни скелета
    public float skeletonAttackDamage = 15; // Урон от физ атаки
    public int skeletonReward = 2;//награда за победу над врагом
    private bool isBlock; //проверка поставлен ли блок
    private float blockDMG;



    public float moushroomMaxHP = 50; //Максимальные жизни Гриба
    public float moushroomAttackDamage = 15; // Урон от физ атаки
    public int moushroomReward = 2;//награда за победу над врагом


    public float goblinMaxHP = 35; //Максимальные жизни Гоблина
    public float goblinAttackDamage = 25; // Урон от физ атаки
    public int goblinReward = 2;//награда за победу над врагом

    public float slimeMaxHP = 20;//Максимальные жизни Слайма
    public float slimeAttackDamage = 15; // Урон от физ атаки
    public int slimeReward = 1;//награда за победу над врагом

    public float deathMaxHP = 150;//Максимальные жизни Слайма
    public float deathAttackDamage = 50; // Урон от физ атаки
    public int deathReward = 40;//награда за победу над врагом

    private float playerHP;
    //Перемменая для записи разницы координат между игроком и врагом
    private float directionY; 
    private float directionX;

    public float currentHP;
    public float takedDamage; //разница между макс хп и полученным уроном
    public float enemyAttackRange = 1.2f; //Дальность физ атаки

    public bool enemyDead = false;
    public bool enemyTakeDamage = false;
    
    public Transform enemyAttackPoint; //Тут мы ссылаемся на точку которая является дочерним (нужна для реализации физ атаки)
    
    public LayerMask playerLayers;
    public Vector3 lossyScale;
    private Rigidbody2D e_rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    string tag; // к этой переменной присваивается тэг на старте
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
            if (skeletonMaxHP == 0)
            {
                skeletonMaxHP = 50;
            }
            currentHP = skeletonMaxHP;
            skeletonAttackDamage = SaveSerial.Instance.skeletonDamage;
            if (skeletonAttackDamage == 0)
            {
                skeletonAttackDamage = 15;
            }
        }
        if (tag == "Mushroom")
        {
            moushroomMaxHP = SaveSerial.Instance.moushroomHP;
            if (moushroomMaxHP == 0)
            {
                moushroomMaxHP = 50;
            }
            currentHP = moushroomMaxHP;
            moushroomAttackDamage = SaveSerial.Instance.moushroomDamage;
            if (moushroomAttackDamage == 0)
            {
                moushroomAttackDamage = 15;
            }
        }
        if (tag == "Goblin")
        {
            goblinMaxHP = SaveSerial.Instance.goblinHP;
            if (goblinMaxHP == 0)
            {
                goblinMaxHP = 35;
            }
            currentHP = goblinMaxHP;
            goblinAttackDamage = SaveSerial.Instance.goblinDamage;
            if (goblinAttackDamage == 0)
            {
                goblinAttackDamage = 25;
            }
        }
        if (tag == "Slime")
        {
            if (slimeMaxHP == 0)
            {
                slimeMaxHP = 35;
            }
            currentHP = slimeMaxHP;
            if (slimeAttackDamage == 0)
            {
                slimeAttackDamage = 15;
            }
        }
        if (tag == "Death")
        {
            if (deathMaxHP == 0)
            {
                deathMaxHP = 150;
            }
            currentHP = deathMaxHP;
            if (deathAttackDamage == 0)
            {
                deathAttackDamage = 50;
            }
        }
    }
    public void DamageDeealToPlayer()
    {
        directionX = Enemy_Behavior.Instance.directionX;
        directionY = Enemy_Behavior.Instance.directionY;
        if(directionX < 0.8f && directionY < 0.3f)
        {
            if (tag == "Skeleton")
            {
                Hero.Instance.GetDamage(skeletonAttackDamage);//тут мы получаем доступ к скрипту игрока и активируем оттуда функцию GetDamage
                float heal = skeletonAttackDamage * 0.5f; //Скелет ворует половину урона который наносит скелет игроку к себе в хп
                currentHP += heal;
                float healBar = heal / (float)skeletonMaxHP; //на сколько надо увеличить прогресс бар
                this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBarPlusHP(healBar);//обновление прогресс бара
            }
            if (tag == "Mushroom")
            {
                Hero.Instance.GetDamage(moushroomAttackDamage);//тут мы получаем доступ к скрипту игрока и активируем оттуда функцию GetDamage
            }
            if (tag == "Goblin")
            {
                Hero.Instance.GetDamage(goblinAttackDamage);//тут мы получаем доступ к скрипту игрока и активируем оттуда функцию GetDamage
            }
            if (tag == "Slime")
            {
                Hero.Instance.GetDamage(slimeAttackDamage);//тут мы получаем доступ к скрипту игрока и активируем оттуда функцию GetDamage
            }
            if (tag == "Death")
            {
                Hero.Instance.GetDamage(deathAttackDamage);//тут мы получаем доступ к скрипту игрока и активируем оттуда функцию GetDamage
            }
            
        }                                                       
    }
    //Секция где идет уселение характеристик врагов, если добавляется новый враг, тут нужно добавить его характеристики
    public void BoostEnemyHP() 
    {
        skeletonMaxHP += 10;
        moushroomMaxHP += 10;
        goblinMaxHP += 10;
    }
    public void BoostEnemyAttackDamage() //тут усиливыем урон
    {
        skeletonAttackDamage += 3;
        moushroomAttackDamage += 3;
        goblinAttackDamage += 3;
    }
    public void BoostEnemyReward() //тут увеличиваем награду за убийство
    {
        skeletonReward += 2;
        moushroomReward += 2;
        goblinReward += 2;
    }
    public void Push() //Метод для отталкивания тела во время получения урона
    {
        if (transform.lossyScale.x < 0) //смотрим в трансформе в какую сторону повернут по х обьект
        {
            this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(-0.5f, e_rb.velocity.y ), ForceMode2D.Impulse);//Импульс это значит что сила приложиться всего 1 раз
        }
        else
        {
            this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0.5f, e_rb.velocity.y), ForceMode2D.Impulse);//Импульс это значит что сила приложиться всего 1 раз
        }
    }
    public void TakeDamage(float dmg) //Метод для получения дамага где (int dmg) это значение можно будет вводить при вызове метода (то есть туда можно будет вписать урон)
    {
        isBlock = Enemy_Behavior.Instance.skeleton_block;
        if (currentHP > 0 && !isBlock)
        {
            anim.SetTrigger("damage");//анимация получения демейджа
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / (float)skeletonMaxHP; //на сколько надо уменьшаить прогресс бар
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//обновление прогресс бара
        }
        else if(currentHP > 0 && isBlock)
        {
            int level = LvLGeneration.Instance.Level;
            if (level < 5) //если меньше 5 уровня то 50% блокирования урона
            {
                blockDMG = dmg * 0.5f;
            }
            if (level >= 5) //если больше 5 уровня то 90% блокирования урона
            {
                blockDMG = dmg * 0.1f;
            }
            currentHP -= blockDMG;
            enemyTakeDamage = true;
            float maxHP = 50;
            if (tag == "Skeleton")
            {
                maxHP = skeletonMaxHP;
            }
            if (tag == "Mushroom")
            {
                maxHP = moushroomMaxHP;
            }
            if (tag == "Goblin")
            {
                maxHP = goblinMaxHP;
            }
            if (tag == "Slime")
            {

            }
            if (tag == "Death")
            {

            }
            takedDamage = blockDMG / maxHP; //на сколько надо уменьшаить прогресс бар
            if (this.gameObject != null) this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage);//обновление прогресс бара
        }
        if (currentHP <= 0)
        {
            int reward = 2;
            if (tag == "Skeleton")
            {
                reward = skeletonReward;
            }
            if (tag == "Mushroom")
            {
                reward = moushroomReward;
            }
            if (tag == "Goblin")
            {
                reward = goblinReward;
            }
            if (tag == "Slime")
            {
                reward = 1;
            }
            if (tag == "Death")
            {
                reward = 40;
            }
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
        Destroy(this.gameObject);//уничтожить этот игровой обьект
        if (tag == "Skeleton")
        {
            LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        }
        if (tag == "Mushroom")
        {
            LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        }
        if (tag == "Goblin")
        {
            LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        }
        if (tag == "Slime")
        {
            
        }
        if (tag == "Death")
        {
            LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
        }
        
    }

}
