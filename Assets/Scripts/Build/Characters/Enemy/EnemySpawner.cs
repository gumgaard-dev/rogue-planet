using Build.World.WorldTime;
using Capstone.Build.Objects.ObjectPool;
using UnityEngine;

namespace Build.Characters.Enemy
{
    public class EnemySpawner : PoolUser, IDayNightBehavior, ITimeBasedBehavior
    {
        public Clock Clock;

        [SerializeField] private bool _isActive;
        [SerializeField] private float _timer = 0;
        [SerializeField] private float _timeBetweenSpawns = 10f;

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
                CreatePool(10);
            }
        }


        //called from update, spawn the provided prototype
        void Spawn()
        {
            GetFromPool();
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
            Spawn();
            Activate();
        }

        public void OnTimeChanged(float timeChange)
        {
            // use the _spawnrate and Time.deltaTime to spawn the enemies
            _timer += Time.deltaTime;

            if (_timer > _timeBetweenSpawns)
            {
                Spawn();
                _timer = 0;
            }
        }
    }
}