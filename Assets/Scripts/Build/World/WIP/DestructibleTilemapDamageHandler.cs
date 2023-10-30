using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TerrainUtils;
using UnityEngine.Tilemaps;

namespace Capstone.WIP
{
    [System.Serializable] public class TileDestroyedEvent : UnityEvent<TerrainTile, Vector3> { }
    [System.Serializable] public class OreDestroyedEvent : UnityEvent<OreCollectable, Vector3> { }
    public class DestructibleTilemapDamageHandler : MonoBehaviour
    {
        public Tilemap TerrainMap;
        public Tilemap Oremap;
        public TileDestroyedEvent TileDestroyed;
        public OreDestroyedEvent OreDestroyed;

        private void Awake()
        {
            if (TerrainMap == null)
            {
                Debug.Log(gameObject.name + "->DestructibleTilemap: no terrain map set in inspector.");
            }
        }

        public void HandleDamage(Vector3Int position, int damageAmount)
        {
            // Get the tile at the given position
            TerrainTile currentTile = TerrainMap.GetTile(position) as TerrainTile;

            if (currentTile != null)
            {
                currentTile.Damage(damageAmount); // apply damage

                // check if tile health is at or below zero after damage calculations
                if (currentTile.ShouldBeDestroyed())
                {
                    TileDestroyed?.Invoke(currentTile, position);// notify listeners
                                                                 // If the tile contains ore, trigger the OreDestroyedEvent
                    if (currentTile.OreTile != null)
                    {
                        //notify any listeners that this ore type was destroyed
                        OreDestroyed?.Invoke(currentTile.OreTile.OreToDrop, TerrainMap.GetCellCenterWorld(position));
                    }
                    TerrainMap.SetTile(position, null); // Remove the tile from the map
                }
            }
        }
    }
}
