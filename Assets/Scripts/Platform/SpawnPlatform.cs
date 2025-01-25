using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpawnPlatform : NetworkBehaviour
{
    private float platformLifetime = 8f;
    private float startTime;

    public Button startGameButton;
    private SpriteRenderer spriteRenderer;

    private bool despawnStarted = false;
    private bool isBlinking = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the MeshRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startGameButton.GetComponent<StartGameButton>().gameStarted && !despawnStarted){
            startTime = Time.time;
            despawnStarted = true;
            
        }

        if (despawnStarted){

            // 3 seconds before the platform despawns, start blinking
            if (Time.time - startTime >= (platformLifetime - 3) && isBlinking == false)
            {
                isBlinking = true;
                StartBlinking();
            }

            if (Time.time - startTime >= platformLifetime)
            {
                StopBlinking();
                if (HasStateAuthority)
                {
                    Runner.Despawn(Object);
                }
                
            }
            
        }
    }

    private void StartBlinking()
    {
        // Blink the SpriteRenderer by toggling its color
        spriteRenderer.enabled = true;
        spriteRenderer.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopBlinking()
    {
        spriteRenderer.DOKill();
        spriteRenderer.enabled = true;
    }
}
