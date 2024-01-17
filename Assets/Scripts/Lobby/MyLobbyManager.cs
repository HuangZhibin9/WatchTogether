using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class MyLobbyManager : MonoBehaviour
{
    public static MyLobbyManager Instance { get; private set; }

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_CHARACTER = "PlayerCharacter";
    public const string KEY_START_GAME = "StartGame";

    public event Action OnLeftLobby;

    public event Action<Lobby> OnJoinedLobby;

    public event Action<Lobby> OnJoinedLobbyUpdate;

    public event Action<Lobby> OnKickedFromLobby;

    public event Action<List<Lobby>> OnLobbyListChanged;

    public event Action OnGameStarted;

    public enum PlayerCharacter
    {
        Anon,
        Soyo,
        Tomori,
        Taki,
        Rana,
    }

    private float heartbeatTimer;
    private float lobbyPollTimer;
    private float refreshLobbyListTimer = 5f;

    private Lobby joinedLobby;

    [SerializeField, ReadOnly] private string playerName;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Button]
    public async void Authenticate(string _playerName)
    {
        Debug.Log("Authenticate Function");
        playerName = _playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);

            RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPolling();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                Debug.Log("Heartbeat");
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private async void HandleLobbyPolling()
    {
        if (joinedLobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer <= 0f)
            {
                Debug.Log("joinedLobby.Players.Count: " + joinedLobby.Players.Count);

                float lobbyPollTimerMax = 1.1f;
                lobbyPollTimer = lobbyPollTimerMax;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                OnJoinedLobbyUpdate?.Invoke(joinedLobby);

                if (!IsPlayerInLobby())
                {
                    Debug.Log("Kicked from lobby");
                    OnKickedFromLobby?.Invoke(joinedLobby);
                    joinedLobby = null;
                }

                if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                {
                    Debug.Log("Start Game");
                    if (!IsLobbyHost())
                    {
                        RelayManager.Instance.JoinRelayAsync(joinedLobby.Data[KEY_START_GAME].Value);
                        if (joinedLobby != null)
                        {
                            try
                            {
                                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                                Debug.Log("Leave the lobby : " + joinedLobby.Name);
                                joinedLobby = null;
                            }
                            catch (LobbyServiceException e)
                            {
                                Debug.Log(e);
                            }
                        }
                    }

                    OnGameStarted?.Invoke();
                }
            }
        }
    }

    public Lobby GetJoinedLobby()
    {
        Debug.Log("GetJoinedLobby Function");
        return joinedLobby;
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject>()
        {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
            { KEY_PLAYER_CHARACTER, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,PlayerCharacter.Anon.ToString())},
        });
    }

    public async void CreateLobby(string lobbyName, int maxPlayers = 5, bool isPrivate = false)
    {
        Player player = GetPlayer();

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = player,
            IsPrivate = isPrivate,
            Data = new Dictionary<string, DataObject>()
            {
                { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(joinedLobby);
        Debug.Log("Created lobby " + lobby.Name);
    }

    public async void RefreshLobbyList()
    {
        Debug.Log("RefreshLobbyList Function");
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 5;

            options.Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0" ,QueryFilter.OpOptions.GT)
            };

            options.Order = new List<QueryOrder>
            {
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbyListQueryResponse = await LobbyService.Instance.QueryLobbiesAsync(options);
            Debug.Log("Find :" + lobbyListQueryResponse.Results.Count + " lobbies");
            OnLobbyListChanged?.Invoke(lobbyListQueryResponse.Results);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public async void FindLobbyByName(string lobbyName)
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            options.Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.Name,lobbyName ,QueryFilter.OpOptions.CONTAINS)
            };

            options.Order = new List<QueryOrder>
            {
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbyListQueryResponse = await LobbyService.Instance.QueryLobbiesAsync(options);
            OnLobbyListChanged?.Invoke(lobbyListQueryResponse.Results);

            foreach (Lobby lobby in lobbyListQueryResponse.Results)
            {
                Debug.Log(lobby.Name);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        Player player = GetPlayer();
        Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions { Player = player });
        if (lobby == null)
        {
            Debug.Log("Join lobby failed");
            return;
        }
        Debug.Log("Joined lobby " + lobby.Name);
        joinedLobby = lobby;
        OnJoinedLobby?.Invoke(joinedLobby);
    }

    public async void JoinLobby(Lobby lobby)
    {
        Player player = GetPlayer();
        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions { Player = player });
        OnJoinedLobby?.Invoke(joinedLobby);
    }

    public async void UpdatePlayerName(string _playerName)
    {
        this.playerName = _playerName;

        if (joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions();
                options.Data = new Dictionary<string, PlayerDataObject>()
            {
                {KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, _playerName) }
            };

                string playerId = AuthenticationService.Instance.PlayerId;
                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;
                OnJoinedLobbyUpdate?.Invoke(joinedLobby);

                foreach (Player player in joinedLobby.Players)
                {
                    if (player.Id == AuthenticationService.Instance.PlayerId)
                    {
                        Debug.Log("Player Name in lobby changed to : " + player.Data[KEY_PLAYER_NAME].Value);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        Debug.Log("Current Player Name : " + playerName);
    }

    public async void UpdatePlayerCharacter(PlayerCharacter playerCharacter)
    {
        if (joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_CHARACTER, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerCharacter.ToString())
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(joinedLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                Debug.Log("Leave the lobby : " + joinedLobby.Name);
                joinedLobby = null;
                OnLeftLobby?.Invoke();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void StartWatchAsync()
    {
        if (IsLobbyHost())
        {
            try
            {
                Debug.Log("Start Watch!");

                string relayCode = await RelayManager.Instance.CreateRelay(joinedLobby.Players.Count);
                Debug.Log("joinedLobby.Players.Count: " + joinedLobby.Players.Count);
                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id,
                    new UpdateLobbyOptions
                    {
                        Data = new Dictionary<string, DataObject>
                        {
                            {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode)}
                        }
                    });
                joinedLobby = lobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}