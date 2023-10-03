using System;
using UnityEngine;


[System.Serializable]
public class LayerTerrainData : ScriptableObject
{
    public LayerData layer;

    [Header("Ore")]
    public TerrainData terrainData;

    [Header("Rarity")]
    public float spawnChance;

    [Header("Noise")]
    [Range(0.1f, 3f)]
    public float scale = 1f;
    public Vector2 offset;

    public Wave[] spawnChanceWaves;
    public float[,] noiseMap;

    public void GenerateResourceNoisemap ()
    {
        int layerHeight = this.layer.TotalDepth();
        int mapWidth = Map.getWidth();
        this.noiseMap = NoiseGen.Generate(mapWidth, layerHeight, scale, spawnChanceWaves, offset);
    }


    public Texture2D GenerateNoiseMapTexture ()
    {
        this.GenerateResourceNoisemap();

        int width = noiseMap.GetLength(0) * 2;
        int height = noiseMap.GetLength(1) * 2;

        Texture2D texture = new(width, height);
        for (int x = 0; x < width; x += 2)
        {
            for (int y = 0; y < height; y += 2)
            {
                float value = noiseMap[x / 2, y / 2];
                Color color = new(value, value, value, 1.0f);
                texture.SetPixel(x, y, color);
                texture.SetPixel(x+1, y, color);
                texture.SetPixel(x, y+1, color);
                texture.SetPixel(x+1, y+1, color);
            }
        }
        texture.Apply();
        return texture;
    }

    public bool TerrainCanSpawnInLayer()
    {
        return spawnChance > 0 && spawnChanceWaves != null;
    }

    internal bool ShouldSpawn(int x, int y)
    {
        if (!TerrainCanSpawnInLayer())
        {
            return false;
        }

        if (noiseMap == null)
        {
            GenerateResourceNoisemap();
        }

        return noiseMap[x, y] < spawnChance;
    }
}
