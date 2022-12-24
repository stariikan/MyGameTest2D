using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHP = 100; //Максимальные жизни скелета
    int currentHP;
    [SerializeField] private float AttackCooldown;//кулдаун Атаки (физ)
    public float enemyAttackRange = 0.4f; //Дальность физ атаки
    public int enemyAttackDamage = 7; // Урон от физ атаки
    private float cooldownTimer = Mathf.Infinity; //Если мы поставим тут 0, то игрок никогда не сможет аттаковать потому-что он будет меньше attackCooldown.
    public Transform enemyAttackPoint; //Тут мы ссылаемся на точку которая является дочерним обьектом игрока (нужна для реализации физ атаки)
    public LayerMask playerLayers;
    public static Entity Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    
    private Animator anim;
    private void Start()
    {
        currentHP = maxHP;
        Instance = this;
        anim = GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
                                         //к которому привязан скрипт
    }
    private void Attack()
    {
        //Anim.SetTrigger("Attack");//для воспроизведения анимации атаки при выполнения тригера Attack
        cooldownTimer = 0;
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(enemyAttackPoint.position, enemyAttackRange, playerLayers); //Создает круг из точки attackPoint c радиусом который мы указываем
        foreach (Collider2D enemy in hitEnemys)
        {
            enemy.GetComponent<Entity>().TakeDamage(enemyAttackDamage);//тут мы получаем доступ к скрипту врага Entity и активируем оттуда функцию TakeDamage и
                                                                  //урон прописан у нас в attackDamage
        }
    }
    public void Push() //Метод для отталкивания тела во время получения урона
    {
        if (transform.localScale.x < 0) //Условия чтобы определить в куда оттолкнется враг
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(-50, 0, 0));
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(50, 0, 0));
        }
    }
    public void TakeDamage(int dmg) //Метод для получения дамага где (int dmg) это значение можно будет вводить при вызове метода (то есть туда можно будет вписать урон)
    {
        currentHP -= dmg;
        anim.SetTrigger("damage");//анимация получения демейджа
        Push();
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public virtual void Die() //Обьявляем публичный метод Die
    {
        //anim.SetTrigger("death");//анимация смерти
        Debug.Log("Enemy Defeat");
        Destroy(this.gameObject);//уничтожить этот игровой обьект
    }
}
