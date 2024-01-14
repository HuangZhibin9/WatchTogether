using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InBottomUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [ReadOnly]
    public static bool isInBottomUIZone = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInBottomUIZone = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInBottomUIZone = false;
    }
}