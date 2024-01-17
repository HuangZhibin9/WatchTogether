using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterStart : MonoBehaviour
{
    private void Start()
    {
        MyLobbyManager.Instance.OnGameStarted += OnGameStarted;

        //Hide();
    }

    private void OnGameStarted()
    {
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}