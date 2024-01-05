using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Capstone.Build.Cam
{
    // use this event to invoke ShakeCamera()
    [System.Serializable] public class CameraShakeEvent : UnityEvent<float, float> { }
    public class CameraShaker : MonoBehaviour
    {
        // using camera container transform instead of accessing camera transform directly
        // this is because manipulating the camera's transform would interfere with other scripts that update the camera's position
        public Transform CamContainerTransform;

        public void ShakeCamera(float duration, float intensity)
        {
            StartCoroutine(ShakeCoroutine(duration, intensity));
        }

        private IEnumerator ShakeCoroutine(float duration, float intensity)
        {

            Vector2 _defaultPosition = CamContainerTransform.localPosition;

            float timeElapsedInShake = 0f;
            while(timeElapsedInShake < duration)
            {
                if (!GameManager.GamePaused)
                {
                    Vector2 curShakeAmount = new(Random.Range(-1f * intensity, intensity), Random.Range(-1f * intensity, intensity));

                    CamContainerTransform.localPosition = _defaultPosition + curShakeAmount;

                    timeElapsedInShake += Time.deltaTime;
                }
                yield return null;
            }

            CamContainerTransform.localPosition = _defaultPosition;
        }
    }
}

