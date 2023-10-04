using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public float DefaultGravityScale = 6.0f;
        public float FallingGravityScale = 8.5f;

        [Space]
        public float RunSpeed = 10f;
        public float MinMoveSpeed = 0.01f;
        public float MinRunSpeed = 1.2f;
        public float JumpSpeed = 22f;
        public float MinJumpSpeed = 0.01f;
        public float MinFallSpeed = 0.01f;

        [Space]
        public float GroundSpeedSmoothTime = 0.08f;
        public float AirSpeedSmoothTime = 0.27f;
    }
}
