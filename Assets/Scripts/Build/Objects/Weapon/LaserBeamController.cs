using Capstone.Build.Characters.Player;
using Capstone.Build.Characters.Player.Animation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Capstone.Build.Weapon
{
    [System.Serializable] public class BeamHitEvent : UnityEvent<Vector3, int> { }
    public class LaserBeamController : MonoBehaviour
    {
        // class constant
        private static Vector2 RELATIVE_BEAM_DIRECTION = Vector2.right;

        public Player p;

        private Transform _playerArmTransform;

        // inspector fields
        [SerializeField] private float _beamMaxDistance = 50f;
        [SerializeField] private LineRenderer _beamLineRenderer;
        [SerializeField] private LayerMask _targetLayers;
        public int DamageAmount = 1;

        [Header("RaycastHit is surface of tile, needs slight offset to hit tile itself.")]
        public float RaycastCollisionCorrectionAmount = 0.2f;

        // event to dispatch information to listeners about hit location
        // used by terrainDestroyer
        public BeamHitEvent BeamHitEvent;

        public PlayerAimController PlayerAimController;

        // cache current hit in world space
        private Vector2 _currentHit;

        // caching start and end variables in local space
        private Vector2 _beamStart;
        private Vector2 _beamEnd;
        private Vector2 _beamDirection;
        
        private void Awake()
        {
            p = GameObject.Find("Player").GetComponent<Player>();
            _playerArmTransform = GetComponentInParent<Transform>();
            if (_beamLineRenderer == null)
            {
                if (!TryGetComponent(out _beamLineRenderer))
                {
                    Debug.LogWarning("No beam attached.");
                }
            }
        }


        private void FixedUpdate()
        {
            if (_beamLineRenderer && _beamLineRenderer.enabled)
            {
                CasRaytAndCheckHits();

                // will be vector2.zero if nothing is hit
                if (_currentHit != Vector2.zero)
                {
                    // convert to local space for line renderer
                    _beamEnd = this.transform.InverseTransformPoint(_currentHit);
                }
                else
                {
                    _beamEnd = RELATIVE_BEAM_DIRECTION * _beamMaxDistance;
                }
                
                 SetBeamEndPosition(_beamEnd);
            }
        }


        // updates _currentHit member variable
        // if there's a hit, invokes the beam hit event with the world coordinates of the hit
        public void CasRaytAndCheckHits()
        {
            if (_beamLineRenderer == null) { return; }

            _beamStart = this.transform.position;

            _beamDirection = RotationToDirectionVector2(_playerArmTransform.eulerAngles.z);

            Debug.Log(this._beamDirection + " " +  _beamDirection);

            RaycastHit2D hit = Physics2D.Raycast(_beamStart, this._beamDirection, _beamMaxDistance, _targetLayers);

            if (hit.collider != null)
            {
                // get collision point
                _currentHit = hit.point;

                // see this function's description below
                CorrectPositionAndInvokeHitEvent();
            } else
            {
                _currentHit = Vector2.zero;
            }

            
        }

        public static Vector2 RotationToDirectionVector2(float rotationAngleDegrees)
        {
            float angleInRadians = rotationAngleDegrees * Mathf.Deg2Rad;
            float x = Mathf.Cos(angleInRadians);
            float y = Mathf.Sin(angleInRadians);

            return new Vector2(x, y);
        }

        public void StartFiring()
        {
            this._beamLineRenderer.enabled = true;
        }

        public void StopFiring()
        {
            this._beamLineRenderer.enabled = false;
        }

        public void SetBeamEndPosition(Vector2 position)
        {
            _beamLineRenderer.SetPosition(1, position);
        }


        // The raycast only returns the coordinate of the surface it hits
        // because of the way tilemaps work, we need to add a slight correction
        // to make sure the position sent in the event is actually inside of the target tile
        private void CorrectPositionAndInvokeHitEvent()
        {
            Vector2 normalizedDirection = _beamDirection.normalized;

            float hitPointXCorrection = normalizedDirection.x * RaycastCollisionCorrectionAmount;
            float hitPointYCorrection = normalizedDirection.y * RaycastCollisionCorrectionAmount;

            Vector3 correctedHitPosition = new(_currentHit.x + hitPointXCorrection, _currentHit.y + hitPointYCorrection, 0);

            BeamHitEvent?.Invoke(correctedHitPosition, DamageAmount);
        }

        private void OnDrawGizmos()
        {
            if (_currentHit == Vector2.zero) { return; }
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_currentHit, 0.1f);
        }


    }
}
