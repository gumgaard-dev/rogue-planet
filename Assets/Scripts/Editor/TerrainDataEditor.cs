#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainData))]
public class TerrainDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainData terrainData = (TerrainData)target;
        DrawDefaultInspector();

        if (terrainData.tilePrefab == null)
        {
            if (GUILayout.Button("Create New Terrain Type"))
            {
                CreateNewTerrainType();
            }
        }
    }

    void CreateNewTerrainType()
    {
        TerrainData terrainData = (TerrainData)target;
        
        // create gameobject in scene
        GameObject newTerrain = new GameObject(terrainData.terrainName);

        // config object
        newTerrain.AddComponent<SpriteRenderer>().sprite = terrainData.sprite;
        newTerrain.AddComponent<BoxCollider2D>();
        if(terrainData.isDestructible)
        {
            newTerrain.AddComponent<DestructibleTerrain>().maxHP = terrainData.maxHP;
        }

        // save GameObject as a prefab
        string path = "Assets/Prefabs/Terrain/pfb_" + newTerrain.name + ".prefab";
        PrefabUtility.SaveAsPrefabAssetAndConnect(newTerrain, path, InteractionMode.UserAction);

        // set reference to this prefab to the TerrainData object
        terrainData.tilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        TerrainDB.RegisterNew(terrainData);

        // destroy object from scene
        DestroyImmediate(newTerrain);
    }
}
#endif
