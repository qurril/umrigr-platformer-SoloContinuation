using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UIElements;

public class PlatformScript : NetworkBehaviour
{
    public float speedHorizontal = 5f;
    public float speedVertical = 5f;
    public float motionMagnitude = 3f;
    


    private float screenHeight;
    private float screenWidth;

    [Networked] private float NetworkedDirection { get; set; }
    [Networked] public Vector3 scale { get; set; }

    public override void Spawned()
    {

        transform.localScale = scale;

    
        // Cache screen dimensions
        screenHeight = Camera.main.orthographicSize * 1.1f;
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;

        if (HasStateAuthority)
        {
            // Initialize direction on the authoritative client
            NetworkedDirection = Random.value < 0.5f ? -1f : 1f;
        }
    }
    public override void FixedUpdateNetwork()
    {


            

            Vector3 newPosition = transform.position +
                                      (Vector3.down * speedVertical * Runner.DeltaTime +
                                       Vector3.right * speedHorizontal * NetworkedDirection * Runner.DeltaTime);


        if (!HasStateAuthority) return;


            Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
            
            

           

            if (screenPosition.x <= 0.0f) {
                NetworkedDirection = Mathf.Abs(NetworkedDirection);
            }
            if (screenPosition.x >= 1.0f)
            {
                NetworkedDirection = -1*Mathf.Abs(NetworkedDirection);
            }

        transform.position = newPosition;

            if (newPosition.y <= -1 * screenHeight)
            {
                Runner.Despawn(Object); // Despawn networked objects using Fusion
            }


    }

}
