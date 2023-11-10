/*using Capstone.Build.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Capstone.EditorScripts
{
    [CustomEditor(typeof(TerrainTile))]
    public class TerrainTileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            TerrainTile tile = (TerrainTile)target;

            if (tile.m_DefaultSprite != null)
            {
                Texture2D newIcon = new Texture2D(width, height, TextureFormat.ARGB32, false);
                Texture2D spritePreview = AssetPreview.GetAssetPreview(tile.m_DefaultSprite);
                EditorUtility.CopySerialized(spritePreview, newIcon);
                EditorUtility.SetDirty(tile);
                return newIcon;
            }

            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }
    }
}
*/
