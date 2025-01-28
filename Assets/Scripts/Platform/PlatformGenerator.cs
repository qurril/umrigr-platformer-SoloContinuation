using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlatformGenerator : NetworkBehaviour
{
    [SerializeField]
    NetworkPrefabRef[] platformPrefabs;
    [Tooltip("The rate at which platforms are spawned, in seconds between platforms")]
    [SerializeField]
    float spawnRate = 2f;
    [SerializeField]
    float maxHorizontalSpeed = 10f;
    [SerializeField]
    float minHorizontalSpeed = 3f;
    [SerializeField]
    float verticalSpeed = 5f;
    [SerializeField]
    float maxWidth = 9f;
    [SerializeField]
    float minWidth = 3f;

    float screenWidth = 1280;

   private NetworkRunner Runner;

    void Start(){
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
        Runner = FindObjectOfType<NetworkRunner>();
    }
    void Update()
    {
        if (Time.time % spawnRate < Time.deltaTime && Runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-screenWidth / 2, screenWidth / 2) + Camera.main.transform.position.x, Camera.main.orthographicSize * 1.1f, 0);

            Vector3 scaleVec = new Vector3(Random.Range(minWidth, maxWidth), 0.2f, 1);
            // Randomly select a platform prefab
            NetworkPrefabRef platformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

            NetworkObject instance = Runner.Spawn(platformPrefab, spawnPosition, Quaternion.identity, null, (runner, obj) =>
            {
                var platformScript = obj.GetComponent<PlatformScript>();
                platformScript.scale = scaleVec;
                platformScript.speedHorizontal = Random.Range(minHorizontalSpeed, maxHorizontalSpeed);
                platformScript.speedVertical = verticalSpeed;
            });
        }

    }
}
