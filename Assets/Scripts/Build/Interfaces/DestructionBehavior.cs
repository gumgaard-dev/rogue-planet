using UnityEngine;
namespace Capstone.WIP {
    public class DestructionBehavior : MonoBehaviour
    {
        [Tooltip("Set to true to make any DestructionHandlers on this or parent will ignore this behavior")]
        public bool ExecuteManually = false;

        public void Awake()
        {
            // Handler can be attached to same object, or to parent and set to "HandleChildrenDestructionBehaviors"
            if (!TryGetComponent(out DestructionHandler _))
            {
                DestructionHandler parentHandler = GetComponentInParent<DestructionHandler>();

                // check if parent has handler
                if (parentHandler == null)
                {
                    Debug.LogWarning(gameObject.name + ": has DestructionBehavior but no accessible DestructionHandler.");
                }

                // if parent has handler but isn't set up to handle children
                else if (parentHandler.HandleDestructionOfChildren == false)
                {
                    Debug.LogWarning(gameObject.name + ": has DestructionBehavior but no accessible DestructionHandler. DestructionHandler exists in parent but is not set to handle children.");
                }
            }
        }

        // DestructionBehavior can be executed manually but should normally only be executed by a DestructionHandler
        public virtual void Execute()
        {

        }
    }
}
