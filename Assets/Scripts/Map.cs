using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class Map : MonoBehaviour
{


    private GameObject[,] tiles;

    public TerrainType[] terrainTypes;
    public GameObject tilePfb;

    [Header("Dimensions")]
    public int width = 50;
    public int height = 50;
    public float scale = 1f;
    public Vector2 offset;

    [Header("Resource Chance")]
    public Wave[] resourceChanceWaves;
    public float[,] resourceChanceNoiseMap;
    public bool autoUpdate;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        GenerateNoiseMap();

        // todo:
        int layerDepth = 0;

        tiles = new GameObject[width, height];


        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                GameObject tile = Instantiate(tilePfb, new Vector3(x, y, 0), Quaternion.identity);
                TerrainType terrainType = GetTerrainType(layerDepth, resourceChanceNoiseMap[x, y]);
                tile.GetComponent<SpriteRenderer>().sprite = terrainType.GetTexture();

                tile.transform.SetParent(this.transform, false);
                tiles[x, y] = tile;
            }
        }
    }

    public float[,] GetNoiseMap()
    {
        return resourceChanceNoiseMap;
    }

    TerrainType GetTerrainType(int layerDepth, float resourceChance) { 
        List<TerrainTempData> terrainTemp = new List<TerrainTempData>();
        Parallel.ForEach(terrainTypes, terrain =>
        {
            if(terrain.MatchCondition(layerDepth, 1 - resourceChance))
            {
                terrainTemp.Add(new TerrainTempData(terrain));
            }
        });

        // find closest match
        TerrainType curClosestMatch = null;
        float curMax = 0f;

        foreach(TerrainTempData curTerrain in terrainTemp)
        {
            if (curClosestMatch == null)
            {
                curClosestMatch = curTerrain.terrain;
                curMax = curClosestMatch.minResourceChance;
            }
            else
            {
                float curDif = curTerrain.terrain.minResourceChance;
                if (curDif > curMax)
                {
                    curClosestMatch = curTerrain.terrain;
                    curMax = curClosestMatch.minResourceChance;
                }
            }
        }

        if(curClosestMatch == null)
        {
            curClosestMatch = terrainTypes[0];
        }

        return curClosestMatch;
    }

    public void GenerateNoiseMap()
    {
        this.offset = new Vector2(UnityEngine.Random.Range(-1000000, 1000000), UnityEngine.Random.Range(-1000000, 1000000));
        resourceChanceNoiseMap = NoiseGen.Generate(width, height, scale, resourceChanceWaves, offset);      
    }

    public class TerrainTempData
    {
        public TerrainType terrain;
        public TerrainTempData(TerrainType terrainType)
        {
            terrain = terrainType;
        }

        public float GetDiffValue(int layerDepth, float resourceChance)
        {
            return (resourceChance - terrain.minResourceChance);
        }
    }
}
