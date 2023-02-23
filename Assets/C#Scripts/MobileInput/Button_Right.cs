using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Button_Right : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) //метод при нажатии на экран
    {
        Hero.Instance.Right();
    }

    public void OnPointerUp(PointerEventData eventData) //возврат джойстика
    {

    }
}