using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainHP : MonoBehaviour
{
    
    public float direction;//переменная направления
    private float playerHP; //переменная метки попал ли во что-то снаряд

    private float drainHPDamage = 15f;

    GameObject player; //геймобьект игрок и ниже будет метод как он определяется и присваивается этой переменной
    public Rigidbody2D rb; //Физическое тело
    private Animator anim; //Переменная благодаря которой анимирован обьект
    private BoxCollider2D boxCollider; //Коллайдер магии
    public static DrainHP Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    private void Start()
    {
        Instance = this;
        player = GameObject.FindWithTag("PlayerCharacter");
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object) к которому привязан скрипт
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object) к которому привязан скрипт
        boxCollider = GetComponent<BoxCollider2D>();
        playerHP = Hero.Instance.curentHP;
    }
    private void Update()
    {
        playerHP = Hero.Instance.curentHP;
    }
    public void DrainHPDmg()
    {
        float directionX = player.transform.position.x - this.gameObject.transform.localPosition.x; //вычисление направление движения это Позиция игрока по оси х - позиция скелета по оси х
        float directionY = player.transform.position.y - this.gameObject.transform.localPosition.y; //вычисление направление движения это Позиция игрока по оси y - позиция скелета по оси y
        if (Mathf.Abs(directionX) < 1f && Mathf.Abs(directionY) < 2f && playerHP > 0) 
        {
            Hero.Instance.GetDamage(drainHPDamage);
            GameObject[] deathObjects = GameObject.FindGameObjectsWithTag("Death");
            foreach (GameObject obj in deathObjects)
            {
                if (obj.name != "BossDeath")
                {
                    obj.GetComponent<Entity_Enemy>().BossDeathHeal(50);
                }
            }
        }
    }
    public void DrainHPDirection(Vector3 _direction)// выбор направления полета 
    {
        this.gameObject.SetActive(true); //активация игрового обьекта
        this.gameObject.transform.position = _direction;
        anim.SetTrigger("drain_hp");
    }
    public void DrainHPOff()
    {
        this.gameObject.SetActive(false);
    }
}
