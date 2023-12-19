using Capstone.Build.Characters.Player;
using Capstone.Input;
using UnityEngine;


namespace Capstone.Build
{
    public class GameManager : MonoBehaviour
    {

        private InputSystem _inputSystem;

        [SerializeField]
        private Player _player;

        public int TargetFramerate = 60;
        public static bool GamePaused => Time.timeScale == 0;

        void Awake()
        {
            Application.targetFrameRate = TargetFramerate;
            _inputSystem = GetComponent<InputSystem>();
            _inputSystem.AwakeManaged();

            _player.AwakeManaged();
        }

        void Start()
        {
            _player.StartManaged();
        }


        void Update()
        {
            _inputSystem.UpdateManaged();
            _player.UpdateManaged();
        }

        void FixedUpdate()
        {
            _player.FixedUpdateManaged();
        }

        void LateUpdate()
        {
            
        }

        public static void PauseGame()
        {
            Time.timeScale = 0;
        }

        public static void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}
