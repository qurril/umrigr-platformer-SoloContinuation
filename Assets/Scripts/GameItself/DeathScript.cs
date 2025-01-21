using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCharacterController player = collision.GetComponent<PlayerCharacterController>();
            if (player != null)
            {
                player.HandleDeath();
            }
        }
    }
}
