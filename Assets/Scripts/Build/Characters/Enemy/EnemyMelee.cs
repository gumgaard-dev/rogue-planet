using UnityEngine;

namespace Build.Characters.Enemy
{
    public class EnemyMelee : Enemy
    {
        private void Update()
        {
            if (target) Follow();
        }
    }
}