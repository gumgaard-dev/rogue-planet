using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "TerrainType", menuName = "New Terrain Type")]
public class TerrainType : ScriptableObject
{
    // can store multiple textures for one terrain type, for added visual interest
    public Sprite[] textures;

    // minimum layer at which the given terrain will spawn
    public int minLayerDepth;
    public float minResourceChance;
    
    public Sprite GetTexture()
    {
        int randomTextureIndex = Random.Range(0, textures.Length);
        return textures[randomTextureIndex];
    }

    public bool MatchCondition(int layerDepth, float resourceChance)
    {
        bool canSpawnInLayer = layerDepth >= minLayerDepth;
        bool resourceChanceMet = resourceChance >= minResourceChance;
        return canSpawnInLayer && resourceChanceMet;
    }
}
