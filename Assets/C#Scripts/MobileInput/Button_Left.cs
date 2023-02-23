using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Button_Left : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) //метод при нажатии на экран
    {
        Hero.Instance.Left();
    }

    public void OnPointerUp(PointerEventData eventData) //возврат джойстика
    {

    }
}