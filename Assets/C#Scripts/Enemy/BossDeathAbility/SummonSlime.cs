using System.Collections;
using UnityEngine;

public class SummonSlime : MonoBehaviour
{
    public GameObject[] guards;
    public float direction;//переменная направления
    public Rigidbody2D rb; //Физическое тело
    private Animator anim; //Переменная благодаря которой анимирован обьект
    public static SummonSlime Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    private void Start()
    {
        Instance = this;
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object) к которому привязан скрипт
        anim = this.gameObject.GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object) к которому привязан скрипт
    }
    public void SummonGuards()
    {
        Vector3 pos = transform.position;
        GameObject guard1 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 1.5f, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
        guard1.name = "Enemy" + Random.Range(1, 999);
        GameObject guard2 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 1f, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
        guard2.name = "Enemy" + Random.Range(1, 999);
        GameObject guard3 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 2f, pos.y, pos.z), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
        guard3.name = "Enemy" + Random.Range(1, 999);

    }
    public void SummonDirection(Vector3 _direction)// выбор направления полета 
    {
        this.gameObject.SetActive(true); //активация игрового обьекта
        this.gameObject.transform.position = _direction;
        anim.SetTrigger("summon");
    }
    public void SummonOff()
    {
        this.gameObject.SetActive(false);
    }
}
