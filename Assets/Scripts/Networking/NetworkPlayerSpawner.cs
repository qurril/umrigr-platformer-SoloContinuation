using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerSpawner : Singleton<NetworkPlayerSpawner>
{

  
    [SerializeField] public Transform[] spawnPoints;

    [Networked] public int LastSpawnIndex { get; set; }


    public Vector2 getSpawnLocation() {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return Vector2.zero;
        }
        Transform spawnPoint = spawnPoints[LastSpawnIndex];
        LastSpawnIndex = (LastSpawnIndex + 1) % spawnPoints.Length;
        return spawnPoint.position;


    }
}
