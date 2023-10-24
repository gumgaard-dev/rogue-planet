using Capstone.Build.Characters.Player.PlayerStates;
using System.Collections.Generic;
using UnityEngine;


namespace Capstone.Build.Characters.Player
{
    public class Player : MonoBehaviour
    {

        public Vector3 Position => transform.position;
        public Vector2 Velocity => _rigidBody.velocity;
        public float Facing => transform.localScale.x;
        public Vector3 Scale => transform.localScale;
        public Bounds Bounds => _bodyCollider.bounds;

        // Reference to current player state (Abstract class, states are upcast to this)
        public PlayerState State { get; private set; }
        // Enum reference to current state name
        public PlayerStateType StateType;

        // Mapping enum state names to actual instance of state object
        private Dictionary<PlayerStateType, PlayerState> _playerStates;

        private GameSettings _settings;

        private TriggerInfo _triggerInfo;

        private Animator _animator;
        private Collider2D _bodyCollider;
        private Rigidbody2D _rigidBody;

        public LayerMask ShipLayer;

        // ref to player's ship
        public Ship ship;

        private bool _isNearShip;
        public bool IsNearShip {  get { return _isNearShip; } }

        public void AwakeManaged()
        {
            _settings = Resources.Load<GameSettings>("Settings/GameSettings");

            _triggerInfo = GetComponent<TriggerInfo>();

            _animator = GetComponent<Animator>();
            _bodyCollider = GetComponent<Collider2D>();
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void StartManaged() 
        {
            _playerStates = new Dictionary<PlayerStateType, PlayerState>
            {
                [PlayerStateType.Run] = new PlayerRunState(_settings, this),
                [PlayerStateType.Duck] = new PlayerDuckState(_settings, this),
                [PlayerStateType.Idle] = new PlayerIdleState(_settings, this),
                [PlayerStateType.Fall] = new PlayerFallState(_settings, this),
                [PlayerStateType.InShip] = new InShipState(_settings, this),
            };

            SetState(PlayerStateType.Run);
        }

        public void UpdateManaged()
        {
            State.UpdateManaged();
        }

        public void FixedUpdateManaged()
        {
            State.FixedUpdateManaged();
        }

        public void SetState(PlayerStateType stateType)
        {
            if(State != null)
            {
                State.Exit();
            }
            

            StateType = stateType;
            State = _playerStates[stateType];

            State.Enter();
        }

        public void SetPosition(float x, float y) 
        {
            transform.position = new Vector2(x, y);
        }

        public void SetPosition(Vector2 position)
        {
            SetPosition(position.x, position.y);
        }

        public void SetVelocity(float x, float y) 
        {
            if (x == 0)
            {
                State.ResetVelocityXDamping();
            }
            else if (Mathf.Abs(x) < _settings.MinMoveSpeed)
            {
                x = 0;
            }

            _rigidBody.velocity = new Vector2(x, y);
        }

        public void SetVelocity(Vector2 velocity)
        {
            SetVelocity(velocity.x, velocity.y);
        }

        public void SetFacing(float facing)
        {
            transform.localScale = new Vector3(facing, transform.localScale.y, transform.localScale.z);
        }

        public void SetGravityScale(float gravityScale)
        {
            _rigidBody.gravityScale = gravityScale;
        }

        public void SetAnimation(string stateName)
        {
            SetAnimationSpeed(1);

            _animator.Play($"Base Layer.Player-{stateName}");
        }

        public void SetAnimationSpeed(float speed)
        {
            _animator.speed = speed;
        }


        void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.GetComponentInParent<Ship>())
            {
                this._isNearShip = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == ShipLayer)
            {
                this._isNearShip = false;
            }
        }


        public void UpdateFacing()
        {
            // Checking the direction the player is currently facing, then checking if the velocity is in the same direcition.
            // Using MinMoveSpeed to not erratically change facing direction if a collision is being resolved and Unity is moving the player.
            if (Facing != 1 && Velocity.x >= _settings.MinMoveSpeed)
            {
                SetFacing(1);
            }
            else if (Facing != -1 && Velocity.x <= -_settings.MinMoveSpeed)
            {
                SetFacing(-1);
            }
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = new Color(1, 0, 1, 0.5f);

                Gizmos.DrawWireCube(_triggerInfo.GroundBounds.center, _triggerInfo.GroundBounds.size);
            }
        }
    }
}
