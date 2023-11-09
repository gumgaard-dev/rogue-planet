using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    [System.Serializable]
    public class OreTerrainSpawnData
    {
        public TerrainTile terrainType;
        public float veinChance;
    }
    [CreateAssetMenu]
    public class OreTile : Tile
    {
        // set in custom editor class
        public Sprite DefaultSprite;

        // used by OreVisibilityController
        public bool IsVisible { get; set; }

        public string OreName;
        [Header("Ore Settings")]
        public OreCollectable OreToDrop;

        [Header("Terrain Modifiers")]
        public float TerrainHealthModifier = 1;

        [Header("Spawn Rate per Terrain type")]
        public List<OreTerrainSpawnData> TerrainSpawnData;

        [Header("Vein Settings")]
        public int MaxVeinSize = 10; // max number of ores in a vein
        
        

        public float GetSpawnChanceInTerrain(string terrainName)
        {
            foreach (var spawnData in TerrainSpawnData)
            {
                if (spawnData.terrainType.TerrainName == terrainName)
                {
                    return spawnData.veinChance;
                }
            }
            return 0f; // return 0 if the terrain type is not found in the list
        }
        public override void GetTileData(Vector3Int cell, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = IsVisible ? DefaultSprite : null;
        }

    }

}
