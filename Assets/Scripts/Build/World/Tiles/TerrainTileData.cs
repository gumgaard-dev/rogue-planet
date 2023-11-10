
using Capstone.Build.World;

namespace Capstone.Build.World
{
    public class TerrainTileData
    {
        public OreTile OreTile;
        public int CurrentHealth;
        public bool HasOre => OreTile != null;
        // Add other fields as necessary
    }
}