using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerCharacterController : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if ( GetInput(out NetorkData input)){
            float moveX = input.Movement.x;
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

            if (input.Jump) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

        }

    }

    

}
