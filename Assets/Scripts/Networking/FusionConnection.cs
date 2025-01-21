using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using Fusion.Sockets;
using System;


public class FusionConnection : SingletonPersistent<FusionConnection>, INetworkRunnerCallbacks
{

    private static string _playerName = null;
    [SerializeField] private PlayerCharacterController _playerPrefab = null;
    [SerializeField] private GameManager _gameManagerPrefab = null;
    [SerializeField] private NetworkRunner _networkRunnerPrefab = null;
    [SerializeField] private int _playerCount = 10;
    [SerializeField] private List<GameObject> _spawnPlatforms = null;


    private NetworkRunner _runner = null;
    private NetworkSceneManagerDefault _networkSceneManager = null;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private List<SessionInfo> _sessions = new List<SessionInfo>();

    public PlayerCharacterController LocalCharacterController { get; set; }
    public List<SessionInfo> Sessions => _sessions;

    public string PlayerName => _playerName;



    private void Awake()
    {
        base.Awake();

        if (Instance == null) Debug.LogError("No instance");

        _networkSceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        _runner = gameObject.AddComponent<NetworkRunner>();
    }

    public void ConnectToLobby(String playerName = null)
    {
        if (!playerName.IsNullOrEmpty()) _playerName = playerName;
        _runner.JoinSessionLobby(SessionLobby.ClientServer);
    }

    //Ovdew ide spajanje na session i kreiranje sessiona

    public async void JoinSession(string sessionName)
    {
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs { SessionName = sessionName, GameMode = GameMode.Client });
    }

    public async void CreateSession(string sessionName)
    {
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = sessionName,
            Scene = SceneRef.FromIndex(1),
            PlayerCount = _playerCount,
            SceneManager = _networkSceneManager,

        });
    }

    public void LeaveSession()
    {
        _runner.Shutdown();
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        _sessions = sessionList;
        SessionView.Instance.UpdateSessionList();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("On Player Joined");
        if (runner.IsServer)
        {
            if (player == runner.LocalPlayer)
            {
                runner.Spawn(_gameManagerPrefab);
            }

            Vector3 spawnPosition = GameManager.Instance.GetRandomSpawnPosition();
            Debug.Log("Spawn position: " + spawnPosition);
            NetworkObject playerObject = runner.Spawn(_playerPrefab.gameObject, position: spawnPosition, inputAuthority: player);
            _spawnedCharacters.Add(player, playerObject);
        }
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("On Player Left");
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    private void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        if (_runner != null && !_runner.IsDestroyed()) _runner.Shutdown();
    }

    #region UnusedCallbacks
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("On Input");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("On Connected to server");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("On Connect Failed");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("On Connect Request");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("On Custom Authentication Response");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("On Disconnected From Server");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("On Host Migration");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("On Input Missing");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("On Object Enter AOI");
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("OnO bject Exit AOI");
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log("On Reliable Data Progress");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        Debug.Log("On Reliable Data Received");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("On Scene Load Done");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("On Scene Load Start");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("On Shut down, reason: " + shutdownReason.ToString());
        LeaveSession();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("On User Simulation Message");
    }
    #endregion
}


