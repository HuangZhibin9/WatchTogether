using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyList : MonoBehaviour
{
    public static LobbyList Instance { get; private set; }

    [SerializeField] private Transform lobbySingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button createLobbyButton;

    private void Awake()
    {
        Instance = this;

        lobbySingleTemplate.gameObject.SetActive(false);

        refreshButton.onClick.AddListener(RefreshButtonClick);
        createLobbyButton.onClick.AddListener(CreateLobbyButtonClick);
    }

    private void Start()
    {
        MyLobbyManager.Instance.OnLobbyListChanged += OnLobbyListChanged;
        MyLobbyManager.Instance.OnJoinedLobby += OnJoinedLobby;
        MyLobbyManager.Instance.OnLeftLobby += OnLeftLobby;
        MyLobbyManager.Instance.OnKickedFromLobby += OnKickedFromLobby;
    }

    private void OnKickedFromLobby(Lobby lobby)
    {
        Show();
    }

    private void OnLeftLobby()
    {
        Show();
    }

    private void OnJoinedLobby(Lobby lobby)
    {
        Hide();
    }

    private void OnLobbyListChanged(List<Lobby> lobbyList)
    {
        UpdataLobbyList(lobbyList);
    }

    private void UpdataLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in container)
        {
            if (child == lobbySingleTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbySingleTransform = Instantiate(lobbySingleTemplate, container);
            lobbySingleTransform.gameObject.SetActive(true);

            LobbySingle lobbySingle = lobbySingleTransform.GetComponent<LobbySingle>();
            lobbySingle.UpdateLobby(lobby);
        }
    }

    [Button]
    private void CreateLobbyButtonClick()
    {
        LobbyCreate.Instance.Show();
    }

    private void RefreshButtonClick()
    {
        MyLobbyManager.Instance.RefreshLobbyList();
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