using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    [CreateAssetMenu]
    public class TerrainTile : Tile
    {
        public bool HasOre { get { return OreTile != null; } }
        public OreTile OreTile { get; private set; } // Reference to the ore overlay tile.

        public string TerrainName;
        private int _curHealth;


        [Header("Background")]
        public Tile BackgroundTile;

        [Header("Destructible Terrain Settings")]
        public int BaseHealth = 1;
        [SerializeField] private int _maxHealth;
        public bool Destructible = true;

        [Header("Depth Settings (values are inclusive, lower numbers are deeper)")]
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
            this._curHealth = newMaxHealth;
        }

        public void Damage(int incomingDamage)
        {
            this._curHealth -= incomingDamage;
        }

        public bool ShouldBeDestroyed()
        {
            return this._curHealth <= 0;
        }

        public void InitializeStats()
        {
            this._curHealth = this._maxHealth;
        }

        public Tile GetBackgroundTile()
        {
            return Instantiate(this.BackgroundTile) as Tile;
        }
    }
}

