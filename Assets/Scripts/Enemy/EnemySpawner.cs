using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public Enemy prototype;
        float _timer = 0;
        private float _spawnRate = 10f;

        void Start()
        {
            _spawnRate = 4;
            //instantiate enemy at the spawner's location with no rotation
            Instantiate(prototype, transform.position, Quaternion.identity);
        }

        void Update()
        {
            // use the _spawnrate and Time.deltaTime to spawn the enemies
            _timer += Time.deltaTime;

            if (_timer > _spawnRate)
            {
                Spawn();
                _timer = 0;
            }
        }

        //called from update, spawn the provided prototype
        void Spawn()
        {
            //create new Enemy
            Instantiate(prototype, transform.position, Quaternion.identity);
        }
    }
}