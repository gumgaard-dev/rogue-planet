using UnityEngine;
using Capstone.Input; // Import namespace to reference 
using Capstone.Build.Characters.Player;
using UnityEngine.Events;

namespace Capstone.Build.Characters.Player.Animation
{ 
    public class PlayerAimController : MonoBehaviour
    {
        private Player _player;
        public Transform bone; // Assign the bone you want to rotate in the inspector
        public float rotationSpeed = 5f; // Adjust rotation speed as needed
        public Vector2 AimDirection;
        public bool IsAiming;

        public UnityEvent StartedAiming;
        public UnityEvent StoppedAiming;
        public float AimXDirection { get; private set; }

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            // Retrieve the aim direction from the input system
            AimDirection = InputInfo.PlayerAim;

            AimXDirection = AimDirection.normalized.x;
            
            if (AimDirection.sqrMagnitude > 0.01f)
            {
                if (!IsAiming)
                {
                    StartedAiming?.Invoke();
                }
                IsAiming = true;
                RotateBoneTowardsDirection(new Vector2(AimDirection.x, AimDirection.y));
            }
            else
            {
                if (IsAiming)
                {
                    StoppedAiming?.Invoke();
                }
                IsAiming = false;
                RotateBoneTowardsDirection(new Vector2(_player.Facing, 0)); // put arm down if not aiming
            }
        }

        void RotateBoneTowardsDirection(Vector2 direction)
        {
            // Calculate the angle from the aim direction
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Create a target rotation based on the calculated angle
            var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Apply the rotation to the bone
            bone.rotation = targetRotation;
        }
    }


}
