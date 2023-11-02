/*using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "db_terrain", menuName = "Terrain/Terrain Database")]
public class TerrainDB : ScriptableObject
{

    private static string DB_PATH = "Data/Terrain/db_terrain";
    [SerializeField]
    private List<TerrainData> terrainData;

    // global singleton instance
    private static TerrainDB _instance;

    public static List<TerrainData> GetTerrainList()
    { return Instance.terrainData; }


    public static void RegisterNew(TerrainData terrain)
    {
        Instance.terrainData.Add(terrain);
    }
    private static TerrainDB Instance
    {
        get
        {
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
}*/
