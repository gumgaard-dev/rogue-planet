using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainUtils;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

namespace Capstone.WIP
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Tilemaps")]
        public Tilemap terrainTilemap;
        public Tilemap oreTilemap;

        [Header("Dimensions")]
        public int width = 100;
        public int height = 100;

        [Header("Terrain Type Config")]
        public List<TerrainTile> TerrainTileTypes;
        [Header("Ore Type Config")]
        public List<OreTile> OreTileTypes;

        // class variable
        private static Vector3Int[] DIRECTIONS = {Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right};

        void Start()
        {
            SetTileStats();
            GenerateTerrainMap();
            GenerateOreMap();
        }

        private void SetTileStats()
        {
            foreach(TerrainTile tile in TerrainTileTypes)
            {
                tile.UpdateMaxHealth(tile.BaseHealth);
            }
        }

        void GenerateTerrainMap()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    TerrainTile selectedTile = GetTerrainTileAtDepth(y);
                    PlaceTerrain(selectedTile, new Vector3Int(x, y));
                }
            }
        }
        TerrainTile GetTerrainTileAtDepth(int depth)
        {
            List<TerrainTile> validTiles = new();

            foreach (var terrainType in TerrainTileTypes)
            {
                if (depth >= terrainType.minDepth && depth < terrainType.maxDepth)
                {
                    validTiles.Add(terrainType);
                }
            }


            // one valid tile, return it
            if (validTiles.Count == 1)
            {
                return validTiles[0];
            }

            // multiple valid tiles, choose randomly
            else if (validTiles.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, validTiles.Count);
                return validTiles[randomIndex];
            }

            // no valid tiles
            return null;
        }

        void GenerateOreMap()
        {
            // iterate over each type of ore
            foreach (OreTile oreTile in OreTileTypes)
            {
                // generate a list of starting points, and add the starting points to the tilemap
                List<Vector3Int> startPoints = GenerateStartingPoints(oreTile);

                // grow veins for each starting point for this ore
                GenerateOreVeins(oreTile, startPoints);
            }
        }


        void GenerateOreVeins(OreTile oreTile, List<Vector3Int> startingPositions)
        {
            // generate a vein for each starting position
            for (int i = 0; i < startingPositions.Count; i++)
            {
                // get current vein start point from generated positions
                Vector3Int startPoint = startingPositions[i];

                // calculate the size of the current vein
                float veinSize = UnityEngine.Random.Range(1, oreTile.MaxPropagationValue);

                // grow the vein out until it reaches vein size
                GrowVein(oreTile, startPoint, SelectRandomDirection(), veinSize);
            }
        }



        void GrowVein(OreTile oreTile, Vector3Int lastPosition, Vector3Int direction, float propagationValue)
        {
            Vector3Int currentPosition = lastPosition + direction;
            if (propagationValue > 0 && ValidNextVeinPosition(currentPosition, oreTile, propagationValue))
            {
                PlaceOre(oreTile, currentPosition);
                propagationValue -= 1;
                GrowVein(oreTile, currentPosition, SelectRandomDirection(), propagationValue);
            }
        }

        bool ValidNextVeinPosition(Vector3Int position, OreTile oreTile, float currentPropagationValue)
        {
            if (currentPropagationValue == 0) return false;

            if (!IsValidPosition(position)) return false;

            if (terrainTilemap.GetTile(position) == null) return false;

            return true;
            
        }

        private void PlaceOre(OreTile oreTile, Vector3Int position)
        {
            // Inverse the y-coordinate before placing the tile
            Vector3Int invertedCenteredPosition = new((width / 2) - position.x, position.y * -1, position.z);

            TerrainTile terrainTile = terrainTilemap.GetTile(invertedCenteredPosition) as TerrainTile;

            if (terrainTile != null) {
                // updates the health values for the tile, and sets the associated ore tile
                terrainTile.SetOre(oreTile);
                oreTilemap.SetTile(invertedCenteredPosition, Instantiate(oreTile));
            }
        }

        private void PlaceTerrain(TerrainTile terrainTile, Vector3Int position)
        {
            // Inverse the y-coordinate before placing the tile
            Vector3Int invertedCenteredPosition = new((width / 2) - position.x, position.y * -1, position.z);
            TerrainTile tileClone = Instantiate(terrainTile);
            terrainTilemap.SetTile(invertedCenteredPosition, tileClone);
        }

        List<Vector3Int> GenerateStartingPoints(OreTile oreTile)
        {
            List<Vector3Int> startingPoints = new();

            for(int y = oreTile.MinDepth; y <= oreTile.MaxDepth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float chance = UnityEngine.Random.Range(0f, 1f);
                    if (chance < oreTile.Frequency)
                    {
                        Vector3Int startPoint = new Vector3Int(x, y, 0);
                        if (IsValidStartingBlock(startPoint))
                        {
                            PlaceOre(oreTile, startPoint);
                            startingPoints.Add(startPoint);
                        }
                    }
                }
            }

            return startingPoints;
        }
        private bool IsValidPosition(Vector3Int pos)
        {
            // return false if already populated
            return oreTilemap.GetTile(pos) == null;
        }

        private bool IsValidStartingBlock(Vector3Int pos)
        {
            // can add more requirements if needed
            return IsValidPosition(pos);
        }

        private static Vector3Int SelectRandomDirection()
        {
            int randomIndex = UnityEngine.Random.Range(0, MapGenerator.DIRECTIONS.Length);
            return MapGenerator.DIRECTIONS[randomIndex];
        }
    }
}
