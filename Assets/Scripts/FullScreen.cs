using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    [SerializeField] private Button fullScreenButton;
    [SerializeField] private Button fullScreentExitButton;

    private void Awake()
    {
        fullScreenButton.onClick.AddListener(OnFullScreenButtonClick);
        fullScreentExitButton.onClick.AddListener(OnFullScreenExitButtonClick);

        fullScreenButton.gameObject.SetActive(true);
        fullScreentExitButton.gameObject.SetActive(false);
    }

    private void OnFullScreenExitButtonClick()
    {
        Screen.SetResolution(1280, 720, false);
        fullScreenButton.gameObject.SetActive(true);
        fullScreentExitButton.gameObject.SetActive(false);
    }

    private void OnFullScreenButtonClick()
    {
        Screen.SetResolution(1920, 1080, true);
        fullScreenButton.gameObject.SetActive(false);
        fullScreentExitButton.gameObject.SetActive(true);
    }
}