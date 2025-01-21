using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class GameManager : NetworkBehaviour
{

    private static GameManager _instance;
    private PlayerCharacterController[] players;
    

    [Networked] public int RemainingGameStartTime { get; private set; }
   


    public static GameManager Instance;

    public List<Transform> spawnPoints = new List<Transform>();

    public override void Spawned()
    {
        _instance = this;
        
        Debug.Log("GameManager spawned");
        GameObject[] spawnPlatforms = GameObject.FindGameObjectsWithTag("SpawnPlatform");
        spawnPoints = spawnPlatforms.Select(spawnPlatform => spawnPlatform.transform).ToList();

        StartCoroutine(GameStartCountdown());
    }

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available!");
            return Vector3.zero;
        }

        int randomIndex = Random.Range(0, spawnPoints.Count);
        Vector3 spawnPosition = spawnPoints[randomIndex].position + new Vector3(0, 0.5f, 0);
        spawnPoints.RemoveAt(randomIndex);
        return spawnPosition;

    }



    private IEnumerator GameStartCountdown() {

        UnityEngine.Debug.Log($"GameStartCountingDown value of time {RemainingGameStartTime}");

        while (RemainingGameStartTime > 0)
        {
            yield return new WaitForSeconds(1);
            RemainingGameStartTime--;
        }

        UnityEngine.Debug.Log("GameStart");
        GameManager.Instance.RpcStartGame();
    }


    public void DisableAllPlayers()
    {
        players = FindObjectsOfType<PlayerCharacterController>();

        foreach (var player in players)
        {
            player.DisablePlayerControls();
        }
    }

    public void EnableAllPlayers()
    {
        players = FindObjectsOfType<PlayerCharacterController>();

        foreach (var player in players)
        {
            player.EnablePlayerControls();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcStartGame()
    {
        EnableAllPlayers();
    }


}
