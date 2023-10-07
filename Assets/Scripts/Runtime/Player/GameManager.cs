using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Capstone
{
    public class GameManager : MonoBehaviour
    {

        private InputSystem _inputSystem;
        private Player _player;

        void Awake()
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

        void FixedUpdate()
        {
            _player.FixedUpdateManaged();
        }

        void LateUpdate()
        {
            
        }
    }
}
