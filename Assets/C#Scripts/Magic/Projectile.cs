using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float direction;//переменная направления
    [SerializeField] private float Speed; //Скорость снаряда
    [SerializeField] private float lifetime; //длительность жизни снаряда
    private bool hit; //переменная метки попал ли во что-то снаряд
    
    private BoxCollider2D boxCollider; //Коллайдер магии
    private Animator anim; //переменная для аниматора
   
    private void Awake() //Действие выполняется до старта игры и 1 раз
    {
        anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // вытаскиваем информацию из компанента бокс колайдер
    }

    private void Update()
    {
        if (hit) return; //проверка попадания огненого шара во что-нибудь
        float movementSpeed = Speed * Time.deltaTime * direction; // вычисление скорости перемещения в секунду и в каком направлении полетит снаряд
        transform.Translate(movementSpeed, 0, 0);//ось х = movementspeed, y = 0, z=0 - все это перемещение по оси x

        lifetime += Time.deltaTime; //увелечение переменной lifetime каждую сек +1
        if (lifetime > 5) gameObject.SetActive(false);//когда переменная достигает 5, снаряд исчезает
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true; //тут указываем что произошло столкновение
        boxCollider.enabled = false; //отключаем коллайдер
        anim.SetTrigger("explode");//для воспроизведения анимации атаки снарядом при выполнения тригера magicAttack
        //Deactivate();
    }

    public void SetDirection(float _direction)// выбор направления полета 
    {

        lifetime = 0;
        gameObject.SetActive(true); //активация игрового обьекта
        direction = _direction;
        boxCollider.enabled = true; //активация коллайдера 
        hit = false; //обьект коснулся другого обьекта = false
       
        float localScaleX = transform.localScale.x; //этот весь код про то чтобы менялся x на -x в зависимости в какую сторону мы стреляем, тоесть был переворот спрайта 
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);//смена направления снаряда
    }
    private void Deactivate() //деактивация снаряда после завершения анимации взрывал
    {
        gameObject.SetActive(false);
    }
}
