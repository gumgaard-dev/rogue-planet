using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Build
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Gravity")]
        public float DefaultGravityScale = 3.0f;
        public float FallingGravityScale = 8.5f;

        [Header("Run Speed")]
        public float RunSpeed = 10f;
        public float MinMoveSpeed = 0.01f;
        public float MinRunSpeed = 1.2f;

        [Header("Jump force")]
        public float MinJumpSpeed = 0.01f;
        public float MinFallSpeed = 0.01f;

        [Header("Movement Smoothing")]
        public float GroundSpeedSmoothTime = 0.08f;
        public float AirSpeedSmoothTime = 0.27f;

        [Header("Jetpack")]
        public int DefaultJetpackMaxFuel = 125;
        public int DefaultJetpackRechargeRate = 4;
        public int DefaultJetpackRechargeDelay = 5;
        public int DefaultJetpackConsumptionRate = 2;
        public float JetpackSpeed = 5f;
        public float JetpackBoostSpeed = 10f;
    }
}
