
using Fusion;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomNetworkedRigidBody2d : NetworkBehaviour
{
    private Rigidbody2D rb;

    [Networked]
    public Vector2 NetworkedPosition { get; set; } // Synchronized position

    [Networked]
    public Vector2 NetworkedVelocity { get; set; } // Synchronized velocity

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            // Update position and velocity for other clients
            NetworkedPosition = rb.position;
            NetworkedVelocity = rb.velocity;
        }
        else
        {
            // Smoothly interpolate position and apply velocity
            rb.position = Vector2.Lerp(rb.position, NetworkedPosition, Runner.DeltaTime * 10f);
            rb.velocity = NetworkedVelocity;
        }
    }
}
