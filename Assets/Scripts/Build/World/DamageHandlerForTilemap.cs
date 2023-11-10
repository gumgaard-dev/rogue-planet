using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    [System.Serializable] public class TileDestroyedEvent : UnityEvent<Vector3Int> { }
    [System.Serializable] public class OreDestroyedEvent : UnityEvent<Vector3Int> { }
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

        // Reference to the MapGenerator to access the terrainTileDataMap
        public MapGenerator MapGenerator;

        private void Awake()
        {
            if (TerrainTilemap == null)
            {
                Debug.Log(gameObject.name + "->DestructibleTilemap: no terrain map set in inspector.");
            }
        }

        public void HandleDamage(Vector3 worldPosition, int damageAmount)
        {
            Vector3Int tilePos = TerrainTilemap.WorldToCell(worldPosition);
            HandleDamage(tilePos, damageAmount);
        }

        public void HandleDamage(Vector3Int position, int damageAmount)
        {
            if (MapGenerator == null)
            {
                Debug.LogError("MapGenerator reference is not set.");
                return;
            }

            if (!MapGenerator.TerrainTileDataMap.TryGetValue(position, out var tileData))
            {
                Debug.LogWarning("No tile data found for the position: " + position);
                return;
            }

            // Handle damage
            tileData.CurrentHealth -= damageAmount;
            Debug.Log(tileData.CurrentHealth);

            if (tileData.CurrentHealth <= 0)
            {
                // Check for ore and trigger events
                OreTile ore = OreTilemap.GetTile(position) as OreTile;
                if (ore != null)
                {
                    Vector3 oreWorldPosition = TerrainTilemap.GetCellCenterWorld(position);
                    TileWithItemDestroyed?.Invoke(ore.OreToDrop, oreWorldPosition);
                    OreDestroyed?.Invoke(position);

                    OreTilemap.SetTile(position, null);
                    OreTilemap.RefreshTile(position);
                }

                TerrainDestroyed?.Invoke(position);
                TerrainTilemap.SetTile(position, null);
                TerrainTilemap.RefreshTile(position);

                // Remove the tile data from the dictionary
                MapGenerator.TerrainTileDataMap.Remove(position);
            }
        }
    }
}