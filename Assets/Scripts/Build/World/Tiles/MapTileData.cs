
using Capstone.Build.World;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    public class MapTileData
    {
        private TerrainTile _terrainTile;
        private OreTile _oreTile;
        private Tile _backgroundTile;
        public TerrainTile TerrainTile
        {
            get
            {
                return _terrainTile;
            }
            set
            {
                _terrainTile = value;
                if (value != null)
                {
                    CurrentHealth = _terrainTile.BaseHealth;
                    _backgroundTile = _terrainTile.BackgroundTile;
                }

            }
        }


        public OreTile OreTile
        {
            get
            {
                return _oreTile;
            }
            set
            {
                _oreTile = value;
                if (value != null)
                {
                    CurrentHealth = (int)(CurrentHealth * _oreTile.TerrainHealthModifier);
                }
            }
        }

        public Tile BackgroundTile { get { return _backgroundTile; } }

        public int CurrentHealth;
        public bool HasOre => OreTile != null;

        public bool HasTerrain => TerrainTile != null;
    }
}