using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(LayerData))]
public class LayerEditor : Editor
{
    LayerData layerData;
    private static string LAYER_TER_DATA_PATH = "Assets/Resources/Data/LayerTerrainData/";
    private Dictionary<LayerTerrainData, bool> layerTerrainDataFoldouts = new Dictionary<LayerTerrainData, bool>();

    void OnEnable()
    {
        layerData = (LayerData)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        if (GUILayout.Button("Re-populate Layer Terrain Data"))
        {
            RepopulateLayerTerrainData();
        }

        DrawLayerTerrainData();

    }

    private void DrawLayerTerrainData()
    {
        // Display each LayerTerrainData object in its own foldout
        SerializedProperty layerTerrainDataArray = serializedObject.FindProperty("layerTerrainData");

        for (int i = 0; i < layerTerrainDataArray.arraySize; i++)
        {
            SerializedProperty layerTerrainDataProperty = layerTerrainDataArray.GetArrayElementAtIndex(i);
            LayerTerrainData layerTerrainData = layerTerrainDataProperty.objectReferenceValue as LayerTerrainData;

            if (layerTerrainData != null)
            {
                // Ensure that the foldout dictionary has an entry for this LayerTerrainData
                if (!layerTerrainDataFoldouts.ContainsKey(layerTerrainData))
                {
                    layerTerrainDataFoldouts[layerTerrainData] = false;
                }

                // Use the foldout state from the dictionary
                layerTerrainDataFoldouts[layerTerrainData] = EditorGUILayout.Foldout(layerTerrainDataFoldouts[layerTerrainData], layerTerrainData.name);
                if (layerTerrainDataFoldouts[layerTerrainData])
                {
                    EditorGUI.indentLevel++;  // Indent for visual nesting
                    LayerTerrainDataEditor layerTerrainDataEditor = (LayerTerrainDataEditor)CreateEditor(layerTerrainData);
                    layerTerrainDataEditor.DrawLayerTerrainDataGUI(layerTerrainData);
                    DestroyImmediate(layerTerrainDataEditor);  // Prevent memory leak
                    EditorGUI.indentLevel--;  // Reset indentation
                }

                EditorGUILayout.Space();  // Add spacing for better visual separation
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void RepopulateLayerTerrainData()
    {
        // Add new ores from the database if they don't already exist in the layerTerrainData list
        foreach (var dbEntry in TerrainDB.GetTerrainList())
        {
            // check to see if this layer knows about current DB entry
            bool alreadyExists = false;
            foreach (var layerTerrain in layerData.layerTerrainData)
            {
                if (layerTerrain.terrainData == dbEntry)
                {
                    alreadyExists = true;
                    break;
                }
            }

            // if not already known, create new LayerTerrainData for this layer and the current TerrainDB entry 
            if (!alreadyExists)
            {
                LayerTerrainData newLayerTerrainData = ScriptableObject.CreateInstance<LayerTerrainData>();
                newLayerTerrainData.terrainData = dbEntry;
                newLayerTerrainData.layer = layerData;

                // add to assets
                string assetName = "ltd_layer" + layerData.id + "_" + dbEntry.name;
                string assetPath = LAYER_TER_DATA_PATH + assetName + ".asset";
                AssetDatabase.CreateAsset(newLayerTerrainData, assetPath);
                AssetDatabase.SaveAssets();


                layerData.layerTerrainData.Add(newLayerTerrainData);
            }
        }
        EditorUtility.SetDirty(layerData);  // Mark the LayerData object as dirty to ensure changes are saved
    }
}
