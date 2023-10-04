using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewTerrainData", menuName = "Terrain/Terrain Data")]
public class TerrainData : ScriptableObject
{
    public string terrainName;
    public int id;
    public bool isOre;
    public bool isDestructible;
    public int maxHP;
    public Sprite sprite;

    // The prefab associated with this terrain type
    public GameObject tilePrefab;
}
