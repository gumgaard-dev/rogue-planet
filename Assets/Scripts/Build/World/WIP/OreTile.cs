using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class OreTile : Tile
{
    [Header("Ore Settings")]
    public OreCollectable OreToDrop;
    public float TerrainHealthModifier = 1;
    
    [Header("Starting Point Settings")]
    public int MinDepth;
    public int MaxDepth;
    public float Frequency;

    [Header("Vein Settings")]
    public int MaxPropagationValue = 10; // max number of ores in a vein
}
