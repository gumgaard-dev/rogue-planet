using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Capstone.Input
{
    public class InputInfo : MonoBehaviour
    {
        // all control modes
        public Vector2 Move;
        public Vector2 AimMining;
        public bool Jump;
        public bool Shoot;
        public bool EnterShip;
        public bool ExitShip;

        public float AimShip;
        public bool PreceisionAim;
    }
}
