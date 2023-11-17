using UnityEngine;

namespace Capstone.Build.World
{
    public class ItemSpawner : MonoBehaviour
    {
        public static void SpawnOre(OreCollectable collectable, Vector3 position)
        {
            Instantiate(collectable, position, Quaternion.identity);
        }
    }
}

