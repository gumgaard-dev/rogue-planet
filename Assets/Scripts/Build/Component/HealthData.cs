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
                // clamp between zero and max health to prevent overheal/overkill
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);

                // notify listeners
                CurrentHealthChanged?.Invoke(value);
            }
        }

        public UnityEvent HealthIsZero = new();
        public HealthChangedEvent MaxHealthChanged = new();
        public HealthChangedEvent CurrentHealthChanged = new();
        
        void Start()
        {
            MaxHealthChanged?.Invoke(MaxHealth);
            CurrentHealth = MaxHealth;
        }

        public void Damage(int damageAmount)
        {
            CurrentHealth -= damageAmount;
            if (CurrentHealth == 0)
            {
                    HealthIsZero?.Invoke();
            }
        }

        public void HealToFull()
        {
            CurrentHealth = MaxHealth;
        }
        public void IncreaseMaxHealthBy(int amount)
        {
            MaxHealth += amount;
            CurrentHealth += amount;
        }

        public void HealBy(int amount)
        {
            CurrentHealth += amount;
        }
    }
}