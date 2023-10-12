namespace Build.Characters.Enemy
{
    public class EnemyRanged : Enemy
    {
        private void Update()
        {
            GetWithinRange();
            Attack();
        }

        private void GetWithinRange()
        {
            //move in direction of target until their attackrange can rach the target
        }

        private void Attack()
        {
            //shoot weapon at player
        }
    }
}