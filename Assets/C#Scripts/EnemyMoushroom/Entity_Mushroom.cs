using UnityEngine;

public class Entity_Mushroom : MonoBehaviour
{
    public int maxHP = 50; //Максимальные жизни скелета
    public int currentHP;
    public float takedDamage; //разница между макс хп и полученным уроном
    public float enemyAttackRange = 1.2f; //Дальность физ атаки
    public int enemyAttackDamage = 15; // Урон от физ атаки
    public Transform enemyAttackPoint; //Тут мы ссылаемся на точку которая является дочерним (нужна для реализации физ атаки)
    public LayerMask playerLayers;
    public Vector3 lossyScale;
    private Rigidbody2D e_rb;
    public static Entity_Mushroom Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    private Animator anim;
    public bool enemyDead = false;
    public bool enemyTakeDamage = false;
    public int rewardForKillEnemy = 2;//награда за победу над врагом

    private BoxCollider2D boxCollider;

    private float directionY;
    private float directionX;

    private void Start()
    {
        maxHP = SaveSerial.Instance.enemyHP;
        if (maxHP == 0)
        {
            maxHP = 50;
        }
        currentHP = maxHP;
        enemyAttackDamage = SaveSerial.Instance.enemyDamage;
        if (enemyAttackDamage == 0)
        {
            enemyAttackDamage = 15;
        }
        Instance = this;
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
        e_rb = this.gameObject.GetComponent<Rigidbody2D>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }
    public void DamageDeealToPlayer()
    {
        directionX = Enemy_Mushroom.Instance.directionX;
        directionY = Enemy_Mushroom.Instance.directionY;
        if(directionX < 0.8f && directionY < 0.3f)
        {
            Hero.Instance.GetDamage(enemyAttackDamage);//тут мы получаем доступ к скрипту игрока и активируем оттуда функцию GetDamage  
        }                                                       
    }
    public void BoostHP() //тут усиливыем хп
    {
        maxHP += 10;
    }
    public void BoostAttackDamage() //тут усиливыем урон
    {
        enemyAttackDamage += 3;
    }
    public void BoostReward() //тут увеличиваем награду за убийство
    {
        rewardForKillEnemy += 2;
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
    public void TakeDamage(int dmg) //Метод для получения дамага где (int dmg) это значение можно будет вводить при вызове метода (то есть туда можно будет вписать урон)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("damage");//анимация получения демейджа
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / (float)maxHP; //на сколько надо уменьшаить прогресс бар
            //Debug.Log(takedDamage);
            //Push();
            this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//обновление прогресс бара
            //Debug.Log(currentHP + " " + gameObject.name);
        }
        else
        {
            return;
        }

        if (currentHP <= 0)
        {
            LvLGeneration.Instance.PlusCoin(rewardForKillEnemy);//вызов метода для увелечения очков
            e_rb.gravityScale = 0;
            e_rb.velocity = Vector2.zero;
            boxCollider.enabled = false;
            anim.SetTrigger("m_death");//анимация смерти
            enemyDead = true;
            //Debug.Log("Enemy Defeat -> " + gameObject.name);
        }
    }
    public virtual void Die() //Метод удаляет этот игровой обьект, вызывается через аниматор сразу после завершения анимации смерти
    {
        Destroy(this.gameObject);//уничтожить этот игровой обьект
        LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
    }
}
