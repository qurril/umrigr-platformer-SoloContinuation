using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerCharacterController : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] private int maxJumps = 3;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount;
    private bool jumpPressed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetorkData input))
        {
            // Horizontal movement
            float moveX = input.Movement.x;
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

            // Debounce jump input
            if (input.Jump && !jumpPressed && jumpCount < maxJumps)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpPressed = true;
                jumpCount++;
            }

            if (!input.Jump)
            {
                jumpPressed = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.1f)
        {
            isGrounded = true;
            jumpCount = 0;
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
