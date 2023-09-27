using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayerData))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LayerData layerData = (LayerData)target;

        if (GUILayout.Button("Repopulate Layer Terrain Data"))
        {
            RepopulateLayerTerrainData(layerData);
        }
    }

    private void RepopulateLayerTerrainData(LayerData layerData)
    {
        // Assuming the oreDatabase field in LayerData is already populated
        if (layerData.oreDatabase != null)
        {
            // Clear the existing LayerTerrainData entries if any
            layerData.layerTerrainData.Clear();

            // Loop through the TerrainData array in the TerrainDatabase
            foreach (var terrain in layerData.oreDatabase.GetTerrainData())
            {
                LayerTerrainData newLayerTerrainData = new LayerTerrainData
                {
                    oreData = terrain,
                    // Assign default values or compute values for other fields as needed
                    spawnThreshold = 0.5f,
                    // ... and so on for other fields
                };

                // Add the newly created LayerTerrainData object to the list
                layerData.layerTerrainData.Add(newLayerTerrainData);
            }

            // Notify Unity of the data change to ensure it's saved properly
            EditorUtility.SetDirty(layerData);
        }
        else
        {
            Debug.LogWarning("Terrain Database is not set!");
        }
    }
}
