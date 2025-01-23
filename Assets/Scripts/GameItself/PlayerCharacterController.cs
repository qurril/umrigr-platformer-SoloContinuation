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
    private GameObject jumpUICanvas;
    [SerializeField] private float maxDistance = 0.5f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public LayerMask platforms;

    [SerializeField] private int maxJumps = 3;


    [Networked] private int jumpCount { get; set; }
    private bool jumpPressed;


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


    public override void FixedUpdateNetwork()
    {

        if (Active && GetInput(out NetorkData input)){
          
        
            // Horizontal movement
            float moveX = input.Movement.x;
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

            // Debounce jump input
            if (input.Jump && !jumpPressed && jumpCount < maxJumps)
            {

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpPressed = true;
                jumpCount++;
                UnityEngine.Debug.Log("Jumped");
            }

            if (!input.Jump)
            {
                jumpPressed = false;
            }
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

    public void HandleDeath()
    {
        if (Object.HasStateAuthority)
        {
            UnityEngine.Debug.Log($"Player {Object.InputAuthority.PlayerId} has died!");
            gameObject.SetActive(false);
        }
    }
}
