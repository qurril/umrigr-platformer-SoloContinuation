using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float attackRange = 1.5f;
    private GameObject jumpUICanvas;
    [SerializeField] private float maxDistance = 0.5f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public LayerMask platforms;

    [SerializeField] private int maxJumps = 3;


    [Networked] private int jumpCount { get; set; }
    private bool jumpPressed;

    private float lastAttackTime = 0f;
    private float attackCooldown = 1f;
    private bool isFacingRight = true;

    public float lastHitTime = 0f;
    private float hitDuration = 0.7f;
    private float pushbackForce = 8f; // Force to apply for pushback


    private float defaultGravityScale = 1f;

    private bool Active = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

                if (jumpUICanvas == null)
        {
            jumpUICanvas = transform.GetChild(0)?.gameObject;
            if (jumpUICanvas == null)
            {
                UnityEngine.Debug.LogError("JumpUICanvas not found in player hierarchy!");
            }
        }
        DisablePlayerControls();
    }
   

    public override void Spawned() {
        if (Runner.IsServer) InitServer();
        jumpUICanvas.SetActive(Object.HasInputAuthority);
    }

    private void InitServer() {
        //UnityEngine.Debug.Log($"{NetworkPlayerSpawner.Instance.getSpawnLocation().x} {NetworkPlayerSpawner.Instance.getSpawnLocation().y}");
        //rb.position = NetworkPlayerSpawner.Instance.getSpawnLocation();
    }
    private int lastCount = -50;
    void Update(){
        if(lastCount != jumpCount) {
            UpdateJumps();
            lastCount = jumpCount;
        }
    }

    public void DisablePlayerControls()
    {
        Active = false;
        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
        UnityEngine.Debug.Log($"Gravity stored as  {defaultGravityScale}");
    }

    public void EnablePlayerControls()
    {
        Active = true;
        rb.gravityScale = defaultGravityScale;
    }

    private void Flip()
    {
        UnityEngine.Debug.Log("Flipping");
        isFacingRight = !isFacingRight;

        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }


    public override void FixedUpdateNetwork()
    {

        if (Active && GetInput(out NetorkData input)){
          
        
            // Horizontal movement

            float moveX = 0;
            
            // hit duration is 0.7s, slows down after 0.5s
            // disable movement if target is recently hit
            if (Time.time - lastHitTime > hitDuration){
                moveX = input.Movement.x;
                rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
            } 
            // slow down x velocity after some time
            else if (Time.time - lastHitTime > hitDuration - 0.3f){
                
                float delta = ((Time.time - lastHitTime) - (hitDuration - 0.3f)) / 0.3f;
                rb.velocity = new Vector2(rb.velocity.x * (1 - delta) , rb.velocity.y);
            }


            if (moveX > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveX < 0 && isFacingRight)
            {
                Flip();
            }


            
            if (input.Attack && Time.time - lastAttackTime > attackCooldown)
            {
                lastAttackTime = Time.time;
                Vector2 position = transform.position;
                Vector2 scale = transform.localScale;
                int right = isFacingRight ? 1 : -1;

                Collider2D playerCollider = gameObject.GetComponent<Collider2D>();
                float height = playerCollider.bounds.size.y;  

                // check for other players in range
                Vector2 pointA = new Vector2(position.x, position.y - height / 2);
                Vector2 pointB = new Vector2((position.x + (attackRange * right)), position.y + height / 2);

                
                Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB);

                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Player") && collider.gameObject != gameObject)
                    {
                        UnityEngine.Debug.Log("Hit player");
                        if (collider.gameObject.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
                        {
                            RpcApplyPushback(networkObject, right);
                        }
                    }
                }

            }

            // Debounce jump input & cant jump if recently hit
            if (input.Jump && !jumpPressed && jumpCount < maxJumps && Time.time - lastHitTime > hitDuration)
            {

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpPressed = true;
                jumpCount++;
                //UnityEngine.Debug.Log("Jumped");
            }

            if (!input.Jump)
            {
                jumpPressed = false;
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcApplyPushback(NetworkObject target, int direction)
    {   
        if (target.TryGetComponent<Rigidbody2D>(out Rigidbody2D targetRb))
        {
            targetRb.velocity = new Vector2(direction * pushbackForce, 0);
            target.GetComponent<PlayerCharacterController>().lastHitTime = Time.time;
        }
    }

    private void UpdateJumps()
    {
        GameObject jumpCirclesHolder = jumpUICanvas.transform.GetChild(0).GetChild(0).gameObject;

        for (int i = 0; i < maxJumps; i++)
        {
            var jumpCircle = jumpCirclesHolder.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
            jumpCircle.color = (i < jumpCount) ? Color.grey : Color.yellow;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.1f)
        {
            isGrounded = true;
            jumpCount = 0;
            //UpdateJumps();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    [Rpc(RpcSources.StateAuthority,  RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_HandleDeath()
    {
       // if (Object.HasStateAuthority)
        {
            UnityEngine.Debug.Log($"Player {Object.InputAuthority.PlayerId} has died!");
            gameObject.SetActive(false);
        }
    }
}
