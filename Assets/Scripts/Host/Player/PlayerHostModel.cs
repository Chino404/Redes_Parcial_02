using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHostModel : NetworkCharacterControllerPrototype
{
    [SerializeField] NetworkMecanimAnimator _mecanimAnim;
    private TrailRenderer _tr;
    

    public override void Spawned()
    {
        
        _tr = GetComponent<TrailRenderer>();
        GetComponent<LifeHostHandler>().OnRespawn += () => TeleportToPosition(Vector3.zero);
        //GetComponent<Teleports>().Teleport += () => TeleportToPosition(_teleport._nextTeleport.transform.position);

    }

    private void Start()
    {
        
        GameManager.instance.players.Add(this);
    }

    public override void Move(Vector3 direction)
    {
        var deltaTime = Runner.DeltaTime;
        var previousPos = transform.position;
        var moveVelocity = Velocity;

        direction = direction.normalized;

        if (IsGrounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }

        moveVelocity.y += gravity * Runner.DeltaTime;

        var horizontalVel = default(Vector3);
        horizontalVel.z = moveVelocity.x;

        if (direction == default)
        {
            horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
        }
        else
        {
            horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
            transform.rotation = Quaternion.Euler(Vector3.up * (90 * Mathf.Sign(direction.z)));
        }

        moveVelocity.x = horizontalVel.z;

        Controller.Move(moveVelocity * deltaTime);

        Velocity = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
        IsGrounded = Controller.isGrounded;

        _mecanimAnim.Animator.SetFloat("MovementValue", Velocity.sqrMagnitude);
    }

    public void Dash()
    {
        if (_canDash)
        {
            StartCoroutine(Dashing());
        }
    }

    IEnumerator Dashing()
    {
        _canDash = false;
        _isDashing = true;
        //float originalGravity = _networkRB.Rigidbody.;
        //networkRB.Rigidbody.gravityScale = 0f;
        //_networkRB.Rigidbody.velocity = new Vector2(transform.localScale.x * _dashingPower, 0f);
        Velocity = transform.forward * _dashingPower;
        _tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        _tr.emitting = false;
        _isDashing = false;
        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;
    }

    private void OnDestroy()
    {
        //GameManager.instance.CanvasLost();
        GameManager.instance.players.Remove(this);
    }
}
