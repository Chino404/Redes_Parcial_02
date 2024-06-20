using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerHostGun : NetworkBehaviour
{
    [SerializeField] BulletHost _bulletPrefab;
    [SerializeField] Transform _bulletSpawner;
    [SerializeField] ParticleSystem _shootParticle;
    [SerializeField] float _shootCooldown = 0.2f;

    float _lastShootTime;

    [Networked(OnChanged = nameof(OnFiringChanged))]
    bool IsFiring { get; set; }

    public void Start()
    {
        AudioManager.instance.StopSFX();
    }

    public void Shoot()
    {
        if (Time.time - _lastShootTime < _shootCooldown) return;

        _lastShootTime = Time.time;

        StartCoroutine(ShootCooldown());
        if(Object.HasInputAuthority)AudioManager.instance.PlaySFX(AudioManager.instance.shoot);
        Runner.Spawn(_bulletPrefab, _bulletSpawner.position, transform.rotation);

        #region Raycast
        /*var Raycast = Runner.LagCompensation.Raycast(origin: _bulletSpawner.position,
                                                     direction: _bulletSpawner.forward,
                                                     length: 500,
                                                     player: Object.InputAuthority,
                                                     hit: out var hitInfo);

        if (!Raycast) return;

        hitInfo.GameObject.GetComponentInParent<LifeHostHandler>()?.TakeDamage(25);
        Debug.Log(hitInfo.Hitbox.Root.gameObject.name);*/
        #endregion
    }

    IEnumerator ShootCooldown()
    {
        IsFiring = true;
        yield return new WaitForSeconds(_shootCooldown);
        IsFiring = false;
    }

    static void OnFiringChanged(Changed<PlayerHostGun> changed)
    {
        var currentFiring = changed.Behaviour.IsFiring;
        changed.LoadOld();
        var oldFiring = changed.Behaviour.IsFiring;

        if (!oldFiring && currentFiring)
        {
            changed.Behaviour._shootParticle?.Play();
        }
    }
}
