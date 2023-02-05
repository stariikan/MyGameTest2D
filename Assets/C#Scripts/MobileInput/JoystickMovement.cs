using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class JoystickMovement : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    private RectTransform joystickTransform;

    [SerializeField] private float dragTreshold = 0.6f; /// Если переместиться джойстик больше чем на 0,6 в любом из направлений, то скрипт поймет что игрок хочет пойти в этом направлении
    [SerializeField] private int dragMovementDistance = 5;
    [SerializeField] private int dragOffsetDistance = 30;

    public event Action<Vector2> onMove;
    public Vector2 offset;
    public float moveX;
    public float moveY;
    public Camera MainCamera;

    public static JoystickMovement Instance { get; set; }

    private void Start()
    {
        Instance = this;
    }
    public void OnDrag(PointerEventData eventData)
    {

        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTransform, eventData.position, MainCamera, out offset);
        offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance; // (-1) - 1
        joystickTransform.anchoredPosition = offset * dragMovementDistance;
        Vector2 inputVector = ColculateMovementInput(offset);
        onMove?.Invoke(inputVector);
    }

    private Vector2 ColculateMovementInput(Vector2 offset)
    {
        moveX = Mathf.Abs(offset.x) > dragTreshold ? offset.x : 0;
        moveY = Mathf.Abs(offset.y) > dragTreshold ? offset.y : 0;
        return new Vector2(moveX, moveY);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData) //возврат джойстика
    {
        joystickTransform.anchoredPosition = Vector2.zero;
        moveX = 0;
        moveY = 0;
        onMove?.Invoke(Vector2.zero);
    }

    private void Awake()
    {
        joystickTransform = (RectTransform)transform;
    }
} 