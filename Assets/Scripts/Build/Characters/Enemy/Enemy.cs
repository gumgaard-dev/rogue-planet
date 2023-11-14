using Build.Component;
using TMPro;
using UnityEngine;

namespace Build.Characters.Enemy
{
    [RequireComponent(typeof(AttackData))]
    [RequireComponent(typeof(HealthData))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : PoolableObject
    {
        public GameObject target;

        [SerializeField] protected float speed;
        private HealthData _healthData;
        private AttackData _attackData;
        
        //TODO make hitbox that get attached as child element which will act as the trigger
        private Collider2D _trigger;
        private Collider2D _collider;

        private int _facingDirection;
        private Rigidbody2D _rb;

        protected void Start()
        {
            target = GameObject.FindGameObjectWithTag("Ship");
            _attackData = GetComponent<AttackData>();
            _healthData = GetComponent<HealthData>();
            _rb = GetComponent<Rigidbody2D>();

            // setting condition under which this should be returned to the pool
            _healthData.HealthIsZero.AddListener(this.ReturnToPool);
        }
        
        //at this point, any enemy will deal damage if the player touches it
        private void OnTriggerEnter2D(Collider2D col)
        {
            //ignore collisions with non-player objects
            if (!col.CompareTag("Ship")) return;
            
            var targetHealth = col.GetComponent<HealthData>();

            if (targetHealth && _attackData.Cooldown.IsAvailable())
            {
                //apply damage to target
                targetHealth.Damage(_attackData.AttackPower);  
                //reset cooldown
                _attackData.Cooldown.Activate();
                
            }
        }
        

        protected void UpdateFacing()
        {
            if (target != null)
            {
                _facingDirection = target.transform.position.x >= this.transform.position.x ? 1 : -1;
            }
        }
        
        protected void UpdateVelocity()
        {
            if (target != null)
            {
                this._rb.velocity = new(speed * _facingDirection, this._rb.velocity.y);
            }
        }

        //very basic pathing (for now) that will just draw the enemy towards the target in all directions
        protected void Follow()
        {
            UpdateFacing();
            UpdateVelocity();
        }

        // method is set as listener to healthData.HealthIsZero event
        public override void ReturnToPool()
        {
            // reset any values here
            _healthData.ResetCurrentHealth();

            // return this to pool
            base.ReturnToPool();
        }
    }
}

