using UnityEngine;
using Random = UnityEngine.Random;

// Rotates GO a random amount at random intervals
namespace Fontinixxl.NaptimeGames
{
    public class Rotator : MonoBehaviour
    {
        private float _timeToNextRotation;
        private float _randomRotation;

        private void Start()
        {
            ScheduleNextRotation();
        }

        private void Update()
        {
            _timeToNextRotation -= Time.deltaTime;
            if (_timeToNextRotation > 0) return;
        
            transform.Rotate(Vector3.up * _randomRotation);
            ScheduleNextRotation();
        }

        private void ScheduleNextRotation()
        {
            _timeToNextRotation = Random.value; // 0 - 1s
            _randomRotation = Random.Range(1f, 360f);
        }
    }
}
