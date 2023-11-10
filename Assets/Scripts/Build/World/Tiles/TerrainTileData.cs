
using Capstone.Build.World;

namespace Capstone.Build.World
{
    public class TerrainTileData
    {
        private TerrainTile _terrainTileType;
        public TerrainTile TerrainTileType
        { 
            get 
            {
                return _terrainTileType;
            } 
            set 
            { 
                _terrainTileType = value;
                CurrentHealth = _terrainTileType.BaseHealth;
            } 
        }

        private OreTile _oreTile;
        public OreTile OreTile
        {
            get
            {
                return _oreTile;
            }
            set
            {
                _oreTile = value;
                CurrentHealth = (int)(CurrentHealth * _oreTile.TerrainHealthModifier);
            }
        }

        public int CurrentHealth;
        public bool HasOre => OreTile != null;
        // Add other fields as necessary
    }
}