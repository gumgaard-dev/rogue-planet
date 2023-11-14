using Capstone.Build.Characters.Player;
using Capstone.Build.Characters.Player.PlayerStates;
using Capstone.Build.Characters.Ship;
using UnityEngine;

namespace Capstone.Build.Cam
{
    [RequireComponent(typeof(Camera))]
    public class CameraStateManager : MonoBehaviour
    {
        private MiningCameraState _miningCameraState;
        private CombatCameraState _combatCameraState;
        public CameraState CurrentCamState;
        public Camera Cam;

        [Header("Testing/Debugging")]
        [SerializeField] private bool _tweakStatesAtRuntime;

        [Header("Mining Camera Options")]
        public Player Player;
        public float MiningCameraSize;
        public float MiningLookaheadAmount;
        public float MiningMoveSmoothTime;
        public float MiningZoomSmoothTime;

        [Header("Combat Camera Options")]
        public Ship Ship;
        public float CombatCameraSize;
        public float CombatZoomSmoothTime;
        public float CombatMoveSmoothTime;
        public float CombatVerticalOffset;

        [ExecuteInEditMode]
        private void Awake()
        {
            if (Cam == null)
            {
                Cam = GetComponent<Camera>();
            }
            
            _miningCameraState = new MiningCameraState(Player, Cam, MiningCameraSize);
            _combatCameraState = new CombatCameraState(Ship, Cam, CombatCameraSize);
            CurrentCamState = (Player.StateType == PlayerStateType.InShip) ? _combatCameraState : _miningCameraState;
        }

        [ExecuteInEditMode]
        void Update()
        {
            if (_tweakStatesAtRuntime) { UpdateStateVariables(); }
            CurrentCamState.UpdateCachedVariables();
            CurrentCamState.MoveCamera();
        }

        [ExecuteInEditMode]
        private void UpdateStateVariables()
        {
            _miningCameraState.CameraSize = MiningCameraSize;
            _miningCameraState.ZoomSmoothTime = MiningZoomSmoothTime;
            _miningCameraState.MoveSmoothTime = MiningMoveSmoothTime;
            _miningCameraState.LookaheadAmount = MiningLookaheadAmount;

            _combatCameraState.CameraSize = CombatCameraSize;
            _combatCameraState.ZoomSmoothTime = CombatZoomSmoothTime;
            _combatCameraState.MoveSmoothTime = CombatMoveSmoothTime;
            _combatCameraState.VerticalOffset = CombatVerticalOffset;
        }

        [ExecuteInEditMode]
        // set as listener for Player's EnterShip event
        public void OnPlayerEnterShip()
        {
            this.CurrentCamState = _combatCameraState;
        }

        [ExecuteInEditMode]
        // set as listener for Player's ExitShip event
        public void OnPlayerExitShip()
        {
            this.CurrentCamState = _miningCameraState;
        }
    }
}

