using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Build.Weapon
{
    public class Beam : MonoBehaviour
    {
        [SerializeField] private float _beamDistance = 50;
        public LineRenderer LineRenderer { get; private set; }

        public void FireBeam()
        {

        }
    }
}
