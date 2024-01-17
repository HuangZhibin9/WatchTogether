using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HideImageButton : MonoBehaviour, IPointerClickHandler
{
    public static event Action HideImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        HideImage?.Invoke();
    }
}