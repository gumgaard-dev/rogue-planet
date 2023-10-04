using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    int terrainLayerMask;

    private void Start()
    {
        terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, terrainLayerMask);

            if (hit.collider != null)
            {
                Debug.Log("hit");
                if (hit.collider.gameObject.CompareTag("Destructible"))
                {
                    hit.collider.gameObject.GetComponent<DestructibleTerrain>().Hit();
                }
            }
        }
    }
}
