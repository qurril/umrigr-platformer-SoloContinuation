using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionInfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TextMeshProUGUI _playerCount = null;
    [SerializeField] private Toggle _toggle = null;

    public void ShowSession(string name, int playerCount, int maxPlayers, Action<bool, string> onToggle, ToggleGroup toggleGroup) {
        _name.text = name;
        _playerCount.text= "" + playerCount + "/" + maxPlayers;
        _toggle.onValueChanged.AddListener((isOn) => onToggle.Invoke(isOn, name));
        _toggle.group = toggleGroup;
    }
}
