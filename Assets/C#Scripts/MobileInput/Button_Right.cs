using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Button_Right : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) //����� ��� ������� �� �����
    {
        Hero.Instance.Right();
    }

    public void OnPointerUp(PointerEventData eventData) //������� ���������
    {

    }
}