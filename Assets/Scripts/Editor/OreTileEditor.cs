using Capstone.Build.World;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Capstone.EditorScripts
{
    [CustomEditor(typeof(OreTile))]
    public class OreTileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            OreTile tile = (OreTile)target;
            tile.DefaultSprite = tile.sprite;
            EditorUtility.SetDirty(tile);
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            Tile tile = (Tile)target;

            if (tile.sprite != null)
            {
                Texture2D newIcon = new Texture2D(width, height, TextureFormat.ARGB32, false);
                Texture2D spritePreview = AssetPreview.GetAssetPreview(tile.sprite);
                EditorUtility.CopySerialized(spritePreview, newIcon);
                EditorUtility.SetDirty(tile);
                return newIcon;
            }

            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }
    }
}

