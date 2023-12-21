using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Input
{
    public class InputInfo : MonoBehaviour
    {
        public Vector2 Move;
        public Vector2 AimMining;
        public bool Jump;
        public bool EnterShip;

        public bool Shoot;
        public bool ExitShip;
        public float AimShip;
        public bool PrecisionAim;

        // UI control modes
        public bool OpenUpgradeMenu;
        public bool Back;
        public bool Confirm;
        public Vector2 MoveCursor;

        public bool PlaceDeployable { get; internal set; }
    }
}
