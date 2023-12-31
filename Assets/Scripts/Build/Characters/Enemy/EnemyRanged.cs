using Build.Component;
using Capstone.Build.Weapon;
using UnityEngine;

namespace Build.Characters.Enemy
{
    [RequireComponent(typeof(CheckDistance))]
    public class EnemyRanged : Enemy
    {
        public Gun Gun;
        public HingeRotator Hinge;
        
        private CheckDistance _check;
        public float minDistanceToTarget;
        private bool _withinRange;
        
        new void Start()
        {
            base.Start();
            _check = GetComponent<CheckDistance>();
            _check.target = target;
            _check.self = gameObject;
        }
        
        private void Update()
        {
            _withinRange = _check.distanceBetweenObjects <= minDistanceToTarget;
            
            if (!_withinRange && target)
            {
                Follow();
                UpdateAimDirection();
            }

            if (Gun && target && _withinRange)
            {
                
                Gun.Shoot();
            }
        }

        private void UpdateAimDirection()
        {
            Hinge.RotateTo(target.transform.position);
        }
    }
}