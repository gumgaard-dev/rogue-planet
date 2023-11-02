using UnityEngine;

namespace Capstone.Build.World
{
    public class ItemSpawner : MonoBehaviour
    {
        public void SpawnOre(OreCollectable collectable, Vector3 position)
        {
            Instantiate(collectable, position, Quaternion.identity);
        }
    }
}

