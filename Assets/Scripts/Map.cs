using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using UnityEngine;

public class Map : MonoBehaviour
{

    public TerrainType[] terrainTypes;
    public GameObject tilePrefab;

    [Header("Dimensions")]
    public int width = 50;
    public int height = 50;
    public float scale = 1f;
    public Vector2 offset;

    [Header("Resource Chance")]
    public Wave[] resourceChanceWaves;
    public float[,] resourceChanceNoiseMap;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        resourceChanceNoiseMap = NoiseGen.Generate(width, height, scale, resourceChanceWaves, offset);

        // todo:
        int layerDepth = 0;

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                TerrainType terrainType = GetTerrainType(layerDepth, resourceChanceNoiseMap[x, y]);
                tile.GetComponent<SpriteRenderer>().sprite = terrainType.GetTexture();
            }
        }
    }

    TerrainType GetTerrainType(int layerDepth, float resourceChance) { 
        List<TerrainTempData> terrainTemp = new List<TerrainTempData>();
        Parallel.ForEach(terrainTypes, terrain =>
        {
            if(terrain.MatchCondition(layerDepth, resourceChance))
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

    public class TerrainTempData
    {
        public TerrainType terrain;
        public TerrainTempData(TerrainType terrainType)
        {
            terrain = terrainType;
        }

        public float GetDiffValue(int layerDepth, float resourceChance)
        {
            return (layerDepth - terrain.minLayerDepth) + (resourceChance - terrain.minResourceChance);
        }
    }
}
