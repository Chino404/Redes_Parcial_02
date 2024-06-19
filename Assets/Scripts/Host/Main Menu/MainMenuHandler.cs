using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    [SerializeField] NetworkRunnerHandler _networkRunner;

    [Space(25), Header("Panels"), SerializeField] GameObject _joinLobbyPanel;
    [SerializeField] GameObject _joiningLobbyStatusPanel,_sessionBrowserPanel, _hostSessionPanel;

    [Space(25), Header("Buttons"), SerializeField] Button _joinLobbyButton;
    [SerializeField] Button _hostLobbyPanelButton, _hostSessionButton;

    [Space(25), Header("InputField"), SerializeField] TMP_InputField _hostSessionName;
    [SerializeField] TMP_InputField _nickNameInputField;

    [Space(25), Header("Text"), SerializeField] TextMeshProUGUI _statusText;
    [SerializeField] string _sceneName;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.Play();
        

        _joinLobbyPanel.SetActive(true);
        _joiningLobbyStatusPanel.SetActive(false);
        _sessionBrowserPanel.SetActive(false);
        _hostSessionPanel.SetActive(false);

        _joinLobbyButton.onClick.AddListener(JoinLobby);
        _hostLobbyPanelButton.onClick.AddListener(HostLobbyPanel);
        _hostSessionButton.onClick.AddListener(HostGame);

        _networkRunner.OnLobbyConected += () =>
        {
            _joiningLobbyStatusPanel.SetActive(false);
            _sessionBrowserPanel.SetActive(true);
        };
    }

    void JoinLobby()
    {
        _networkRunner.JoinLobby();

        PlayerPrefs.SetString("NickNameSave", _nickNameInputField.text);

        _joinLobbyPanel.SetActive(false);
        _joiningLobbyStatusPanel.SetActive(true);
        StartCoroutine(JoiningTextChange());
    }

    IEnumerator JoiningTextChange()
    {
        while(_joiningLobbyStatusPanel.activeInHierarchy)
        {
            _statusText.text = "Joining.";
            yield return new WaitForSeconds(0.15f);
            _statusText.text = "Joining..";
            yield return new WaitForSeconds(0.15f);
            _statusText.text = "Joining...";
            yield return new WaitForSeconds(0.5f);
        }
    }

    void HostLobbyPanel()
    {
        _sessionBrowserPanel.SetActive(false);
        _hostSessionPanel.SetActive(true);
    }

    void HostGame()
    {
        _networkRunner.CreateSession(_hostSessionName.text, _sceneName);
    }
}
