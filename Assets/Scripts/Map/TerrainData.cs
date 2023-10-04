using UnityEngine;

[CreateAssetMenu(fileName = "ter_new", menuName = "Terrain/Terrain Type")]
[System.Serializable]
public class TerrainData : ScriptableObject
{
    public int id;
    public GameObject tilePrefab;
    public bool isOre; // Indicator if this terrain is an ore or just a regular terrain type
}

