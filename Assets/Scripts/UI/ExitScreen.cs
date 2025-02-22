using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

public class ExitScreen : MonoBehaviour
{

    public GameObject exitScreen;

    public Button gameStart = null;
    public Button _quit = null;
    public Button _mainMenu = null;

    public TMP_Text winnerTitle;


    private bool isExitScreenActive = false;
    private bool triggeredEnd = false;


    private void Awake()
    {
         _mainMenu.onClick.AddListener(() => {
             FusionConnection.Instance.LeaveSession();
             //UnityEngine.SceneManagement.SceneManager.LoadScene("SessionsTest", UnityEngine.SceneManagement.LoadSceneMode.Single);
        });

        _quit.onClick.AddListener(() => {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isExitScreenActive = !isExitScreenActive;
            exitScreen.SetActive(isExitScreenActive);
            // startGameButton.SetActive(!isExitScreenActive);
            // platformGenerator.SetActive(!isExitScreenActive);
        }


        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        NetworkObject player = null;
        int playerCount = 0;
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "Character(Clone)")
            {
                playerCount++;
                player = obj.GetComponent<NetworkObject>();
            }

        }
        
        if (!triggeredEnd && playerCount <= 1 && gameStart.GetComponent<StartGameButton>().gameStarted)
        {
            isExitScreenActive = true;
            triggeredEnd = true;
            exitScreen.SetActive(isExitScreenActive);

            winnerTitle.text = "Player " + player.InputAuthority.PlayerId + " wins!";

            
            Debug.Log("Player name: " + player.InputAuthority.PlayerId);
        }


    }
}
