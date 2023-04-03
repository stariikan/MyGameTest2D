using System.Collections;
using UnityEngine;

public class SummonSlime : MonoBehaviour
{
    public GameObject[] guards;
    public float direction;//переменна¤ направлени¤
    public Rigidbody2D rb; //‘изическое тело
    private Animator anim; //ѕеременна¤ благодар¤ которой анимирован обьект
    public static SummonSlime Instance { get; set; } //ƒл¤ сбора и отправки данных из этого скрипта

    private void Start()
    {
        Instance = this;
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //ѕеременна¤ rb получает компонент Rigidbody2D (‘изика game.Object) к которому прив¤зан скрипт
        anim = this.gameObject.GetComponent<Animator>(); //ѕеременна¤ anim получает информацию из компонента Animator (јнимаци¤ game.Object) к которому прив¤зан скрипт
    }
    public void SummonGuards()
    {
        Vector3 pos = transform.position;
        GameObject guard1 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 1.5f, pos.y, pos.z), Quaternion.identity); // лонировани¤ обьекта (враг) и его координаты)
        guard1.name = "Enemy" + Random.Range(1, 999);
        GameObject guard2 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 1f, pos.y, pos.z), Quaternion.identity); // лонировани¤ обьекта (враг) и его координаты)
        guard2.name = "Enemy" + Random.Range(1, 999);
        GameObject guard3 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 2f, pos.y, pos.z), Quaternion.identity); // лонировани¤ обьекта (враг) и его координаты)
        guard3.name = "Enemy" + Random.Range(1, 999);

    }
    public void SummonDirection(Vector3 _direction)// выбор направлени¤ полета 
    {
        this.gameObject.SetActive(true); //активаци¤ игрового обьекта
        this.gameObject.transform.position = _direction;
        anim.SetTrigger("summon");
    }
    public void SummonOff()
    {
        this.gameObject.SetActive(false);
    }
}
