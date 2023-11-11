using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    [CreateAssetMenu]
    public class TerrainTile : RuleTile
    {
        public bool HasOre { get { return OreTile != null; } }
        public OreTile OreTile { get; private set; } // Reference to the ore overlay tile.

        public string TerrainName;
        public int CurrrentHealth;


        [Header("Background")]
        public Tile BackgroundTile;

        [Header("Destructible Terrain Settings")]
        public int BaseHealth = 1;
        [SerializeField] private int _maxHealth;
        public bool Destructible = true;

        [Header("Depth Settings (values are inclusive, higher numbers are deeper)")]
        public int minDepth;
        public int maxDepth;

        

        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(location, tilemap, ref tileData);
        }

        public override bool RuleMatch(int neighbor, TileBase tile)
        {
            if (tile is TerrainTile)
            {
                // Special handling for the "self" tile
                if (neighbor == TilingRule.Neighbor.This && this == tile)
                {
                    return true;
                }

                // Special handling for "not a neighbor" - needed for isolated tiles
                if (neighbor == TilingRule.Neighbor.NotThis)
                {
                    return !(tile is TerrainTile);
                }

                // General case - any TerrainTile is considered a match
                return true;
            }
            return base.RuleMatch(neighbor, tile);
        }

        public Tile GetBackgroundTile()
        {
            return Instantiate(this.BackgroundTile) as Tile;
        }

        public MapTileData CreateMapTileData()
        {
            MapTileData tileData = new MapTileData();

            tileData.TerrainTile = this;

            return tileData;
        }
    }
}

