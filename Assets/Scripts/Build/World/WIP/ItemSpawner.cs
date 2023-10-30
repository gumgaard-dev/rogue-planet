using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.WIP
{
    public class ItemSpawner : MonoBehaviour
    {
        public void SpawnOre(OreCollectable collectable, Vector3 position)
        {
            Instantiate(collectable, position, Quaternion.identity);
        }
    }
}

