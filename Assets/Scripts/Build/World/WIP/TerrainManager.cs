using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;  
namespace Capstone.WIP
{

    [System.Serializable] public class TerrainHitEvent : UnityEvent<Vector3, int> { }
    public class TerrainManager : MonoBehaviour
    {
        int terrainLayerMask;
        static int MAX_DAMAGE = 100;
        public TerrainHitEvent TerrainHit;
        private void Start()
        {
            terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");
        }

        void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click");
                Vector3 mousePosition = UnityEngine.Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, terrainLayerMask);

                if (hit.collider != null)
                {
                    Debug.Log("hit");
                    Vector3 hitWorldPosition = hit.point; // getting the world position of the hit
                    TerrainHit?.Invoke(hitWorldPosition, MAX_DAMAGE);
                }
            }
        }
    }
}

