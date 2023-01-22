using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHP = 50; //Максимальные жизни скелета
    public int currentHP;
    public float takedDamage; //разница между макс хп и полученным уроном
    [SerializeField] private float AttackCooldown;//кулдаун Атаки (физ)
    public float enemyAttackRange = 0.3f; //Дальность физ атаки
    public int enemyAttackDamage = 7; // Урон от физ атаки
    private float cooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown.
    public Transform enemyAttackPoint; //Тут мы ссылаемся на точку которая является дочерним обьектом игрока (нужна для реализации физ атаки)
    public LayerMask playerLayers;
    public Vector3 lossyScale;
    public static Entity Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    private Animator anim;
    public bool enemyDead = false;
    public int rewardForKillEnemy = 2;//награда за победу над врагом
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
            enemyAttackDamage = 7;
        }
        Instance = this;
        anim = GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
                                         //к которому привязан скрипт
    }
    private void Attack()
    {
        if (cooldownTimer > AttackCooldown)
        {
            //Anim.SetTrigger("Attack");//для воспроизведения анимации атаки при выполнения тригера Attack
            cooldownTimer = 0;
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(enemyAttackPoint.position, enemyAttackRange, playerLayers); //Создает круг из точки attackPoint c радиусом который мы указываем
            foreach (Collider2D enemy in hitEnemys)
            {
                enemy.GetComponent<Hero>().GetDamage(enemyAttackDamage);//тут мы получаем доступ к скрипту врага Entity и активируем оттуда функцию TakeDamage и
                                                                        //урон прописан у нас в attackDamage
            }
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
            this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(-7f, 5f), ForceMode2D.Impulse);//Импульс это значит что сила приложиться всего 1 раз
        }
        else
        {
            this.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(7f, 5f), ForceMode2D.Impulse);//Импульс это значит что сила приложиться всего 1 раз
        }
    }
    public void TakeDamage(int dmg) //Метод для получения дамага где (int dmg) это значение можно будет вводить при вызове метода (то есть туда можно будет вписать урон)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("damage");//анимация получения демейджа
            currentHP -= dmg;
            takedDamage = (float)dmg / (float)maxHP; //на сколько надо уменьшаить прогресс бар
            Debug.Log(takedDamage);
            Push();
            this.gameObject.GetComponentInChildren<enemyProgressBar>().UpdateEnemyProgressBar(takedDamage) ;//обновление прогресс бара
            Debug.Log(currentHP + " " + gameObject.name);
        }
        else
        {
            return;
        }

        if (currentHP <= 0)
        {
            LvLGeneration.Instance.PlusCoin(rewardForKillEnemy); //вызов метода для увелечения очков
            anim.SetTrigger("death");//анимация смерти
            enemyDead = true;
            Debug.Log("Enemy Defeat -> " + gameObject.name);
        }
    }
    public virtual void Die() //Метод удаляет этот игровой обьект, вызывается через аниматор сразу после завершения анимации смерти
    {
        Destroy(this.gameObject); ;//уничтожить этот игровой обьект
    }
    private void Update()
    {
        if (currentHP > 0)
        {
            cooldownTimer += Time.deltaTime; // прибавление времени к кулдаун таймеру
            Attack();
        }
        else
        {
            return;
        }
        
            
    }
}
