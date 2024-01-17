using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeStart : MonoBehaviour
{
    private void Start()
    {
        MyLobbyManager.Instance.OnGameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        Hide();
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