using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class InLobby : MonoBehaviour
{
    public static InLobby Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI lobbyNameText;

    [SerializeField] private TextMeshProUGUI lobbyCode;
    [SerializeField] private Button leaveLobbyButton;

    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject waitPanel;

    private void Awake()
    {
        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        leaveLobbyButton.onClick.AddListener(() =>
        {
            MyLobbyManager.Instance.LeaveLobby();
        });

        startButton.onClick.AddListener(() =>
        {
            waitPanel.SetActive(true);
            MyLobbyManager.Instance.StartWatchAsync();
        });
    }

    private void Start()
    {
        MyLobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        MyLobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        MyLobbyManager.Instance.OnLeftLobby += MyLobbyManager_OnLeftLobby;
        MyLobbyManager.Instance.OnKickedFromLobby += MyLobbyManager_OnLeftLobby;

        Hide();
    }

    private void MyLobbyManager_OnLeftLobby(Lobby lobby)
    {
        ClearLobby();
        Hide();
    }

    private void MyLobbyManager_OnLeftLobby()
    {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(Lobby lobby)
    {
        UpdateLobby(lobby);
    }

    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();

        foreach (Player player in lobby.Players)
        {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerSingle lobbyPlayerSingle = playerSingleTransform.GetComponent<LobbyPlayerSingle>();
            lobbyPlayerSingle.SetKickPlayerButtonVisible(
                MyLobbyManager.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId);
            lobbyPlayerSingle.UpdatePlayer(player);
        }
        lobbyNameText.text = lobby.Name;
        lobbyCode.text = lobby.LobbyCode;
        if (MyLobbyManager.Instance.IsLobbyHost())
        {
            startButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
        Show();
    }

    private void ClearLobby()
    {
        foreach (Transform child in container)
        {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}