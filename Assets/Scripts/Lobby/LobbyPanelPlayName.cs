using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyPanelPlayName : MonoBehaviour
{
    public static LobbyPanelPlayName Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI nameText;
    private string currentName;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateLobbyPanelPlayerName(string name)
    {
        currentName = name;
        nameText.text = name;
    }

    public void EndInput(string name)
    {
        currentName = name;
        MyLobbyManager.Instance.UpdatePlayerName(name);
    }

    public void OnDiselected(string name)
    {
        nameText.text = currentName;
    }
}