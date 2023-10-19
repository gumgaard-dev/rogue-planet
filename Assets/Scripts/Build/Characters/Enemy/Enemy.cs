using Build.Component;
using UnityEngine;

namespace Build.Characters.Enemy
{
    [RequireComponent(typeof(AttackData))]
    [RequireComponent(typeof(HealthData))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        public GameObject target;

        [SerializeField] protected float speed;
        private HealthData _healthData;
        private AttackData _attackData;
        
        //TODO make hitbox that get attached as child element which will act as the trigger
        private Collider2D _trigger;
        private Collider2D _collider;

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            _attackData = GetComponent<AttackData>();
            _healthData = GetComponent<HealthData>();
        }
        
        //at this point, any enemy will deal damage if the player touches it
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

