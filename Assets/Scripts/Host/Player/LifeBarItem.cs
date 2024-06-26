using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class LifeBarItem : MonoBehaviour
{
    [SerializeField] float _offSet;
    [SerializeField] Image _lifeBarImage;

    Transform _target;

    public void UpdatePosition() => transform.position = _target.position + Vector3.up * _offSet;

    public void UpdateLifeBar(float amount) => _lifeBarImage.fillAmount = amount;

    //public void SetTarget(Transform target)
    //{
    //    _target = target;
    //}

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
    public LifeBarItem SetTarget(NetworkHostPlayer target)
    {
        _target = target.transform;
        

        return this;
    }

    public LifeBarItem SetLife(LifeHostHandler life)
    {
        life.OnLifeUpdate += UpdateLifeBar;
        return this;
    }
}
