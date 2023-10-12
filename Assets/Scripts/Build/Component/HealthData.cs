using UnityEngine;

namespace Build.Component
{
    public class HealthData : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        private int _currentHealth;
        

        //public getter properties
        public int MaxHealth => maxHealth;
        public int CurrentHealth => _currentHealth;

        void Start()
        {
            _currentHealth = maxHealth;
        }

        public void Damage(int damageAmount)
        {
            _currentHealth -= damageAmount;

            if (_currentHealth < 1)
            {
                //destroy the gameobject this health component is attached to
                //TODO we may want something more robust for the player in the future, but this suffices for enemies/terrain
                Destroy(gameObject);
            }
        }
    }
}