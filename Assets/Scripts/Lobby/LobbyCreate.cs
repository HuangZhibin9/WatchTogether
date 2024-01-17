using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreate : MonoBehaviour
{
    public static LobbyCreate Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI inputFieldText;
    [SerializeField] private Button createButton;
    private string lobbyName;

    private void Awake()
    {
        Instance = this;

        Hide();
    }

    private void Start()
    {
        createButton.onClick.AddListener(() =>
        {
            MyLobbyManager.Instance.CreateLobby(lobbyName);
            Hide();
        });
    }

    public void EndInput(string name)
    {
        lobbyName = name;
        Debug.Log("Current lobbyName: " + lobbyName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        inputFieldText.text = "输入房间名字";
    }
}