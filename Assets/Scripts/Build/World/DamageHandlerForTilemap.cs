using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{

    // events with info about the tilemap coordinate of the destroyed tile
    // listeners must have a reference to the tilemap to use this event effectively
    [System.Serializable] public class TileDestroyedEvent : UnityEvent<Vector3Int> { }
    [System.Serializable] public class OreDestroyedEvent : UnityEvent<Vector3Int> { }
    
    // holds info about the item to spawn and the world position
    [System.Serializable] public class TileWithItemDestroyedEvent : UnityEvent<OreCollectable, Vector3> { }
    public class DamageHandlerForTilemap : MonoBehaviour
    {
        [Header("Tilemaps")]
        public Tilemap TerrainTilemap;
        public Tilemap OreTilemap;

        [Header("Event Listeners")]
        public TileDestroyedEvent TerrainDestroyed;
        public OreDestroyedEvent OreDestroyed;
        public TileWithItemDestroyedEvent TileWithItemDestroyed;

        private void Awake()
        {
            if (TerrainTilemap == null)
            {
                Debug.Log(gameObject.name + "->DestructibleTilemap: no terrain map set in inspector.");
            }
        }
        
        
        // overloaded method to accept world coordinates
        public void HandleDamage(Vector3 worldPosition, int damageAmount)
        {
            Vector3Int tilePos = TerrainTilemap.WorldToCell(worldPosition);
            HandleDamage(tilePos, damageAmount);
        }


        public void HandleDamage(Vector3Int position, int damageAmount)
        {
            // Get the tile at the given position
            TerrainTile currentTile = TerrainTilemap.GetTile(position) as TerrainTile;

            if (currentTile != null && currentTile.Destructible == true)
            {
                currentTile.Damage(damageAmount); // apply damage
                Debug.Log(currentTile.CurrrentHealth);

                // check if tile health is at or below zero after damage calculations
                if (currentTile.ShouldBeDestroyed())
                {
                    // If the tile contains ore, trigger the OreDestroyedEvent
                    if (currentTile.HasOre)
                    {
                        OreTile oreTile = OreTilemap.GetTile(position) as OreTile;

                        
                        Vector3 oreWorldPosition = TerrainTilemap.GetCellCenterWorld(position); // convert to world position
                        TileWithItemDestroyed?.Invoke(oreTile.OreToDrop, oreWorldPosition); // notify oreItemSpawner that an item should be spawned at world position

                        OreDestroyed?.Invoke(position);// notify listeners that an ore block was destroyed at tilemap coords

                        OreTilemap.SetTile(position, null);
                        OreTilemap.RefreshTile(position);
                        Destroy(oreTile);
                    }

                    TerrainDestroyed?.Invoke(position); // notify listeners that a terrain tile was destroyed
                    TerrainTilemap.SetTile(position, null); // Remove the tile from the map
                    TerrainTilemap.RefreshTile(position);
                    Destroy(currentTile);
                }
            }
        }
    }
}
