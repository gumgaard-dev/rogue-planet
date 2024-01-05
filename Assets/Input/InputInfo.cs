using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Input
{
    public class InputInfo : MonoBehaviour
    {
        public static Vector2 Move;
        public static Vector2 PlayerAim;
        public static bool JumpHeld;
        public static bool EnterShip;
        public static bool FireLaser;
               
        public static bool ShootHeld;
        public static bool ExitShip;
        public static float ShipAim;
        public static bool PrecisionAim;

        // UI control modes
        public static bool OpenUpgradeMenu;

        public static bool PlaceDeployable;

        public static InputInfo Instance;

        public static object ShootPressed { get; internal set; }
        public static object JumpPressed { get; internal set; }
    }
}
