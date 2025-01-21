using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerCharacterController : NetworkBehaviour
{
    // Start is called before the first frame update

    
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] private float maxDistance = 0.5f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public LayerMask platforms;

    private float defaultGravityScale = 1f;

    private bool Active = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        DisablePlayerControls();

    }
   

    public override void Spawned() {
        if (Runner.IsServer) InitServer();
    }

    private void InitServer() {
        UnityEngine.Debug.Log($"{NetworkPlayerSpawner.Instance.getSpawnLocation().x} {NetworkPlayerSpawner.Instance.getSpawnLocation().y}");
        rb.position = NetworkPlayerSpawner.Instance.getSpawnLocation();
    }


    public void DisablePlayerControls()
    {
        Active = false;
        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
    }

    public void EnablePlayerControls()
    {
        Active = true;
        rb.gravityScale = defaultGravityScale;
    }


    public override void FixedUpdateNetwork()
    {
        if (Active && GetInput(out NetorkData input)){
            float moveX = input.Movement.x;
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

            
            if (input.Jump && checkGrounded()) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

        }

    }

    private bool checkGrounded() {
        
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, maxDistance, platforms);
        UnityEngine.Debug.DrawRay(rb.position, Vector2.down * maxDistance, Color.red);

        if (hit.collider != null) {
            UnityEngine.Debug.Log("Hit object: " + hit.collider.gameObject.name + " at distance: " + hit.distance);

            return true;
        }

        return false;
    }



    

}
