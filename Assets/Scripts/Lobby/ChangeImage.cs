using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public static ChangeImage Instance { get; private set; }
    [SerializeField] private Button mainButton;
    [SerializeField] private SpriteListSO sprites;

    public event Action<MyLobbyManager.PlayerCharacter> mainButtonClick;

    [HideInInspector] public MyLobbyManager.PlayerCharacter playerCharacter = MyLobbyManager.PlayerCharacter.Anon;

    private void Awake()
    {
        Instance = this;
        mainButton.onClick.AddListener(() =>
        {
            mainButtonClick(playerCharacter);
        });
    }

    public void SetMainButtonCharacter(MyLobbyManager.PlayerCharacter _playerCharacter)
    {
        playerCharacter = _playerCharacter;
        mainButton.GetComponent<Image>().sprite = sprites.GetSprite(playerCharacter);
    }
}