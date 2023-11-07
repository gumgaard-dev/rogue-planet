using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Build.Weapon
{
    public class Beam : MonoBehaviour
    {
        [SerializeField] private float _beamDistance = 50;
        private LineRenderer _beamLineRenderer;
        private void Awake()
        {
            if (!TryGetComponent(out _beamLineRenderer))
            {
                Debug.LogWarning("No beam attached.");
            }
        }

        public void StartFiring()
        {
            this._beamLineRenderer.enabled = true;
        }

        public void StopFiring()
        {
            this._beamLineRenderer.enabled = false;
        }
    }
}
