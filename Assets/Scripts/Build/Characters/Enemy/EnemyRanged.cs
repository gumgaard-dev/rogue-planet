using Build.Component;
using UnityEngine;

namespace Build.Characters.Enemy
{
    [RequireComponent(typeof(CheckDistance))]
    public class EnemyRanged : Enemy
    {
        private CheckDistance _check;
        public float minDistanceToTarget;
        private bool _withinRange;
        
        new void Start()
        {
            base.Start();
            _check = GetComponent<CheckDistance>();
            _check.target = target;
        }
        
        private void Update()
        {
            _withinRange = _check.distanceBetweenObjects <= minDistanceToTarget;
            
            if (!_withinRange && target)
            {
                Follow();
            }
            
            Attack();
        }

        private void Attack()
        {
            //shoot held weapon at player
        }
    }
}