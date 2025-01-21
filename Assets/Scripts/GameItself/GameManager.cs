using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public List<Transform> spawnPoints = new List<Transform>();

    public override void Spawned()
    {
        Debug.Log("GameManager spawned");
        GameObject[] spawnPlatforms = GameObject.FindGameObjectsWithTag("SpawnPlatform");
        spawnPoints = spawnPlatforms.Select(spawnPlatform => spawnPlatform.transform).ToList();
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
}
