using TMPro;
using UnityEngine;

namespace Fontinixxl.NaptimeGames.Utils
{
    public class FPSDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fpsText;
        
        private float _deltaTime;
        private readonly float _updateInterval = 0.5f; // Update every 0.5 seconds.
        private float _nextUpdateTime;

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

            // Check if it's time to update the display
            if (Time.unscaledTime > _nextUpdateTime)
            {
                // Calculate the FPS value
                var fps = 1.0f / _deltaTime;
                fpsText.text = $"{fps:0.} fps";

                // Set the next update time
                _nextUpdateTime = Time.unscaledTime + _updateInterval;
            }
        }
    }
}