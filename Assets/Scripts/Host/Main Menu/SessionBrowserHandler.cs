using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

public class SessionBrowserHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkRunner;

    [SerializeField] TextMeshProUGUI _statusText;

    [SerializeField] SessionItem _sessionPrefab;

    [SerializeField] VerticalLayoutGroup _parent;


    void OnEnable()
    {
        _networkRunner.OnSessionListUpdate += ReceiveSessionList;
    }

    void OnDisable()
    {
        _networkRunner.OnSessionListUpdate -= ReceiveSessionList;
    }

    void ReceiveSessionList(List<SessionInfo> allMySessions)
    {
        //Limpiar toda la lista de sesiones
        ClearList();
        //Checkear si la lista es nula
        if(allMySessions.Count == 0)
        {
            NoSessionFound();
            return;
        }
        //Por cada sesion, instancearlo en la lista
        foreach (var session in allMySessions)
        {
            AddNewSessionItem(session);
        }
    }

    void ClearList()
    {
        foreach (GameObject item in _parent.transform) Destroy(item);

        _statusText.gameObject.SetActive(false);
    }

    void NoSessionFound()
    {
        _statusText.gameObject.SetActive(true);
        _statusText.text = "No session founded";
    }

    void AddNewSessionItem(SessionInfo session)
    {
        var newItem = Instantiate(_sessionPrefab, _parent.transform);
        newItem.SetInfo(session, JoinSelectedSession);
    }

    void JoinSelectedSession(SessionInfo session)
    {
        _networkRunner.JoinSession(session);
    }
}
