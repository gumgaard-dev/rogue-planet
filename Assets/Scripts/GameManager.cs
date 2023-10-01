using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Capstone
{
    public class GameManager : MonoBehaviour
    {

        private Player _player;
        private InputSystem _inputSystem;

        private void Awake()
        {
            _inputSystem = GetComponent<InputSystem>();
            _inputSystem.AwakeManaged();

            _player = GameObject.Find("Player").GetComponent<Player>();
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

        private void FixedUpdate()
        {
            _player.FixedUpdateManaged();
        }

        private void LateUpdate()
        {
            
        }
    }
}
