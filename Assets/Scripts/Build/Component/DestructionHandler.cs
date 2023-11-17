using UnityEngine;

namespace Capstone.WIP {
    public class DestructionHandler : MonoBehaviour
    {
        private DestructionBehavior[] _destructionBehaviors;
        private DestructionBehavior[] _childDestructionBehaviors;

        [Tooltip("Call destruction behaviors on child objects as well.")]
        public bool HandleDestructionOfChildren = false;

        [Tooltip("If set to true, child behaviors will be handled before parent behaviors. Otherwise, parent behaviors will be handled first")]
        public bool ChildrenFirst = true;

        public void Awake()
        {
            // get destruction behaviors on this object, if any
            _destructionBehaviors = GetComponents<DestructionBehavior>();

            // get destruction behaviors on children, if any
            _childDestructionBehaviors = GetComponentsInChildren<DestructionBehavior>();


            // if not set to handle destruction of children, and no behaviors on this object
            // if this handles children and has no behaviors, no reason to warn user
            if (!HandleDestructionOfChildren && _destructionBehaviors.Length == 0)
            {
                Debug.LogWarning(gameObject.name + "->DestructionHandler: No destruction behaviors available to handler.");
            }

            // if set to handle children, but no children behaviors found
            if (HandleDestructionOfChildren && _childDestructionBehaviors.Length == 0)
            {
                Debug.LogWarning(gameObject.name + "->DestructionHandler: Handler configured to handle children, but no child destruction behaviors were found");
            }


        }
        public void HandleDestructionRoutine()
        {
            if (!HandleDestructionOfChildren)
            {
                ExecuteBehaviors();
            }
            else if (ChildrenFirst)
            {
                ExecuteChildrenBehaviors();
                ExecuteBehaviors();
            }
            else if (!ChildrenFirst)
            {
                ExecuteBehaviors();
                ExecuteChildrenBehaviors();
            }
        }

        private void ExecuteBehaviors()
        {
            // handle behaviors on this object
            if (_destructionBehaviors.Length > 0)
            {
                foreach (var behavior in _destructionBehaviors)
                {
                    if (!behavior.ExecuteManually) behavior.Execute();
                }
            }
        }

        private void ExecuteChildrenBehaviors()
        {
            // handle children behaviors
            if (_childDestructionBehaviors.Length > 0)
            {
                foreach (var behavior in _childDestructionBehaviors)
                {
                    if (!behavior.ExecuteManually) behavior.Execute();
                }
            }
        }
    }
}

