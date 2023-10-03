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

        foreach (var layerTerrainData in this.layerTerrainData)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if(layerTerrainData.ShouldSpawn(x, y))
                    {
                        layerMapEncoded[x, y] = layerTerrainData.terrainData.id;

                    } else if (layerMapEncoded[x,y] == 0)
                    {
                        layerMapEncoded[x, y] = this.baseTerrainData.id;
                    }
                }
            }
        }

        return layerMapEncoded;
    }
}