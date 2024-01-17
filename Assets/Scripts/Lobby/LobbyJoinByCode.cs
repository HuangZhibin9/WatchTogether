using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyJoinByCode : MonoBehaviour
{
    public static LobbyJoinByCode Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI inputCode;
    [SerializeField] private Button joinButton;
    private string code;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        joinButton.onClick.AddListener(() =>
        {
            MyLobbyManager.Instance.JoinLobbyByCode(code);
        });
    }

    public void EndInput(string _code)
    {
        code = _code;
        Debug.Log("Input lobby code: " + code);
    }
}