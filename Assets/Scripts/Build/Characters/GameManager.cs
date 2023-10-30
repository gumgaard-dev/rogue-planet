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

        void Awake()
        {
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
    }
}