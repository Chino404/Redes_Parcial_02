using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Updatear vida actual y posicion de la barra de vida

public class Lifebar : MonoBehaviour
{

    [SerializeField] float _offSet;
    [SerializeField] Image _lifeBarImage;

    Transform _target;

    public void UpdatePosition() => transform.position = _target.position + Vector3.up * _offSet;

    public void UpdateLifeBar(float amount) => _lifeBarImage.fillAmount = amount;

    public Lifebar SetTarget(PlayerModel target)
    {
        _target = target.transform;

        target.OnLifeUpdate += UpdateLifeBar;

        return this;
    }

}
