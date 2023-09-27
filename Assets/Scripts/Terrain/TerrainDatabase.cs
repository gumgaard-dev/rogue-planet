using UnityEngine;

[CreateAssetMenu(fileName = "db_terrain", menuName = "Terrain/Terrain Database", order = 1)]
public class TerrainDatabase : ScriptableObject
{
    [SerializeField]
    private TerrainData[] terrainData;

    public TerrainData[] GetTerrainData() 
    { return terrainData; }
}
