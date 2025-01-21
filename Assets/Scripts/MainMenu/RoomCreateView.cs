using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomCreateView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _roomNameIn = null;
    [SerializeField] private TMP_InputField _maxPlayers = null;
    [SerializeField] private Button _createButton = null;
    [SerializeField] private Button _quiteButton = null;
    [SerializeField] private GameObject _sessionView = null;

    private void Awake()
    {


        _quiteButton.onClick.AddListener(() => { 
            _sessionView.SetActive(true);
            gameObject.SetActive(false); 
        });

        _createButton.onClick.AddListener(() => {
            CreateRoom();
        });
    }

    private void CreateRoom() {
        FusionConnection.Instance.CreateSession(_roomNameIn.text, _maxPlayers.text);
    }

}
