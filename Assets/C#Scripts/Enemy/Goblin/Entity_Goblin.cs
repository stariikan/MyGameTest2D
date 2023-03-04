using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Goblin : MonoBehaviour
{
    public float maxHP = 35; //Максимальные жизни скелета
    public float currentHP;
    public float takedDamage; //разница между макс хп и полученным уроном
    public float enemyAttackRange = 1.2f; //Дальность физ атаки
    public float enemyAttackDamage = 25; // Урон от физ атаки
    public Transform enemyAttackPoint; //Тут мы ссылаемся на точку которая является дочерним (нужна для реализации физ атаки)
    public LayerMask playerLayers;
    public Vector3 lossyScale;
    private Rigidbody2D e_rb;
    public static Entity_Goblin Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    private Animator anim;
    public bool enemyDead = false;
    public bool enemyTakeDamage = false;
    public int rewardForKillEnemy = 2;//награда за победу над врагом

    private float playerHP;

    private CapsuleCollider2D capsuleCollider;

    private float directionY;
    private float directionX;

    private void Start()
    {
        maxHP = SaveSerial.Instance.moushroomHP;
        if (maxHP == 0)
        {
            maxHP = 35;
        }
        currentHP = maxHP;
        enemyAttackDamage = SaveSerial.Instance.moushroomDamage;
        if (enemyAttackDamage == 0)
        {
            enemyAttackDamage = 25;
        }
        Instance = this;
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
        e_rb = this.gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        playerHP = Hero.Instance.hp;
    }
    public void DamageDeealToPlayer()
    {
        directionX = this.gameObject.GetComponent<Enemy_Goblin>().directionX;
        directionY = this.gameObject.GetComponent<Enemy_Goblin>().directionY;
        if (playerHP > 0 && directionX < 1.2f && directionY < 0.3f)
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
    public void TakeDamage(float dmg) //Метод для получения дамага где (int dmg) это значение можно будет вводить при вызове метода (то есть туда можно будет вписать урон)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("damage");//анимация получения демейджа
            currentHP -= dmg;
            enemyTakeDamage = true;
            takedDamage = (float)dmg / (float)maxHP; //на сколько надо уменьшаить прогресс бар
            this.gameObject.GetComponentInChildren<Goblin_progress_bar>().UpdateEnemyProgressBar(takedDamage);//обновление прогресс бара
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
        LvLGeneration.Instance.FindKey();//вызов метода для получения ключей
    }}
