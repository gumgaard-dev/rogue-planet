using Build.Component;
using Capstone.Build.Weapon;
using UnityEngine;


namespace Capstone.Build.Characters.Ship
{
    [RequireComponent(typeof(HealthData))]
    public class Ship : MonoBehaviour
    {
        public Gun Gun;
        public HingeRotator Arm;

        private const float ROTATION_INPUT_MOD = -1f;

        // should be controlled by a player only, and should not look for input directly
        public void HandleRotationInput(float direction)
        {
            if (Arm & direction != 0f)
            {
                Arm.Rotate(direction * ROTATION_INPUT_MOD);
            }
        }

        public void HandleShootInput(bool input)
        {
            if (Gun != null && input)
            {
                Gun.Shoot();
            }
        }

        public void OnHealthIsZero()
        {
            Destroy(this.gameObject);
        }
    }
}

