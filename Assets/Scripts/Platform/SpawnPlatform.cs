using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : NetworkBehaviour
{
    public float platformLifetime = 5f;
    public float startTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy this game object after platformLifetime seconds
        if (HasStateAuthority)
        {
            if (Time.time - startTime >= platformLifetime)
            {
                Runner.Despawn(Object);
            }
        }
    }
}
