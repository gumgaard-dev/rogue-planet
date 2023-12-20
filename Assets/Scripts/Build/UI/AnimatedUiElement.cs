using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Capstone.Build.UI
{
    [System.Serializable]
    public class AnimatedUiElement : MonoBehaviour
    {

        public float TimeBetweenFrames;
        public List<Sprite> Frames;
        private Image _spriteDisplayer;

        private float _timeSinceLastFrame;
        private int _nextFrameIndex;

        [ExecuteAlways]
        private void Awake()
        {
            TryGetComponent(out _spriteDisplayer);
        }

        [ExecuteAlways]
        void Update()
        {
            _timeSinceLastFrame += Time.deltaTime;
            if (_timeSinceLastFrame >= TimeBetweenFrames)
            {
                // reset timer
                _timeSinceLastFrame = 0;

                // display next frame
                _spriteDisplayer.sprite = Frames[_nextFrameIndex];
                _spriteDisplayer.SetNativeSize();

                // reset to first frame if necessary
                _nextFrameIndex = _nextFrameIndex >= Frames.Count - 1 ? 0 : _nextFrameIndex + 1;
            }

        }
    }
}
