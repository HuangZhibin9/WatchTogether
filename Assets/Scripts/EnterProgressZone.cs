using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnterProgressZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject sliderHandle;

    public void OnPointerEnter(PointerEventData eventData)
    {
        sliderHandle.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sliderHandle.SetActive(false);
    }
}