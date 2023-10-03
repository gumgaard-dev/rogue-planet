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

        [Space]
        public float GroundSpeedSmoothTime = 0.08f;
    }
}
