using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "lay_new", menuName = "Terrain/Layer Type")]
[System.Serializable]
public class LayerData : ScriptableObject
{
    public int id;

    public int minDepth;
    public int maxDepth;

    public TerrainData baseTerrainData;
    public List<LayerTerrainData> layerTerrainData;  // Changed from array to List
    public TerrainDatabase oreDatabase;  // Reference to the central ore database

    public int TotalDepth()
    {
        return maxDepth - minDepth;
    }
}

[System.Serializable]
public class LayerTerrainData : ScriptableObject  // Removed inheritance from ScriptableObject
{
    [System.NonSerialized]
    public LayerData layer;
    
    [Header("Ore")]
    public TerrainData oreData;

    [Header("Rarity")]
    public float spawnThreshold;

    [Header("Noise")]
    public float scale;
    public Vector2 offset;
    public Wave[] spawnChanceWaves;
    public float[,] noiseMap;


    public Texture2D GenerateNoiseMapTexture()
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new(width, height);
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                float value = noiseMap[x, y];
                Color color = new(value, value, value, 1.0f);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    public int DetermineTerrainTypeID(int x, int y)
    {
        float noiseValue = this.noiseMap[x, y];

        if (noiseValue >= this.spawnThreshold)
        {
            return this.oreData.id;  // Return ore ID if ore spawn threshold is met
        }
        else return this.layer.baseTerrainData.id;
    }
}
