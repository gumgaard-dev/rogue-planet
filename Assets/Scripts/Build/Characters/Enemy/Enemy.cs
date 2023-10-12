using Build.Component;
using UnityEngine;

namespace Build.Characters.Enemy
{
    [RequireComponent(typeof(AttackData))]
    [RequireComponent(typeof(HealthData))]
    public class Enemy : MonoBehaviour
    {
        public GameObject target;

        [SerializeField] private float speed;
        private HealthData _healthData;
        private AttackData _attackData;

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            this._attackData = GetComponent<AttackData>();
            this._healthData = GetComponent<HealthData>();
        }

        void Update()
        {
            if (target != null) Follow();
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
                if (_attackData.Cooldown.IsAvailable())
                {
                    //apply damage to target
                    targetHealth.Damage(_attackData.AttackPower);  
                    //reset cooldown
                    _attackData.Cooldown.Activate();
                }
            }
        }
    }
}

