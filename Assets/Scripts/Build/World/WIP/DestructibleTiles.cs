using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Capstone.World.WIP
{
    public class DestructibleTiles : MonoBehaviour
    {

        private Tilemap _tilemap;
        // Start is called before the first frame update
        void Start()
        {
            if (!TryGetComponent(out _tilemap))
            {
                Debug.Log("Destructible tilemap has no tilemap component");
            }
        }

        void HitAt(Vector2 position)
        {
            if (true)
            {
                TileBase t = _tilemap.GetTile(_tilemap.WorldToCell(position));

                //todo:
            }
        }
    }
}

