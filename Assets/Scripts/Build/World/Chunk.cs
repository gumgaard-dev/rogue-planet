using Capstone.Build.World;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Vector2Int StartTile;
    public Vector2Int EndTile;
    public Vector2Int CoordinatesInChunkMap;
    public Dictionary<Vector3Int, TerrainTileData> TerrainDataMap;
    public bool IsGenerated;

    public Chunk(Vector2Int coordinates)
    {
        CoordinatesInChunkMap = coordinates;
        TerrainDataMap = new Dictionary<Vector3Int, TerrainTileData>();
        IsGenerated = false;
    }

    public void UpdateTileData(Vector3Int localPosition, TerrainTileData tileData)
    {
        TerrainDataMap[localPosition] = tileData;
    }

}