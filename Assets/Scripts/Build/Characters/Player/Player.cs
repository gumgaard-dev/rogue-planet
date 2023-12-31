using Capstone.Build.Characters.Player.Animation;
using Capstone.Build.Characters.Player.PlayerStates;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Capstone.Build.Characters.Player
{
    [System.Serializable] public class PlayerMovedEvent : UnityEvent<Vector3> { }
    public class Player : MonoBehaviour
    {
        public string SETTINGS_PATH = "Settings/GameSettings";
        public bool Aiming { get; set; }
        public Vector3 Position => transform.position;
        public Vector2 Velocity => _rigidBody.velocity;
        public float Facing => transform.rotation.y == 0 ? 1 : -1;
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
        private Collider2D _bodyCollider;
        private Rigidbody2D _rigidBody;
        private PlayerAimController _aimController;

        // a reference to the player's ship used by the in-ship state
        [SerializeField]private Ship.Ship _ship;
        public Ship.Ship Ship {  get { return _ship; } }
        public UnityEvent EnterShip;
        public UnityEvent ExitShip;
        public PlayerMovedEvent PlayerMoved;

        private UpgradeMenuController _upgradeMenuController;
        
        // used to determine if player is close enough to enter the ship
        private bool _isNearShip;
        public bool IsNearShip {  get { return _isNearShip; } }

        public Jetpack Jetpack { get; private set; }
        private DeployableInventory _deployableInventory;


        public void AwakeManaged()
        {
            _settings = Resources.Load<GameSettings>(SETTINGS_PATH);
            if(_settings == null)
            {
                Debug.LogWarning("Player could not load GameSettings at " + SETTINGS_PATH);
            }

            if (!TryGetComponent(out _triggerInfo)) {
                Debug.LogWarning("Player has no TriggerInfo component");
            }
            if (!TryGetComponent(out _bodyCollider)) {
                Debug.LogWarning("Player has no Collider2D component");
            }
            if (!TryGetComponent(out _rigidBody)) {
                Debug.LogWarning("Player has no RigidBody2D component");
            }

            if (!_ship)
            {
                Debug.Log("Error: Player does not have a reference to ship.");
            }

            _aimController = GetComponentInChildren<PlayerAimController>();

            Jetpack = GetComponentInChildren<Jetpack>();

            _deployableInventory = GetComponentInChildren<DeployableInventory>();

            EnterShip.AddListener(DepositOreInShip);
        }


        public void StartManaged() 
        {
            _playerStates = new Dictionary<PlayerStateType, PlayerState>
            {
                [PlayerStateType.Run] = new RunPlayerState(_settings, this),
                [PlayerStateType.Duck] = new DuckPlayerState(_settings, this),
                [PlayerStateType.Idle] = new IdlePlayerState(_settings, this),
                [PlayerStateType.Fall] = new FallPlayerState(_settings, this),
                [PlayerStateType.Jetpack] = new JetpackPlayerState(_settings, this),
                [PlayerStateType.InShip] = new InShipState(_settings, this),
                [PlayerStateType.UpgradeMenu] = new UpgradeMenuState(_settings, this)
            };

            SetState(PlayerStateType.Run);
        }


        public void UpdateManaged() {
            PlayerMoved?.Invoke(this.transform.position);
            State.UpdateManaged(); 
        }


        public void FixedUpdateManaged() { State.FixedUpdateManaged(); }


        public void SetState(PlayerStateType stateType)
        {

            StateType = stateType;

            // call the old state's exit method to perform any necessary exit actions
            State?.Exit();

            State = _playerStates[stateType];
            
            // call the new state's enter method to perform any necessary enter actions
            State.Enter();
        }


        public void SetPosition(float x, float y) { 
            Vector2 newPosition = new Vector2(x, y);
            transform.position = newPosition;
        }

        public void SetPosition(Vector2 position) { SetPosition(position.x, position.y); }


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

            if (y == 0)
            {
                State.ResetVelocityYDamping();
            }
            else if (Mathf.Abs(y) < _settings.MinMoveSpeed)
            {
                y = 0;
            }

            _rigidBody.velocity = new Vector2(x, y);
        }
    

        public void SetVelocity(Vector2 velocity) { SetVelocity(velocity.x, velocity.y);}

        public void SetFacing(float facing) 
        {
            if (Mathf.Sign(facing) != Mathf.Sign(Facing))
            {
                transform.Rotate(Vector3.up, 180f * Mathf.Sign(facing));
            }
        }

        public void SetGravityScale(float gravityScale) { this._rigidBody.gravityScale = gravityScale; }

        // called when TriggerDetector invokes AreaEntered
        // sets a flag which determines whether the player can enter the ship
        public void OnEnterShipProximity()
        {
            this._isNearShip = true;
        }

        // called when TriggerDetector invokes AreaExited
        public void OnExitShipProximity()
        {
            this._isNearShip = false;
        }
        
        public void UpdateFacing()
        {
            // prioritize the direction the player is aiming, if not up or down
            if (_aimController.AimXDirection != 0)
            {
                if(_aimController.AimXDirection > 0)
                {
                    SetFacing(1);
                } else if (_aimController.AimXDirection < 0 )
                {
                    SetFacing(-1);
                }
            } 
            else
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
        }

        public void PlaceDeployable()
        {
            _deployableInventory.PlaceDeployable("Lamp");
        }

        public void AddToDeployableInventory(GameObject gObj)
        {
            _deployableInventory.AddToStorage(gObj);
        }

        public void DepositOreInShip()
        {
            Inventory playerInv = GetComponent<Inventory>();
            Inventory shipInv = Ship.GetComponent<Inventory>();

            Dictionary<object, int> TempDict = new(playerInv.storage);

            foreach (var oreEntry in TempDict)
            {
                object oreType = oreEntry.Key;
                int amount = oreEntry.Value;

                shipInv.AddToStorage(oreType, amount);
                playerInv.RemoveFromStorage(oreType, amount);
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
