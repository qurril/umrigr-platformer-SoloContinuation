using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{

    private List<PlayerStats> _players = new();
    public void RegisterPlayer(PlayerStats player) => _players.Add(player);

    public void UnregisterPlayer(PlayerStats player) => _players.Remove(player);

    
}

