using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    // Start is called before the first frame update
    private PlayerCharacterController characterController  = null;

    public PlayerCharacterController PlayerCharacterController => characterController;
    public override void Spawned()
    {
        PlayerManager.Instance.RegisterPlayer(this);
        characterController  = GetComponent<PlayerCharacterController>();
    }

}
