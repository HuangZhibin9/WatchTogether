using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerMove : MonoBehaviour, IPointerMoveHandler
{
    public static event Action PointerMoveEvent;

    public void OnPointerMove(PointerEventData eventData)
    {
        PointerMoveEvent?.Invoke();
    }
}