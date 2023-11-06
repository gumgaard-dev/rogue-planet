using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Build.Characters.Player
{
    public class TriggerInfo : MonoBehaviour
    {
        private GameSettings _settings;
        private Player _player;

        public Collider2D Ground;
        public Collider2D Wall;
        public Collider2D Climb;

        public float GroundOffset;

        private Bounds _groundBounds;

        public Bounds GroundBounds 
        {
            get
            {
                _groundBounds.center = new(_player.Bounds.center.x, _player.Bounds.center.y + GroundOffset);

                return _groundBounds;
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