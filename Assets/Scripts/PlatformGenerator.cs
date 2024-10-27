using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] platformPrefabs;
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

    void Update()
    {
        if (Time.time % spawnRate < Time.deltaTime)
        {
            Vector3 spawnPosition = new Vector3(0, Camera.main.orthographicSize * 1.1f, 0);

            // Randomly select a platform prefab
            GameObject platformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            GameObject instance = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

            instance.transform.localScale = new Vector3(Random.Range(minWidth, maxWidth), 0.2f, 1);
            instance.GetComponent<HorizontalMovement>().speed = Random.Range(minHorizontalSpeed, maxHorizontalSpeed);
            instance.GetComponent<VerticalMovement>().speed = verticalSpeed;
        }

    }
}
