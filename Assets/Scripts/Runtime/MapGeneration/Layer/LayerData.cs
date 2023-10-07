using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "lay_new", menuName = "Terrain/Layer Type")]
[System.Serializable]
public class LayerData : ScriptableObject
{

    public int minDepth;
    public int maxDepth;

    public TerrainData baseTerrainData;

    public List<LayerTerrainData> layerTerrainData;

    public int TotalDepth()
    {
        return maxDepth - minDepth;
    }

    public int[,] buildLayer()
    {
        int width = Map.getWidth();
        int height = this.TotalDepth();

        int[,] layerMapEncoded = new int[width, height];

        // Initialize with base terrain
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                layerMapEncoded[x, y] = this.baseTerrainData.id;
            }
        }


        foreach (var layerTerrainData in this.layerTerrainData)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if(layerTerrainData.ShouldSpawn(x, y))
                    {
                        layerMapEncoded[x, y] = layerTerrainData.terrainData.id;

                    }
                }
            }
        }

        return layerMapEncoded;
    }
}