using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Map map = (Map)target;

        if (DrawDefaultInspector()) // This will draw the default inspector properties and layout.
        {
            if (map.autoUpdate)
            {
                map.GenerateNoiseMap();
            }
        }

        if (GUILayout.Button("Show Noise Map"))
        {
            map.GenerateNoiseMap();
        }

        DrawNoiseMapPreview(map);
    }

    void DrawNoiseMapPreview(Map map)
    {
        float[,] noiseMap = map.GetNoiseMap();
        if (noiseMap != null)
        {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            Texture2D texture = new Texture2D(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float value = noiseMap[x, y];
                    Color color = new Color(value, value, value, 1f);  // Grayscale color
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            int previewSize = 256;
            GUILayout.Label(texture, GUILayout.Width(previewSize), GUILayout.Height(previewSize));
        }
    }
}
