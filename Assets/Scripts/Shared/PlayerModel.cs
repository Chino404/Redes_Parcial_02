using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody _networkRgbd;
   // [SerializeField] private NetworkTransform _networkTransform;
    [SerializeField] private NetworkMecanimAnimator _networkAnimator;
    
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private Transform _shootPosition;

    [Networked(OnChanged = nameof(OnLifeChanged))]
    [SerializeField] private float Life { get; set; }
    [SerializeField] private float _maxLife;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private int _currentSign, _previousSign;

    [Networked(OnChanged = nameof(OnFiringChanged))]
    bool IsFiring { get; set; }

    private float _lastFiringTime;

    private NetworkInputData _inputs;    

    public event Action<float> OnLifeUpdate = delegate { }; 
    public event Action OnPlayerDespawn = delegate { }; 
    
    void Start()
    {
        transform.forward = Vector3.right;
        Life = _maxLife;
    }
    public override void Spawned()
    {
        LifeHandler.Instance.CreateBarLife(this);
    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out _inputs))
        {
            if (_inputs.isFirePressed) Shoot();
            if (_inputs.isJumpPressed) Jump();

            Move(_inputs.xMovement);
        }
    }

    void Move(float xAxi)
    {
        if (xAxi != 0)
        {
            _networkRgbd.Rigidbody.MovePosition(transform.position + Vector3.right * (xAxi * _speed * Time.fixedDeltaTime));

            _currentSign = (int)Mathf.Sign(xAxi);

            if (_currentSign != _previousSign)
            {
                _previousSign = _currentSign;

                transform.rotation = Quaternion.Euler(Vector3.up * (90 * _currentSign));
            }
            
            _networkAnimator.Animator.SetFloat("MovementValue", Mathf.Abs(xAxi));
        }
        else if (_currentSign != 0)
        {
            _currentSign = 0;
            _networkAnimator.Animator.SetFloat("MovementValue", 0);
        }
    }
    
    void Jump()
    {
        _networkRgbd.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }
    
    void Shoot()
    {
        if (Time.time - _lastFiringTime < 0.15f) return;

        _lastFiringTime = Time.time;

        Runner.Spawn(_bulletPrefab, _shootPosition.position, transform.rotation);

        StartCoroutine(FiringTime());

    }

    IEnumerator FiringTime()
    {
        IsFiring = true;

        yield return new WaitForSeconds(0.15f);    

        IsFiring = false;
    }

    public void TakeDamage(float dmg) => RPC_TakeDamage(dmg);


    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float dmg)
    {
        Life -= dmg;
        if (Life < 0) Dead();
    }

    private void Dead()
    {
        Runner.Shutdown();
    }

    static void OnFiringChanged(Changed<PlayerModel> changed)
    {
        var updateFiring = changed.Behaviour.IsFiring;
        changed.LoadOld();
        var oldFiring = changed.Behaviour.IsFiring;

        if (!oldFiring && updateFiring) 
        {
            changed.Behaviour._shootParticle.Play();
        }
    }

    static void OnLifeChanged(Changed<PlayerModel> changed)
    {
        var updateLife = changed.Behaviour;
        updateLife.OnLifeUpdate(updateLife.Life / updateLife._maxLife);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnPlayerDespawn();
    }
}
