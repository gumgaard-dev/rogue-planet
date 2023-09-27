using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(LayerData))]
public class LayerEditor : Editor
{
    LayerData layerData;
    SerializedProperty oreDatabaseProperty;

    void OnEnable()
    {
        layerData = (LayerData)target;
        oreDatabaseProperty = serializedObject.FindProperty("oreDatabase");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(oreDatabaseProperty);
        DrawDefaultInspector();

        if (GUILayout.Button("Re-populate Layer Terrain Data"))
        {
            RepopulateLayerTerrainData();
        }

        // Display LayerTerrainData objects using the custom inspector
        SerializedProperty layerTerrainDataArray = serializedObject.FindProperty("layerTerrainData");
        for (int i = 0; i < layerTerrainDataArray.arraySize; i++)
        {
            SerializedProperty layerTerrainDataProperty = layerTerrainDataArray.GetArrayElementAtIndex(i);
            LayerTerrainData layerTerrainData = layerTerrainDataProperty.objectReferenceValue as LayerTerrainData;

            if (layerTerrainData != null)
            {
                LayerTerrainDataEditor layerTerrainDataEditor = (LayerTerrainDataEditor)CreateEditor(layerTerrainData);
                layerTerrainDataEditor.DrawLayerTerrainDataGUI(layerTerrainData);
                DestroyImmediate(layerTerrainDataEditor);  // Important to prevent memory leak
            }

            EditorGUILayout.Space();  // Add some spacing between entries
        }

        serializedObject.ApplyModifiedProperties();
    }

    void RepopulateLayerTerrainData()
    {
        // This part won't overwrite existing LayerTerrainData entries, but will add new ones based on the TerrainDatabase.
        var oreDatabase = layerData.oreDatabase;
        if (oreDatabase == null)
        {
            Debug.LogWarning("Ore Database is not assigned.");
            return;
        }

        // Add new ores from the database if they don't already exist in the layerTerrainData list
        foreach (var terrainType in oreDatabase.GetTerrainData())
        {
            bool alreadyExists = false;
            foreach (var layerTerrain in layerData.layerTerrainData)
            {
                if (layerTerrain.oreData == terrainType)
                {
                    alreadyExists = true;
                    break;
                }
            }

            if (!alreadyExists)
            {
                LayerTerrainData newLayerTerrainData = ScriptableObject.CreateInstance<LayerTerrainData>();
                newLayerTerrainData.oreData = terrainType;

                // add to assets
                string assetName = "ltd_layer" + layerData.id + "_" + terrainType.name;
                AssetDatabase.CreateAsset(newLayerTerrainData, "Assets/data/LayerTerrainData/" + assetName + ".asset");
                AssetDatabase.SaveAssets();


                layerData.layerTerrainData.Add(newLayerTerrainData);
            }
        }

        // Optional: Remove any LayerTerrainData entries that no longer exist in the TerrainDatabase
        // (uncomment the below code block if you want to use this feature)
        /*
        for (int i = layerData.layerTerrainData.Count - 1; i >= 0; i--)
        {
            var layerTerrain = layerData.layerTerrainData[i];
            bool stillExistsInDatabase = false;
            foreach (var terrainType in oreDatabase.terrainTypes)
            {
                if (layerTerrain.oreData == terrainType)
                {
                    stillExistsInDatabase = true;
                    break;
                }
            }

            if (!stillExistsInDatabase)
            {
                layerData.layerTerrainData.RemoveAt(i);
            }
        }
        */

        EditorUtility.SetDirty(layerData);  // Mark the LayerData object as dirty to ensure changes are saved
    }
}
