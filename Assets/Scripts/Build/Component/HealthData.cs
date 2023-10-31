using UnityEditor.Networking.PlayerConnection;
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

            if (_currentHealth <= 0)
            {     
                Destroy(gameObject);
            }
        }
    }
}