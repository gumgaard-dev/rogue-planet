using UnityEngine;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    [CreateAssetMenu]
    public class TerrainTile : Tile
    {
        public bool HasOre { get { return OreTile != null;  } }
        public bool Destructible = true;
        public int BaseHealth = 1;

        private int _maxHealth;
        public int CurHealth { get; protected set; }
        public OreTile OreTile { get; private set; } // Reference to the ore overlay tile.

        [Header("Spawn Settings")]
        public int minDepth;
        public int maxDepth;
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(location, tilemap, ref tileData);
        }

        public void SetOre(OreTile oreTile)
        {
            this.OreTile = oreTile;
            this.UpdateMaxHealth((int)(this.BaseHealth * oreTile.TerrainHealthModifier));
        }
        
        public void UpdateMaxHealth(int newMaxHealth)
        {
            this._maxHealth = newMaxHealth;
            this.CurHealth = newMaxHealth;
        }

        public void Damage(int incomingDamage)
        {
            this.CurHealth -= incomingDamage;
        }

        public bool ShouldBeDestroyed()
        {
            return this.CurHealth <= 0;
        }
    }
}
