using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : NetworkBehaviour
{
    private static GameManager _instance;
    private PlayerCharacterController[] players;
    public static GameManager Instance => _instance;

    [Networked] public int RemainingGameStartTime { get; private set; }
    public override void Spawned()
    {
        _instance = this;
        StartCoroutine(GameStartCountdown());
    }



    private IEnumerator GameStartCountdown() {

      

        while (RemainingGameStartTime > 0)
        {
            yield return new WaitForSeconds(1);
            RemainingGameStartTime--;
        }


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
