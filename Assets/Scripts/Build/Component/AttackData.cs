using UnityEngine;

namespace Build.Component
{
    public class AttackData : MonoBehaviour
    {
        [SerializeField] private int attackPower;

        public int AttackPower => attackPower;
    }
}