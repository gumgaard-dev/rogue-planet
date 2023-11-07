using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone.Build.Characters.Player
{
    [System.Serializable] public class GroundedEvent : UnityEvent { }
    public class TriggerInfo : MonoBehaviour
    {
        private GameSettings _settings;
        private Player _player;

        public Collider2D Ground;
        public Collider2D Wall;
        public Collider2D Climb;

        public float GroundOffset;

        private Bounds _groundBounds;

        public GroundedEvent PlayerGrounded;
        public GroundedEvent PlayerNotGrounded;
        
        private bool _wasGroundedLastUpdate;

        public Bounds GroundBounds 
        {
            get
            {
                _groundBounds.center = new(_player.Bounds.center.x, _player.Bounds.center.y + GroundOffset);

                return _groundBounds;
            }
        }

        private void Update()
        {
            // was grounded before, now isn't grounded
            if (this.Ground == null && this._wasGroundedLastUpdate)
            {
                _wasGroundedLastUpdate = false;
                PlayerNotGrounded?.Invoke();
            } 
            
            // wasn't grounded before, is now grounded
            else if (this.Ground && !this._wasGroundedLastUpdate)
            {
                _wasGroundedLastUpdate = true;
                PlayerGrounded?.Invoke();
            }
        }

        void Awake()
        {
            _settings = Resources.Load<GameSettings>("Settings/GameSettings");
            _player = GetComponentInParent<Player>();
        }

        void Start()
        {
            // Ground trigger bounds slightly smaller than player bounds so that they cannot activate trigger while standing next to a wall.
            _groundBounds = new Bounds(Vector3.zero, new Vector2(_player.Bounds.size.x - 0.03f, 0.05f));
        }

        public void ResetTriggers()
        {
            Ground = null;
        }
    }
}