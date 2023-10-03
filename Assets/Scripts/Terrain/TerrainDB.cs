using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "db_terrain", menuName = "Terrain/Terrain Database")]
public class TerrainDB : ScriptableObject
{

    private static string DB_PATH = "Data/Terrain/db_terrain";
    [SerializeField]
    private TerrainData[] terrainData;

    // global singleton instance
    private static TerrainDB _instance;

    public static TerrainData[] GetTerrainList() 
    { return Instance.terrainData; }



    private static TerrainDB Instance
    {
        get { 
            if (_instance == null)
            {
                ;
                _instance = Resources.Load<TerrainDB>(DB_PATH);
                if (_instance == null)
                {
                    Debug.Log("TerrainDB: No terrainDB instance found!");
                }
            }

            return _instance;
        }
    }

    // Dictionary to quickly map terrain type IDs to TerrainData
    public static Dictionary<int, TerrainData> getTerrainIDDict()
    {
        Dictionary<int, TerrainData> terrainDataDictionary = new();
        foreach (TerrainData terrainData in TerrainDB.GetTerrainList())
        {
            terrainDataDictionary[terrainData.id] = terrainData;
        }

        return terrainDataDictionary;
    }
}
