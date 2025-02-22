using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionView : Singleton<SessionView>
{
    [SerializeField] private Button _createRoom = null;
    [SerializeField] private Button _refreshButton = null;
    [SerializeField] private Button _joinButton = null;
    [SerializeField] private Button _backButton = null;
    [SerializeField] private GameObject _roomCreate=null;
    [SerializeField] private GameObject _sessionView = null;
    [SerializeField] private GameObject _mainMenu = null;
    [SerializeField] private SessionInfoView _sessionInfoprefab = null;

    [SerializeField] private ToggleGroup _sessionListContainer = null;

    private string sessionName;
    private List<SessionInfoView> _sessions = new List<SessionInfoView>();

    private IEnumerator waitSeconds(int seconds) {
        yield return new WaitForSeconds(seconds);
    }

    private void Awake()
    {
        

        base.Awake();
        _createRoom.onClick.AddListener(() =>
        {
            _roomCreate.SetActive(true);
            gameObject.SetActive(false);
        });

        _joinButton.interactable = false;

        UpdateSessionList();
        _refreshButton.onClick.AddListener(UpdateSessionList);
        _joinButton.onClick.AddListener(() => {
            FusionConnection.Instance.JoinSession(sessionName);
        });
        _backButton.onClick.AddListener(() => { 
            
           // FusionConnection.Instance.QuitLobby();
            _sessionView.SetActive(false);
            _mainMenu.SetActive(true) ;
        });
    }
    public void UpdateSessionList() {

        

        foreach (var session in _sessions) {
            Destroy(session.gameObject);
        }
        _sessions.Clear();


        foreach (var sesInf in FusionConnection.Instance.Sessions) {
            SessionInfoView data = Instantiate(_sessionInfoprefab, _sessionListContainer.transform);
            data.ShowSession(sesInf.Name, sesInf.PlayerCount, sesInf.MaxPlayers, (isOn, sessionName) => SessionView.Instance.SessionOnToggle(isOn, sessionName), _sessionListContainer);
            _sessions.Add(data);
        }
    }

    private void SessionOnToggle(bool isOn, string sessionNameGiven) {
        if (isOn) {
            sessionName = sessionNameGiven;
            _joinButton.interactable = true;

        }else if(sessionName.CompareTo(sessionNameGiven) == 0)
        {
            _joinButton.interactable=false;
        }
    }
}
