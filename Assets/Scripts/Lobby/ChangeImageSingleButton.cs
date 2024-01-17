using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageSingleButton : MonoBehaviour
{
    public static event Action ChangeImageFinished;

    private Image image;
    private Button button;
    [SerializeField] private MyLobbyManager.PlayerCharacter myCharacter;
    private MyLobbyManager.PlayerCharacter mainButtonCharacter = MyLobbyManager.PlayerCharacter.Anon;

    [SerializeField] private SpriteListSO sprites;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            MyLobbyManager.Instance.UpdatePlayerCharacter(myCharacter);
            ChangeImage.Instance.SetMainButtonCharacter(myCharacter);
            myCharacter = mainButtonCharacter;
            ChangeImageFinished?.Invoke();
        });
    }

    private void Start()
    {
        ChangeImage.Instance.mainButtonClick += ((_playerCharacter) =>
        {
            mainButtonCharacter = _playerCharacter;
            image.sprite = sprites.GetSprite(myCharacter);
            if (gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        });

        HideImageButton.HideImage += (() =>
        {
            Hide();
        });

        ChangeImageFinished += (() =>
        {
            Hide();
        });

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}