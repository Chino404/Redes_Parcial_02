    using System;
using System.Collections;
using Fusion;
    using UnityEngine;
    

    [RequireComponent(typeof(CharacterController))]
    [OrderBefore(typeof(NetworkTransform))]
    [DisallowMultipleComponent]
    // ReSharper disable once CheckNamespace
    public class NetworkCharacterControllerPrototype : NetworkTransform 
    {

      [Header("Character Controller Settings")]
      public float gravity       = -20.0f;
      public float jumpImpulse   = 8.0f;
      public float acceleration  = 10.0f;
      public float braking       = 10.0f;
      public float maxSpeed      = 10.0f;
      public float rotationSpeed = 15.0f;

        [SerializeField] protected float _dashingPower = 24f;
        [SerializeField] protected float _dashingCooldown = 0.5f;
        protected float _dashingTime = 0.2f;
        protected bool _canDash = true, _isDashing = false;
        protected bool doubleJump;


      [Networked]
      [HideInInspector]
      public bool IsGrounded { get; set; }

      [Networked]
      [HideInInspector]
      public Vector3 Velocity { get; set; }

        [Networked(OnChanged = nameof(OnJumpChanged))]
        bool IsJumping { get; set; }

        public ParticleSystem _JumpParticleSystem;
 
      protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;

      protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, rotationSpeed);

      public CharacterController Controller { get; private set; }

      protected override void Awake() {
        base.Awake();
        CacheController();
      }

      public override void Spawned() {
        base.Spawned();
        CacheController();
      }

      private void CacheController() {
        if (Controller == null) {
          Controller = GetComponent<CharacterController>();

          Assert.Check(Controller != null, $"An object with {nameof(NetworkCharacterControllerPrototype)} must also have a {nameof(CharacterController)} component.");
        }
      }

      protected override void CopyFromBufferToEngine() {
    
        Controller.enabled = false;

    
        base.CopyFromBufferToEngine();

    
        Controller.enabled = true;
      }

  
      public virtual void Jump(bool ignoreGrounded = false, float? overrideImpulse = null) 
      {
        if (IsGrounded || ignoreGrounded||doubleJump) 
        {
            StartCoroutine(JumpCooldown());
          var newVel = Velocity;
          newVel.y += overrideImpulse ?? jumpImpulse;
            doubleJump = !doubleJump;
          Velocity =  newVel;
        }
      }

    IEnumerator JumpCooldown()
    {
        IsJumping = true;
        yield return new WaitForSeconds(0.2f);
        IsJumping = false;
    }

    static void OnJumpChanged(Changed<NetworkCharacterControllerPrototype> changed)
    {
        var currentJumping = changed.Behaviour.IsJumping;
        changed.LoadOld();
        var oldJumping = changed.Behaviour.IsJumping;

        if (!oldJumping && currentJumping)
        {
            changed.Behaviour._JumpParticleSystem?.Play();
        }
    }

        public void RestartDoubleJump()
        {
        if (IsGrounded) doubleJump = false;
        }
            
        
      public virtual void Move(Vector3 direction) 
      {
            var deltaTime    = Runner.DeltaTime;
            var previousPos  = transform.position;
            var moveVelocity = Velocity;

            direction = direction.normalized;

            if (IsGrounded && moveVelocity.y < 0) {
              moveVelocity.y = 0f;
            }

            moveVelocity.y += gravity * Runner.DeltaTime;

            var horizontalVel = default(Vector3);
            horizontalVel.x = moveVelocity.x;
            horizontalVel.z = moveVelocity.z;

            if (direction == default) 
            {
                 horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
            } 
            else 
            {
              horizontalVel      = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
              transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Runner.DeltaTime);
            }

            moveVelocity.x = horizontalVel.x;
            moveVelocity.z = horizontalVel.z;

            Controller.Move(moveVelocity * deltaTime);

            Velocity   = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
            IsGrounded = Controller.isGrounded;
      }

   
}