using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;

public class StartGameButton : NetworkBehaviour
{
    public Button startGameButton;

    public GameObject platformGenerator;
    
    public bool gameStarted = false;


    // Start is called before the first frame update
    void Start()
    {   

        if (Runner.IsClient)
        {
            // client disables the button
            startGameButton.interactable = false;
            startGameButton.GetComponent<Image>().enabled = false;
            TMP_Text[] childTexts = startGameButton.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text text in childTexts)
            {
                text.enabled = false;
            }
        }

        startGameButton.onClick.AddListener(RpcStartGame);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    void RpcStartGame()
    {
        gameStarted = true;
        platformGenerator.SetActive(true);
        GameManager.Instance.StartGameCountdown();

        StartCoroutine(PlatformGeneratorAfterDelay());

        // server disables the button
        startGameButton.interactable = false;
        startGameButton.GetComponent<Image>().enabled = false;
        TMP_Text[] childTexts = startGameButton.GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text text in childTexts)
        {
            text.enabled = false;
        }
    }

    private IEnumerator PlatformGeneratorAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);
        platformGenerator.GetComponent<PlatformGenerator>().startSpawning = true;
    }

}
