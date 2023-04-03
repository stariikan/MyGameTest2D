using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static Shield Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float direction;//переменная направления
    [SerializeField] private float lifetime; //длительность жизни снаряда

    private BoxCollider2D boxCollider; //Коллайдер удара

    public string TargetName;
    public GameObject target;


    private void Awake() //Действие выполняется до старта игры и 1 раз
    {
        //anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // вытаскиваем информацию из компанента бокс колайдер
        Instance = this;
    }

    private void Update()
    {
        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        if (lifetime > 1.5f)
        {
            this.gameObject.SetActive(false);//когда переменная достигает 1.5, коллайдер атаки исчезает
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        boxCollider.enabled = false; //отключаем коллайдер
        this.gameObject.SetActive(false);//когда переменная достигает 1.5, коллайдер атаки исчезает
        target = GameObject.Find(TargetName);
        Debug.Log(target);
        if (target.CompareTag("Bomb")) target.GetComponent<Bomb>().PushFromPlayer();
        if (target.CompareTag("EvilWizard")) target.GetComponent<Enemy_Behavior>().Stun();
        if (target.CompareTag("Mushroom")) target.GetComponent<Enemy_Behavior>().Stun();
        if (target.CompareTag("Martial")) target.GetComponent<Enemy_Behavior>().Stun();
        if (target != null && target.layer == 7) target.GetComponent<Enemy_Behavior>().PushFromPlayer();
    }
    public void MeleeDirection(Vector3 _direction)// выбор направления полета 
    {
        lifetime = 0;
        gameObject.SetActive(true); //активация игрового обьекта
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //активация коллайдера
    }
}
