using Capstone.Build.World;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk
{
    public Vector2Int StartTile;
    public Vector2Int EndTile;
    public Vector2Int Coordinates;
    public Dictionary<Vector3Int, MapTileData> TileDataDict;
    public bool IsGenerated;

    public Chunk(Vector2Int chunkCoord)
    {
        Coordinates = chunkCoord;
        TileDataDict = new Dictionary<Vector3Int, MapTileData>();
        IsGenerated = false;
    }

    public void UpdateTileData(Vector3Int tilemapCoord, MapTileData tileData)
    {
        TileDataDict[tilemapCoord] = tileData;
    }

    public void UpdateTileData(Vector3Int tilemapCoord, TerrainTile terrainTile, OreTile oreTile)
    {
        if (!CoordinateIsWithinChunk(tilemapCoord)) {
            Debug.LogWarning("TILE OUT OF CHUNK BOUNDS: Tried to add tile at " + tilemapCoord + "to chunk with start = " + StartTile + " and end=" + EndTile + ".");
            return;
        }

        if (!TileDataDict.TryGetValue(tilemapCoord, out MapTileData tileData))
        {
            TileDataDict.Add(tilemapCoord, new());                
        }

        tileData.TerrainTile = terrainTile;
        tileData.OreTile = oreTile;

    }

    private bool CoordinateIsWithinChunk(Vector3Int tilemapCoord)
    {
        return tilemapCoord.x >= StartTile.x && tilemapCoord.x <= EndTile.x &&
            tilemapCoord.y >= StartTile.y && tilemapCoord.y <= EndTile.y;
    }
}