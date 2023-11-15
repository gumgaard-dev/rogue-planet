using Capstone.Build.Characters.Player;
using Capstone.Build.Characters.Player.PlayerStates;
using Capstone.Build.Characters.Ship;
using UnityEngine;

namespace Capstone.Build.Cam
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent (typeof(CamColliderController))]
    public class CamController : MonoBehaviour
    {
        public enum CameraStateType
        {
            Mining,
            Combat
        };

        [Header("Testing/Debugging")]
        [SerializeField] private bool _tweakStatesAtRuntime;

        [Space]
        [Header("Mining Camera Options")]
        public Player Player;
        public float MiningCamSize;
        public float MiningLookaheadAmount;
        public float MiningMoveSmoothTime;
        public float MiningZoomSmoothTime;

        [Space]
        [Header("Combat Camera Options")]
        public Ship Ship;
        public float CombatCamSize;
        public float CombatZoomSmoothTime;
        public float CombatMoveSmoothTime;
        public float CombatVerticalOffset;

        // states
        private MiningCamState _miningCameraState;
        private CombatCamState _combatCameraState;
        private BaseCamState _currentCamState;

        // component references
        private Camera _cam;
        private CamColliderController _camColliderController;

        public BaseCamState CurrentCamState
        {
            get { return _currentCamState; }
            set
            {
                _currentCamState = value;
                if  (_camColliderController != null )
                {
                    _camColliderController.UpdateColliderBounds();
                }
            }
        }
        

        void Awake()
        {
            if (!TryGetComponent(out _cam))
            {
                Debug.LogWarning("CamController: no Camera component attached");
            }

            if (!TryGetComponent(out _camColliderController))
            {
                Debug.LogWarning("CamController: no CamColliderController component attached");
            }

            _miningCameraState = new MiningCamState(Player, _cam, MiningCamSize);
            _combatCameraState = new CombatCamState(Ship, _cam, CombatCamSize);

            CurrentCamState = Player.StateType == PlayerStateType.InShip ? _combatCameraState : _miningCameraState;
        }

        void Update()
        {
            if (_tweakStatesAtRuntime) { UpdateStateVariables(); }
            CurrentCamState.UpdateCachedVariables();
            CurrentCamState.MoveCamera();
        }

        private void UpdateStateVariables()
        {
            _miningCameraState.StateCamSize = MiningCamSize;
            _miningCameraState.ZoomSmoothTime = MiningZoomSmoothTime;
            _miningCameraState.MoveSmoothTime = MiningMoveSmoothTime;
            _miningCameraState.CamOffset.x = MiningLookaheadAmount;

            _combatCameraState.StateCamSize = CombatCamSize;
            _combatCameraState.ZoomSmoothTime = CombatZoomSmoothTime;
            _combatCameraState.MoveSmoothTime = CombatMoveSmoothTime;
            _combatCameraState.CamOffset.y = CombatVerticalOffset;
        }

        // set as listener for Player's EnterShip event
        public void OnPlayerEnterShip()
        {
            this.CurrentCamState = _combatCameraState;
        }

        // set as listener for Player's ExitShip event
        public void OnPlayerExitShip()
        {
            this.CurrentCamState = _miningCameraState;
        }
    }
}

