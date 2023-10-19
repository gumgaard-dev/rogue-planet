namespace Build.Characters.Enemy
{
    public class EnemyRanged : Enemy
    {
        private bool _withinRange;
        private void Update()
        {
            GetWithinRange();
            if (_withinRange) Attack();
        }

        private void GetWithinRange()
        {
            //move in direction of target until their attack range can reach the target
        }

        private void Attack()
        {
            //shoot held weapon at player
        }
    }
}