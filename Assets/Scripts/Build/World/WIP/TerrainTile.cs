using Build.Component;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TreeEditor.TreeEditorHelper;

namespace Capstone.WIP
{
    [CreateAssetMenu]
    public class TerrainTile : Tile
    {
        public bool HasOre { get; set; } = false;
        public bool Destructible = true;
        public int MaxHealth = 1;
        public int CurHealth { get; protected set; }
        public OreTile OreTile { get; private set; } // Reference to the ore overlay tile.

        [Header("Spawn Settings")]
        public int minDepth;
        public int maxDepth;

        public TerrainTile()
        {
            if (HasOre)
            {
                this.MaxHealth = (int)(this.MaxHealth * this.OreTile.TerrainHealthModifier);
            }
            this.CurHealth = this.MaxHealth;
        }
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(location, tilemap, ref tileData);
        }

        public void SetOreRef(OreTile? oreTile)
        {
            this.OreTile = oreTile;
            this.HasOre = this.OreTile != null;
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

