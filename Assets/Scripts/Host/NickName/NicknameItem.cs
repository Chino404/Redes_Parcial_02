using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NicknameItem : MonoBehaviour
{
    const float Y_Offset = 3f;

    Transform _owner;

    TextMeshProUGUI _nickname;


    public void SetOwner(Transform owner)
    {
        _owner = owner;
        _nickname = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateName(string newName)
    {
        _nickname.text = newName;
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * Y_Offset;
    }
}
