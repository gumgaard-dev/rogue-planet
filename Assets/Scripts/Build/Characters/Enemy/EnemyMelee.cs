using UnityEngine;

namespace Build.Characters.Enemy
{
    public class EnemyMelee : Enemy
    {
        private void Update()
        {
            Follow();
        }
        
        //very basic pathing (for now) that will just draw the enemy towards the target in all directions
        protected void Follow()
        {
            transform.position =
                Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        
    }
}