using Build.World.WorldTime;
using Capstone.Build.Objects.ObjectPool;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Build.Characters.Enemy
{
    public class EnemySpawner : PoolUser, IDayNightBehavior, ITimeBasedBehavior
    {

        private float[] _pathYValues = { 10.1f, 10.5f, 10.9f, 11.3f, 11.7f };
        private int _pathIndex = 0; // Index to keep track of the current position in the array

        private float enemyHeight;
        public Clock Clock;

        [SerializeField] private bool _isActive;
        [SerializeField] private float _timeBetweenGroupSpawns = 10f;
        [SerializeField] private float _timeBetweenIndividualSpawns = 0.5f;

        // timer is used for both group and individual spawns
        // if _enemiesToSpawn > 0, reset every time it reaches _timeBetweenIndividualSpawns
        // else reset when it reaches _timeBetweenGroupSpawns
        [SerializeField] private float _timer = 0;

        public int MaxGroupSize;
        public int MinGroupSize;

        // set to a random number between min and max group size when the timer reaches timeBetweenGroupSpawns
        // used in the OnTimeChanged method to determine if an enemy should be spawned every _timeBetweenIndividualSpawns seconds
        // decrememented when an enemy is spawned
        private int _enemiesToSpawn;

        void Start()
        {

            if (Clock == null)
            {
                Clock = FindObjectOfType<Clock>();
            }

            if (Clock == null)
            {
                Debug.LogWarning("No clock found");
            }
            else
            {
                Clock.DayStart.AddListener(this.OnDayStart);
                Clock.NightStart.AddListener(this.OnNightStart);
            }

            if (PoolablePrototype == null)
            {
                Debug.LogWarning("No enemy prototype set for this spawner");
            }
            else
            {
                CreatePool(MaxGroupSize);
                if (PoolablePrototype.gameObject.TryGetComponent<CircleCollider2D>(out CircleCollider2D circ))
                {
                    enemyHeight = circ.radius;
                } else if (PoolablePrototype.gameObject.TryGetComponent(out BoxCollider2D box))
                {
                    enemyHeight = box.size.y;
                }
            }
        }


        //called from update, spawn the provided prototype
        void SetCurrentGroupSize()
        {
            _enemiesToSpawn = Random.Range(MinGroupSize, MaxGroupSize);
        }

        public void OnDayStart()
        {
            Deactivate();
        }

        private void Activate()
        {
            if (!_isActive) {
                _isActive = true;
                Clock.TimeOfDayChanged.AddListener(this.OnTimeChanged);
            }
           
        }

        private void Deactivate()
        {
            if (this._isActive)
            {
                _isActive = false;
                Clock.TimeOfDayChanged.RemoveListener(this.OnTimeChanged);
            }
        }

        public void OnNightStart()
        {
            ShuffleArray(_pathYValues);
            SetCurrentGroupSize();
            Activate();
        }

        public void OnTimeChanged(float timeChange)
        {
            // use the _spawnrate and Time.deltaTime to spawn the enemies
            _timer += Time.deltaTime;

            if (_enemiesToSpawn > 0 && _timer > _timeBetweenIndividualSpawns)
            {
                float yPositionOfEnemy = NextRandomPathYPosition() + enemyHeight / 2;
                InstantiateFromPoolAt(new Vector2(this.transform.position.x, yPositionOfEnemy));
                _enemiesToSpawn--;
                _timer = 0;
            }

            if (_timer > _timeBetweenGroupSpawns)
            {
                SetCurrentGroupSize();
                _timer = 0;
            }
        }

        private float NextRandomPathYPosition()
        {
            if (_pathIndex >= _pathYValues.Length)
            {
                ShuffleArray(_pathYValues); // Shuffle the array
                _pathIndex = 0; // Reset the index
            }

            // Return the current value and increment the index
            return _pathYValues[_pathIndex++];
        }

        private void ShuffleArray(float[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                float temp = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
        }
    }
}