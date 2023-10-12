using UnityEngine;

namespace Build.Component
{
    public class AttackData : MonoBehaviour
    {
        [SerializeField] private int attackPower;
        [SerializeField] private int cooldownDuration;
        private Cooldown _cooldown;
        
        public int AttackPower => attackPower;
        public Cooldown Cooldown => _cooldown;

        private void Start()
        {
            _cooldown = new Cooldown(cooldownDuration);
        }
    }
}