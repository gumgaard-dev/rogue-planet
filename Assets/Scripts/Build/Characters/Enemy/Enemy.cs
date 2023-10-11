using Build.Component;
using UnityEngine;

namespace Build.Characters.Enemy
{
    public class Enemy : MonoBehaviour
    {
        public GameObject target;

        [SerializeField] private float speed = 1.5f;
        private HealthData _healthData;
        private int _attackPower;

        public Enemy(Enemy prototype)
        {
            speed = prototype.speed;
            _healthData = prototype._healthData;
            _attackPower = prototype._attackPower;
        }
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            Follow();
        }

        //very basic pathing (for now) that will just draw the enemy towards the target in all directions
        private void Follow()
        {
            transform.position =
                Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }

        
        private void OnTriggerEnter2D(Collider2D col)
        {
            //ignore collisions with non-player objects
            if (!col.CompareTag("Player")) return;
            
            var targetHealth = col.GetComponent<HealthData>();

            if (targetHealth != null)
            {
                //apply damage to target
                targetHealth.Damage(_attackPower);  
            }
        }
    }
}

