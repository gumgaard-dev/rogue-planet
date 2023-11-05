using Capstone.Build.Characters.Player;
using Capstone.Build.Characters.Player.PlayerStates;
using UnityEngine;

namespace Capstone.Build.Cam
{
    [RequireComponent(typeof(Camera))]
    public class CameraStateManager : MonoBehaviour
    {
        private MiningCameraState _miningCameraState;
        private CombatCameraState _combatCameraState;
        private CameraState _currentState;
        private Camera _camera;

        [Header("select this option to tweak state variables at runtime:")]
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

        private void Awake()
        {
            this._camera = GetComponent<Camera>();
            _miningCameraState = new MiningCameraState(Player, _camera, MiningCameraSize);
            _combatCameraState = new CombatCameraState(Ship, _camera, CombatCameraSize);
            _currentState = (Player.StateType == PlayerStateType.InShip) ? _combatCameraState : _miningCameraState;
        }

        private void Update()
        {
            if (_tweakStatesAtRuntime) { UpdateStateVariables(); }
        }
        void LateUpdate()
        {
            _currentState.UpdateCachedVariables();
            _currentState.MoveCamera();
        }

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


        // set as listener for Player's EnterShip event
        public void OnPlayerEnterShip()
        {
            this._currentState = _combatCameraState;
        }

        // set as listener for Player's ExitShip event
        public void OnPlayerExitShip()
        {
            this._currentState = _miningCameraState;
        }
    }
}

