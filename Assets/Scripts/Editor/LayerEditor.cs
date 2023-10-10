using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(LayerData))]
public class LayerEditor : Editor
{
    LayerData layerData;
    private static string LAYER_TER_DATA_PATH = "Assets/Resources/Data/LayerTerrainData/";

    // variables for terrain selection
    private int selectedIndex = 0;
    private List<string> terrainNames;

    private Dictionary<LayerTerrainData, bool> layerTerrainDataFoldouts = new Dictionary<LayerTerrainData, bool>();

    void OnEnable()
    {
        layerData = (LayerData)target;
        PopulateTerrainNames();
    }

    private void PopulateTerrainNames()
    {
        var terrains = TerrainDB.GetTerrainList();
        terrainNames = new List<string>();
        for (int i = 0; i < terrains.Count; i++)
        {
            if (terrains[i].isOre)
            {
                terrainNames.Add(terrains[i].name);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        selectedIndex = EditorGUILayout.Popup("Terrain Type", selectedIndex, terrainNames.ToArray());
        if (GUILayout.Button("Add New Ore to Layer"))
        {
            AddNewLayerTerrainData();
        }

        DrawLayerTerrainData();

    }

    private void DrawLayerTerrainData()
    {
        SerializedProperty layerTerrainDataArray = serializedObject.FindProperty("layerTerrainData");

        List<int> indicesToRemove = new List<int>();

        for (int i = 0; i < layerTerrainDataArray.arraySize; i++)
        {
            SerializedProperty layerTerrainDataProperty = layerTerrainDataArray.GetArrayElementAtIndex(i);
            LayerTerrainData layerTerrainData = layerTerrainDataProperty.objectReferenceValue as LayerTerrainData;

            if (layerTerrainData != null)
            {
                EditorGUILayout.BeginHorizontal();

                // Ensure that the foldout dictionary has an entry for this LayerTerrainData
                if (!layerTerrainDataFoldouts.ContainsKey(layerTerrainData))
                {
                    layerTerrainDataFoldouts[layerTerrainData] = false;
                }

                // Use the foldout state from the dictionary
                layerTerrainDataFoldouts[layerTerrainData] = EditorGUILayout.Foldout(layerTerrainDataFoldouts[layerTerrainData], layerTerrainData.name);

                // Remove button
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    indicesToRemove.Add(i);
                }

                EditorGUILayout.EndHorizontal();

                if (layerTerrainDataFoldouts[layerTerrainData])
                {
                    EditorGUI.indentLevel++;  // Indent for visual nesting
                    LayerTerrainDataEditor layerTerrainDataEditor = (LayerTerrainDataEditor)CreateEditor(layerTerrainData);
                    layerTerrainDataEditor.DrawLayerTerrainDataGUI(layerTerrainData);
                    DestroyImmediate(layerTerrainDataEditor);  // Prevent memory leak
                    EditorGUI.indentLevel--;  // Reset indentation
                }

                EditorGUILayout.Space();
            }
        }

        // Handle removals using the SerializedProperty
        for (int i = indicesToRemove.Count - 1; i >= 0; i--) // Iterate in reverse order to safely remove items
        {
            SerializedProperty layerTerrainDataProperty = layerTerrainDataArray.GetArrayElementAtIndex(indicesToRemove[i]);
            LayerTerrainData layerTerrainData = layerTerrainDataProperty.objectReferenceValue as LayerTerrainData;
            string assetPath = AssetDatabase.GetAssetPath(layerTerrainData);

            layerTerrainDataArray.DeleteArrayElementAtIndex(indicesToRemove[i]);

            AssetDatabase.DeleteAsset(assetPath);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void AddNewLayerTerrainData()
    {
        var selectedTerrain = TerrainDB.GetTerrainList()[selectedIndex];

        // Check if the terrain is already present
        bool alreadyExists = false;
        foreach (var layerTerrain in layerData.layerTerrainData)
        {
            if (layerTerrain.terrainData == selectedTerrain)
            {
                Debug.LogWarning("Ore already exists in layer.");
                alreadyExists = true;
                break;
            }
        }

        // Only add if it's not already present
        if (!alreadyExists)
        {
            LayerTerrainData newLayerTerrainData = ScriptableObject.CreateInstance<LayerTerrainData>();
            newLayerTerrainData.terrainData = selectedTerrain;
            newLayerTerrainData.layer = layerData;

            // Add to assets
            string assetName = "ltd_" + layerData.name + "_" + selectedTerrain.name;
            string assetPath = LAYER_TER_DATA_PATH + assetName + ".asset";
            AssetDatabase.CreateAsset(newLayerTerrainData, assetPath);
            AssetDatabase.SaveAssets();

            // Add to Layer's LayerTerrainData list
            SerializedProperty layerTerrainDataProp = serializedObject.FindProperty("layerTerrainData");
            layerTerrainDataProp.InsertArrayElementAtIndex(layerTerrainDataProp.arraySize);
            SerializedProperty newElement = layerTerrainDataProp.GetArrayElementAtIndex(layerTerrainDataProp.arraySize - 1);
            newElement.objectReferenceValue = newLayerTerrainData;



        }
    }
}
