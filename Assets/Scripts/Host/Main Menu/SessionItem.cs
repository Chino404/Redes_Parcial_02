using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;
using System;

public class SessionItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _sessionName, _playerCounts;
    [SerializeField] Button _joinLobby;


    // Start is called before the first frame update
    public void SetInfo(SessionInfo session, Action<SessionInfo> onClick)
    {
        _sessionName.text = session.Name;

        _playerCounts.text = $"{session.PlayerCount} / {session.MaxPlayers}";

        /*if(session.PlayerCount < session.MaxPlayers)
        _joinLobby.enabled = true;*/

        _joinLobby.enabled = session.PlayerCount < session.MaxPlayers;

        _joinLobby.onClick.AddListener(() => onClick(session));
    }
}
