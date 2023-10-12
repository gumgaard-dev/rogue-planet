using Build.Component;
using UnityEngine;

namespace Build.Characters.Enemy
{
    [RequireComponent(typeof(AttackData))]
    [RequireComponent(typeof(HealthData))]
    public class Enemy : MonoBehaviour
    {
        public GameObject target;

        [SerializeField] protected float speed;
        private HealthData _healthData;
        private AttackData _attackData;

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            this._attackData = GetComponent<AttackData>();
            this._healthData = GetComponent<HealthData>();
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

