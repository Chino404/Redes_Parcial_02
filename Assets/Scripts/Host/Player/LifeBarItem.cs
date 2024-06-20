using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarItem : MonoBehaviour
{
    [SerializeField] float _offSet;
    [SerializeField] Image _lifeBarImage;

    Transform _target;

    public void UpdatePosition() => transform.position = _target.position + Vector3.up * _offSet;

    public void UpdateLifeBar(float amount) => _lifeBarImage.fillAmount = amount;

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
