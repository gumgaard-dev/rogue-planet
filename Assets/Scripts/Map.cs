using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Databases")]
    [SerializeField]
    private TerrainDatabase terrainDB;
    [SerializeField]
    private LayerDatabase layerDB;

    [Header("Dimensions")]
    public int width = 50;

    // calculated based on layers heights, so not editable or shown in gui
    private int height;
    
    public LayerData[] layerDataArray;
    public TerrainData[] terrainDataArray;

    private int[,] terrainMapEncoded;  // 2D array to store terrain type IDs

    // Dictionary to quickly map terrain type IDs to TerrainData
    private readonly Dictionary<int, TerrainData> terrainDataDictionary = new();

    void Start()
    {
        PopulateLayerData();
        PopulateTerrainData();
        PopulateTerrainDataDictionary();
        CalculateHeight();
        GenerateTerrainMap();
        PopulateScene();
    }

    private void CalculateHeight()
    {
        this.height = layerDataArray.Sum(l => l.TotalDepth());
    }

    private void PopulateLayerData()
    {
        if (layerDB != null)
        {
            Debug.LogWarning("Map: not connected to layer database.");
            this.layerDataArray = new LayerData[0];
        } 
        else
        {
            this.layerDataArray = layerDB.GetLayerData();
        }
        
    }

    private void PopulateTerrainData()
    {
        if (layerDB != null)
        {
            Debug.LogWarning("Map: not connected to terrain database.");
            this.terrainDataArray = new TerrainData[0];
        }
        else
        {
            this.layerDataArray = layerDB.GetLayerData();
        }
    }

    void PopulateTerrainDataDictionary()
    {
        foreach (var terrainData in terrainDataArray)
        {
            terrainDataDictionary[terrainData.id] = terrainData;
        }
    }

    void GenerateTerrainMap()
    {
        terrainMapEncoded = new int[width, height];

        foreach (var layer in layerDataArray)
        {
            foreach (var terrainData in layer.layerTerrainData)
            {
                for (int x = 0; x < width; ++x)
                {
                    for (int y = layer.minDepth; y <= layer.maxDepth; ++y)
                    {
                        terrainMapEncoded[x,y] = terrainData.DetermineTerrainTypeID(x, y);
                    }
                }
            }
        }
    }




    void PopulateScene()
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                int terrainTypeID = terrainMapEncoded[x, y];
                TerrainData terrainData = terrainDataDictionary[terrainTypeID];
                GameObject tilePrefab = terrainData.tilePrefab;
                Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }
}
