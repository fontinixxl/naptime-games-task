using System;
using ObjectPool;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            // Allocate memory and create pooled GameObjects
            PoolManager.Instance.InstantiatePool();
        }
    }
}