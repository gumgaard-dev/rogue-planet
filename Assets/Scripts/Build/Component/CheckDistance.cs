using UnityEngine;

namespace Build.Component
{
    public class CheckDistance : MonoBehaviour
    {
        public GameObject self; // the gameobject this is attached to
        public GameObject target; // the object to check against
        public float distanceBetweenObjects;
        
        private void Update()
        {
            if (!self || !target) return;
            
            Vector3 delta = target.transform.position - self.transform.position;
            distanceBetweenObjects = delta.magnitude;
        }
    }
}