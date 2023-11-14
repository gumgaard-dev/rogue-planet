using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.Events;

namespace Build.Component
{
    [System.Serializable] public class HealthChangedEvent : UnityEvent<int> { }
    public class HealthData : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;

        //public getter properties
        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                MaxHealthChanged?.Invoke(value);
            }
        }
        
        public int CurrentHealth 
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;
                CurrentHealthChanged?.Invoke(value);
            }
        }

        public UnityEvent HealthIsZero;
        public HealthChangedEvent MaxHealthChanged = new();
        public HealthChangedEvent CurrentHealthChanged = new();
        
        void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void Damage(int damageAmount)
        {
            _currentHealth -= damageAmount;
            
            if (_currentHealth <= 0)
            {
                CurrentHealthChanged?.Invoke(0);
                HealthIsZero?.Invoke();
            } else
            {
                CurrentHealthChanged?.Invoke(CurrentHealth);
            }
        }

        public void ResetCurrentHealth()
        {
            CurrentHealth = MaxHealth;
        }
    }
}