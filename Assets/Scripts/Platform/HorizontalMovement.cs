using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : NetworkBehaviour
{
    public float speed = 5f;
    public float motionMagnitude = 3f;

    private Vector3 startPos;
    private float direction;

    [Networked]
    public Vector3 NetworkedPosition { get; set; }
    [Networked]
    public float NetworkedDirection { get; set; }
    void Start()
    {
        direction = Random.value < 0.5f ? -1f : 1f;

        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;


        if (HasStateAuthority)
        {
            // Initialize random position and direction
            float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
            Vector3 startPosition = new Vector3(randomX, transform.position.y, transform.position.z);
            direction = Random.value < 0.5f ? -1f : 1f;

            // Synchronize position and direction across clients
            NetworkedPosition = startPosition;
            NetworkedDirection = direction;
        }
    }

    void Update()
    {
        
        if (HasStateAuthority)
        {
            // Host or authoritative client controls movement
            Vector3 newPosition = NetworkedPosition + Vector3.right * NetworkedDirection * speed * Time.deltaTime;

            // Reflect at screen boundaries
            Vector3 screenPosition = Camera.main.WorldToViewportPoint(newPosition);
            if (screenPosition.x <= 0.0f || screenPosition.x >= 1.0f)
            {
                NetworkedDirection = -NetworkedDirection;
            }

            // Update position and direction on the network
            NetworkedPosition = newPosition;
        }

        transform.position = NetworkedPosition;
    }
}
