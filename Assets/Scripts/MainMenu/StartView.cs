using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : MonoBehaviour
{
    [SerializeField] private Button _start = null;
    [SerializeField] private Button _quit = null;
    [SerializeField] private GameObject _sessionExplore = null;
    [SerializeField] private GameObject _login = null;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 0;

        _start.onClick.AddListener(StartPressed);
        _quit.onClick.AddListener(() => {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        if (FusionConnection.Instance.PlayerName != null) {
            _login.SetActive(false);
            _sessionExplore.SetActive(true);
            FusionConnection.Instance.ConnectToLobby();
        }


    }

    private void StartPressed() {
        _start.interactable = false;
        _quit.interactable = false;

        FusionConnection.Instance.ConnectToLobby("TestName");
        _login.SetActive(false);
        _sessionExplore.SetActive(true);
    }

}
