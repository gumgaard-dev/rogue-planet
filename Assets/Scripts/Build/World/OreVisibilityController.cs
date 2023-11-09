using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    public class OreVisibilityController : MonoBehaviour
    {
        public MapGenerator map;
        private Tilemap OreTilemap;
        private Tilemap TerrainTilemap;

        public Dictionary<Vector3Int, OreTile> OreTilePositions;

        public int VisibilityRadius;

        private Color _invisibleColor = new(1f, 1f, 1f, 0f);
        private Color _visibleColor = Color.white;

        private void Awake()
        {
            if (map == null)
            {
                Debug.Log("No MapGenerator set in the inspector");
            } else
            {
                OreTilemap = map.OreTilemap;
                TerrainTilemap = map.TerrainTilemap;
            }

            this.OreTilePositions = new Dictionary<Vector3Int, OreTile>();
        }

        private void Update()
        {
            UpdateVisibilityAllSprites();
        }

        // this will check ALL tiles
        public void UpdateVisibilityAllSprites()
        {
            if (OreTilePositions.Count > 0 && map != null)
            {
                foreach (Vector3Int position in this.OreTilePositions.Keys)
                {
                    if (NearEmptyTile(position))
                    {
                        SetTileVisible(position);
                    }
                    else
                    {
                        SetTileInvisible(position);
                    }
                }
            }
        }

        // Call this method whenever a terrain tile is destroyed
        public void UpdateVisibilityNearPosition(Vector3Int destroyedTilePosition)
        {
            if (OreTilePositions.Count > 0 && map != null)
            {
                // Define the bounds to check based on the VisibilityRadius
                Vector3Int minBounds = new Vector3Int(destroyedTilePosition.x - VisibilityRadius, destroyedTilePosition.y - VisibilityRadius, destroyedTilePosition.z);
                Vector3Int maxBounds = new Vector3Int(destroyedTilePosition.x + VisibilityRadius, destroyedTilePosition.y + VisibilityRadius, destroyedTilePosition.z);

                // Check in a square defined by the min and max bounds
                for (int x = minBounds.x; x <= maxBounds.x; x++)
                {
                    for (int y = minBounds.y; y <= maxBounds.y; y++)
                    {
                        Vector3Int positionToCheck = new Vector3Int(x, y, destroyedTilePosition.z);

                        // Skip the destroyed tile itself
                        if (positionToCheck == destroyedTilePosition) continue;

                        // If there's an ore tile at this position, check if it should be visible or invisible
                        if (OreTilePositions.ContainsKey(positionToCheck))
                        {
                            SetTileVisible(positionToCheck);
                        }
                    }
                }
            }
        }


        // check the visibility radius for empty terrain tiles
        private bool NearEmptyTile(Vector3Int position)
        {
            int x = position.x;
            int y = position.y;

            Vector3Int positionToCheck = position;

            for (int xOffset = -VisibilityRadius; xOffset <= VisibilityRadius; xOffset++)
            {
                positionToCheck.x = x + xOffset;
                for (int yOffset = -VisibilityRadius; yOffset <= VisibilityRadius; yOffset++)
                {
                    positionToCheck.y = y + yOffset;

                    // Avoid checking the tile itself
                    if (!(xOffset == 0 && yOffset == 0))
                    {
                        if (!TerrainExistsAtPosition(positionToCheck))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool OreExistsAtPosition(Vector3Int position)
        {
            // Check if the specified tile is empty.
            return OreTilePositions.ContainsKey(position);
        }

        private bool TerrainExistsAtPosition(Vector3Int position)
        {
            // Check if the specified tile is empty.
            return TerrainTilemap.GetTile(position) != null;
        }

        private void SetTileVisible(Vector3Int position)
        {
            OreTile toSet = OreTilemap.GetTile(position) as OreTile;
            toSet.IsVisible = true;
            OreTilemap.RefreshTile(position);
        }

        private void SetTileInvisible(Vector3Int position)
        {
            OreTile toSet = OreTilemap.GetTile(position) as OreTile;
            toSet.IsVisible = false;
            OreTilemap.RefreshTile(position);
        }

        public void AddToOrePositionList(Vector3Int newOrePosition, OreTile oreType)
        {
            // don't add if already in the list
            if (OreTilePositions.ContainsKey(newOrePosition)){ return; }

            OreTilePositions.Add(newOrePosition, oreType);

        }

        public void RemoveFromOrePositionList(Vector3Int orePositionToRemove)
        {
            // remove only if contained in the list 
            if (OreTilePositions.ContainsKey(orePositionToRemove))
            {
                OreTilePositions.Remove(orePositionToRemove);
            }
        }
    }
}