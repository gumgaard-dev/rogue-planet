using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Capstone.World.WIP
{
    public class TileMapPopulator : MonoBehaviour
    {
        [SerializeField] private Tilemap _tileMap;
        // Start is called before the first frame update
        private void Start()
        {
            if (this._tileMap == null)
            {
                Debug.Log("No TileMap set in TileMapPopulator");
            }
        }
        void PopulateGridFromEncodedMap(int[][] encodedMap)
        {
            if (this._tileMap)
            {
                Dictionary<int, TerrainData> terrainDecoder = TerrainDB.getTerrainIDDict();
                for (int y = 0; y < encodedMap.GetLength(0); y++)
                {
                    for (int x = 0; x < encodedMap.GetLength(1); x++)
                    {
                        if (terrainDecoder.TryGetValue(encodedMap[y][x], out TerrainData terrainData))
                        {

                        }
                    }
                }
            }
        }
    }

}
