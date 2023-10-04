using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{

    [Header("Dimensions")]
    private static int width = 50;

    // calculated based on layers heights, so not editable or shown in gui
    private int height;
    
    public LayerData[] layerDataArray;

    private int[,] terrainMapEncoded;  // 2D array to store terrain type IDs

    void Start()
    {
        CalculateHeight();
        GenerateTerrainMap();
        PopulateScene();
    }

    public static int getWidth()
    {
        return Map.width;
    }

    public int getHeight()
    {
        return height;
    }

    private void CalculateHeight()
    {
        // height of map is equal to sum of heights of all layers
        this.height = layerDataArray.Sum(l => l.TotalDepth());
    }


    // Function to handle map generation logic
    // Creates an encoded version of the terrain map which stores terrainIDs instead of prefabs to speed up initial generation
    void GenerateTerrainMap()
    {
        terrainMapEncoded = new int[width,height];
        GenerateOreDeposits();
    }

    void GenerateOreDeposits()
    {
        foreach (var layerData in layerDataArray)
        {
            int[,] layerMap = layerData.buildLayer();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < layerData.TotalDepth(); y++)
                {
                    terrainMapEncoded[x, y + layerData.minDepth] = layerMap[x, y];
                }
            }
        }
    }

    void PopulateScene()
    {
        // load dictionary to convert terrainIds to terrainData
        Dictionary<int, TerrainData> terrainDataDictionary = TerrainDB.getTerrainIDDict();
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                // get id from encoded map
                int terrainTypeID = terrainMapEncoded[x, y];

                // get corresponding terrain type
                TerrainData terrainData = terrainDataDictionary[terrainTypeID];

                // get and instantiate prefab
                GameObject tilePrefab = terrainData.tilePrefab;
                Instantiate(tilePrefab, new Vector3(x, height - y, 0), Quaternion.identity);
            }
        }
    }
}
