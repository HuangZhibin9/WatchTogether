using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginInUI : MonoBehaviour
{
    [SerializeField] private Button enterButton;
    [SerializeField] private string playerName;

    private void Awake()
    {
        enterButton.onClick.AddListener(() =>
        {
            Enter();
        });
    }

    private void Update()
    {
    }

    private void Enter()
    {
        Debug.Log(playerName);
        MyLobbyManager.Instance.Authenticate(playerName);
        LobbyPanelPlayName.Instance.UpdateLobbyPanelPlayerName(playerName);
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetName(string str)
    {
        playerName = str;
    }

    public void EndInput(string str)
    {
        playerName = str;
        LobbyPanelPlayName.Instance.UpdateLobbyPanelPlayerName(playerName);
        Enter();
    }
}