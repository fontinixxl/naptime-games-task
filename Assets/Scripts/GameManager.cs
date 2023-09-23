using System;
using ObjectPool;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private Spawner _spawner;

        private void Awake()
        {
            _spawner = GetComponent<Spawner>();
        }

        private void Start()
        {
            // Allocate memory and create pooled GameObjects
            PoolManager.Instance.InstantiatePool();
            _spawner.SpawnObjects();
        }
    }
}