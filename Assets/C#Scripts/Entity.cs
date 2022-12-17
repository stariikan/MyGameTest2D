using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector3 dir;
    private bool flipRight = true; //Поворот спрайта на право, состояние = правда, нужно для поворота спрайта во время смены движения
    public virtual void GetDamage()
    {
        
    }
    public void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
    {
        flipRight = !flipRight; //Когда запускается метод Flip переменная flipRight меняется на false
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
    }
    public void Move() //создаем публичный метом Move
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f); //проверка доступности направления
                                                                                                                                              //смена этого направления

        if (colliders.Length > -3) dir *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);//и двигать самого монстра
        if (dir.x > 0 && !flipRight) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }
        else if (dir.x < 0 && flipRight) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }
    }
    public virtual void Die() //Обьявляем публичный метод Die
    {
        Destroy(this.gameObject);//уничтожить этот игровой обьект
    }
}
