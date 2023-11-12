using Build.World.WorldTime;
using UnityEngine;

namespace Build.Characters.Enemy
{
    public class EnemySpawner : MonoBehaviour, IDayNightBehavior, ITimeBasedBehavior
    {
        public Enemy Prototype;
        public Clock Clock;
        public GameObject EnemyTarget;
        private bool _isActive;
        private float _timer = 0;
        [SerializeField] private float spawnRate = 10f;

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

            Prototype.target = EnemyTarget;
        }


        //called from update, spawn the provided prototype
        void Spawn()
        {
            //create new Enemy
            Enemy enemy = Instantiate(Prototype, transform.position, Quaternion.identity);
            enemy.target = EnemyTarget;
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

            if (_timer > spawnRate)
            {
                Spawn();
                _timer = 0;
            }
        }
    }
}