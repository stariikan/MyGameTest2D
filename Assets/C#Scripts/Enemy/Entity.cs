using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
      
    public virtual void Die() //Обьявляем публичный метод Die
    {
    Destroy(this.gameObject);//уничтожить этот игровой обьект
    }
}
