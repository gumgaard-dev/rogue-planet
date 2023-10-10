using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayerTerrainData))]
public class LayerTerrainDataEditor : Editor
{

    void OnEnable()
    {
        EditorApplication.update += UpdateInspector;
    }

    void OnDisable()
    {
        EditorApplication.update -= UpdateInspector;
    }

    void UpdateInspector()
    {
        Repaint(); // Refresh the inspector
    }

    public void DrawLayerTerrainDataGUI(LayerTerrainData layerTerrainData)
    {
        DrawDefaultInspector();

        // Draw noise map preview
        Texture2D noiseMapTexture = layerTerrainData.GenerateNoiseMapTexture();
        GUILayout.Label(noiseMapTexture);
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