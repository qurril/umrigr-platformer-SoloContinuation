using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : NetworkBehaviour
{
    public float speed = 5f;

    private float screenHeight;

    [Networked]
    public Vector3 NetworkedPosition { get; set; }

    void Start()
    {
        screenHeight = Camera.main.orthographicSize * 1.1f;

        Vector3 startPosition = new Vector3(transform.position.x, screenHeight, transform.position.z);
        NetworkedPosition = startPosition;
    }

    void Update()
    {
        if (HasStateAuthority)
        {
            Vector3 newPosition = NetworkedPosition + Vector3.down * speed * Time.deltaTime;

            if (newPosition.y <= -1 * screenHeight)
            {
                Runner.Despawn(Object); // Use Fusion's despawn for networked objects
            }
            else
            {
                NetworkedPosition = newPosition; // Update position on the network
            }

        }

        transform.Translate(Vector3.down * speed * Time.deltaTime);

        
    }
}
