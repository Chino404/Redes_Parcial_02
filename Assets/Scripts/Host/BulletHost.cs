using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHost : NetworkRigidbody
{
    TickTimer _expireTimer = TickTimer.None;
    [SerializeField] private float _speed = 20f;
    [SerializeField] private byte _damage = 25;

    public override void Spawned()
    {
        base.Spawned();
        Rigidbody.AddForce(transform.forward * _speed, ForceMode.VelocityChange);

        if (Object.HasStateAuthority) _expireTimer = TickTimer.CreateFromSeconds(Runner, 3f);
    }

    public override void FixedUpdateNetwork()
    {
       if(!Object.HasStateAuthority) return;

        if (_expireTimer.Expired(Runner)) DespawnObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out LifeHostHandler enemy))
        {
            enemy.TakeDamage(_damage);
        }

        DespawnObject();
    }

    void DespawnObject()
    {
        _expireTimer = TickTimer.None;
        Runner.Despawn(Object);
    }
}
