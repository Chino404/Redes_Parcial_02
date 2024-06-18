using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Crear una barra de vida por player
//Destruir esa barra de vida
//Actualizar las barras de vida

public class LifeHandler : MonoBehaviour
{
    public static LifeHandler Instance { get; private set; }
    [SerializeField] Lifebar _prefabLifeBar;

    List<Lifebar> _lifeBarList = new List<Lifebar>();   

    void Awake()
    {
        if(Instance) Destroy(gameObject);
        else Instance = this;
    }

    public void CreateBarLife(PlayerModel player)
    {
        Lifebar lifeBar = Instantiate(_prefabLifeBar, transform).SetTarget(player);

        _lifeBarList.Add(lifeBar);

        player.OnPlayerDespawn += () =>
        {
            _lifeBarList.Remove(lifeBar);
            Destroy(lifeBar.gameObject);
        };
    }

    void LateUpdate()
    {
        foreach (var item in _lifeBarList)
        {
            item.UpdatePosition();
        }
    }
}
