using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickNameHandler : MonoBehaviour
{
    public static NickNameHandler Instance;

    List<NicknameItem> _itemNames = new List<NicknameItem>();

    [SerializeField] NicknameItem _nicknamePrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public NicknameItem CreateNewNickName(NetworkHostPlayer owner)
    {
        var nickname = Instantiate(_nicknamePrefab, transform);
        _itemNames.Add(nickname);

        nickname.SetOwner(owner.transform);

        owner.OnPlayerDespawn += () =>
        {
            _itemNames.Remove(nickname);
            Destroy(nickname.gameObject);
        };

        return nickname;
    }


    void LateUpdate()
    {
        foreach (var nick in _itemNames) nick.UpdatePosition();
    }
}
