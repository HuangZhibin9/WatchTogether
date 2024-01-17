using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerSingle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Button kickPlayerButton;
    [SerializeField] private SpriteListSO sprites;

    private Player player;

    private void Awake()
    {
        kickPlayerButton.onClick.AddListener(KickPlayer);
    }

    public void SetKickPlayerButtonVisible(bool visible)
    {
        kickPlayerButton.gameObject.SetActive(visible);
    }

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.Data[MyLobbyManager.KEY_PLAYER_NAME].Value;
        MyLobbyManager.PlayerCharacter playerCharacter =
            System.Enum.Parse<MyLobbyManager.PlayerCharacter>(player.Data[MyLobbyManager.KEY_PLAYER_CHARACTER].Value);
        characterImage.sprite = sprites.GetSprite(playerCharacter);
    }

    private void KickPlayer()
    {
        if (player != null)
        {
            MyLobbyManager.Instance.KickPlayer(player.Id);
        }
    }
}