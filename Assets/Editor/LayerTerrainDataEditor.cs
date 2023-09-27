using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayerTerrainData))]
public class LayerTerrainDataEditor : Editor
{
    public void DrawLayerTerrainDataGUI(LayerTerrainData layerTerrainData)
    {
        // Draw noise map preview
        if (layerTerrainData.noiseMap != null)
        {
            Texture2D noiseMapTexture = layerTerrainData.GenerateNoiseMapTexture();
            GUILayout.Label(noiseMapTexture);
        }
        else
        {
            EditorGUILayout.HelpBox("Noise Map is not generated yet.", MessageType.Info);
        }
    }

    public override void OnInspectorGUI()
    {
        LayerTerrainData layerTerrainData = target as LayerTerrainData;
        if (layerTerrainData != null)
        {
            DrawLayerTerrainDataGUI(layerTerrainData);
        }
    }
}

