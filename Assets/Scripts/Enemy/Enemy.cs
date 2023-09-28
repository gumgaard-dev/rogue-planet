namespace Enemy
{
    using UnityEngine;

    public class Enemy : MonoBehaviour
    {
        private GameObject _target;

        private float _speed;
        private int _health;
        private int _damage;

        void Start()
        {
            //TODO set _target here
        }

        void Update()
        {
            Follow();
        }

        //very basic pathing (for now) that will just draw the enemy towards the target in all directions
        private void Follow()
        {
            transform.position =
                Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        }

        //TODO this requires a player object in the scene
        private void OnTriggerEnter2D(Collider2D col)
        {
            //if collider is a player and they have health/it isn't null, damage them
        }
    }
}

