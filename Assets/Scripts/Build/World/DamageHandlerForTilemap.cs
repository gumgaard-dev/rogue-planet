using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Capstone.Build.World
{
    [System.Serializable] public class TerrainDestroyedEvent : UnityEvent<Vector3Int> { }

    public class DamageHandlerForTilemap : MonoBehaviour
    {

        [Header("Event Listeners")]
        public TerrainDestroyedEvent TerrainDestroyed;

        // Reference to the MapGenerator to access the terrainTileDataMap
        public MapGenerator MapGenerator;

        private void Awake()
        {
            if (MapGenerator == null)
            {
                Debug.LogError("MapGenerator reference is not set.");
                return;
            }
        }

        // overloaded in case world position is needed
        public void HandleDamage(Vector3 worldPosition, int damageAmount)
        {
            Vector3Int tilePos = MapGenerator.GetTerrainAtWorldPosition(worldPosition);
            HandleDamage(tilePos, damageAmount);
        }

        public void HandleDamage(Vector3Int position, int damageAmount)
        {
            if (MapGenerator == null)
            {
                Debug.LogError("MapGenerator reference is not set.");
                return;
            }

            MapTileData tileData = MapGenerator.GetTileDataAt(position);
            if (tileData == null)
            {
                Debug.LogWarning("No tile data found for the position: " + position);
                return;
            }

            else if (tileData.HasTerrain)
            {
                // process damage
                tileData.CurrentHealth -= damageAmount;

                Debug.Log(tileData.CurrentHealth);

                if (tileData.CurrentHealth <= 0)
                {
                    // notify map manager
                    TerrainDestroyed?.Invoke(position);
                    ShadowCaster2DCreator.updated = true;
                }
            }
        }
    }
}