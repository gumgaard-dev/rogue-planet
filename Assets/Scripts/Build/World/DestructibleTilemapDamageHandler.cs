using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    [System.Serializable] public class TileDestroyedEvent : UnityEvent<TerrainTile, Vector3> { }
    [System.Serializable] public class OreDestroyedEvent : UnityEvent<OreCollectable, Vector3> { }
    public class DestructibleTilemapDamageHandler : MonoBehaviour
    {
        [Header("Tilemaps")]
        public Tilemap TerrainTilemap;
        public Tilemap OreTilemap;

        [Header("Event Listeners")]
        public TileDestroyedEvent TileDestroyed;
        public OreDestroyedEvent OreDestroyed;

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
            // Get the tile at the given position
            TerrainTile currentTile = TerrainTilemap.GetTile(position) as TerrainTile;

            if (currentTile != null)
            {
                currentTile.Damage(damageAmount); // apply damage
                Debug.Log(currentTile.CurrrentHealth);

                // check if tile health is at or below zero after damage calculations
                if (currentTile.ShouldBeDestroyed())
                {
                    TileDestroyed?.Invoke(currentTile, position);// notify listeners
                    
                    // If the tile contains ore, trigger the OreDestroyedEvent
                    if (currentTile.HasOre)
                    {
                        OreTile oreTile = OreTilemap.GetTile(position) as OreTile;
                        
                        //notify any listeners that this ore type was destroyed
                        OreDestroyed?.Invoke(oreTile.OreToDrop, TerrainTilemap.GetCellCenterWorld(position));

                        OreTilemap.SetTile(position, null);
                        OreTilemap.RefreshTile(position);
                        Destroy(oreTile);                    }
                    TerrainTilemap.SetTile(position, null); // Remove the tile from the map
                    TerrainTilemap.RefreshTile(position);
                    Destroy(currentTile);
                }
            }
        }
    }
}
