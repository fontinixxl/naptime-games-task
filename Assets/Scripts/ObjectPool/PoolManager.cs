using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class PoolManager : Singleton<PoolManager>
    {
        // Default pool size for each pool item
        public static int DEFAULT_POOL_COUNT = 1;
    
        public Action OnCleanup;
        public Action OnRelease;

        // Data Setup
        [SerializeField] private List<PoolableItemDefinition> poolableObjects = new();
        // Collection checks will throw errors if we try to release an item that is already in the pool.
        [SerializeField] private bool collectionChecks = true;
        // Maximum collection size allocated per pooled object
        [SerializeField] private int maxPoolSize = 10;

        public Dictionary<string, IObjectPool<PooledGameObject>> Pool { get; private set; }
        public bool IsReady { get; private set; }
    
        public PooledGameObject Spawn(string id, Transform parent = null)
        {
            if (!Pool.TryGetValue(id, out var objectPool)) return null;
            var pooledGO = objectPool.Get();
            if (parent)
            {
                pooledGO.transform.SetParent(parent);
            }

            return pooledGO;
        }
    
        public void InstantiatePool()
        {
            if (IsReady)
            {
                return;
            }

            Pool = DictionaryPool<string, IObjectPool<PooledGameObject>>.Get();

            // Memory allocation and initialize GameObject
            poolableObjects.ForEach(
                item =>
                {
                    // Create stack in dictionary
                    CreateObjectsPool(item);
                    InitializeObjectsPool(item, item.PoolCount);
                }
            );

            // Disable all pooled objects
            OnRelease?.Invoke();
            IsReady = true;
        }

        public void CleanupPool(bool garbageCollect = true)
        {
            if (!IsReady)
            {
                return;
            }

            // Destroy all game objects and release allocated memory
            OnCleanup?.Invoke();
            DictionaryPool<string, IObjectPool<PooledGameObject>>.Release(Pool);

            IsReady = false;
            OnCleanup = null;

            if (garbageCollect)
            {
                GC.Collect();
            }
        }

        public void ReleasePooledObject()
        {
            if (!IsReady)
            {
                return;
            }

            OnRelease?.Invoke();
        }

        private void CreateObjectsPool(PoolableItemDefinition item)
        {
            if (!item.Prefab)
            {
                throw new Exception("Pool Item prefab is null");
            }
            
            // Debug.LogFormat($"Adding new Poolable {item.Name} with id {id}");

            var id = item.Name;
            // Game object doesn't get created at this point, we need to call pool.Get() to do so
            Pool.Add(
                id,
                new ObjectPool<PooledGameObject>(
                    () => CreatePooledItem(item.Prefab),
                    OnGetFromPool,
                    OnReleaseToPool,
                    OnDestroyPoolObject,
                    collectionChecks,
                    item.PoolCount,
                    maxPoolSize
                )
            );
        }

        private void InitializeObjectsPool(PoolableItemDefinition item, int count)
        {
            if (!item.Prefab)
            {
                throw new Exception("Pool Item prefab is null");
            }

            var id = item.Prefab.name;
            if (!Pool.TryGetValue(id, out var objectPool))
            {
                CreateObjectsPool(item);
                objectPool = Pool[id];
            }

            // Object shouldn't be release at this point else it will just take the same item from the top of the stack
            for (var i = 0; i < count; ++i)
            {
                objectPool.Get();
            }
        }

        private PooledGameObject CreatePooledItem(GameObject prefab)
        {
            var go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            go.name = prefab.name;

            var pooledGo = go.AddComponent<PooledGameObject>();
            pooledGo.Initialize(this, prefab.name);
            return pooledGo;
        }

        private void OnGetFromPool(PooledGameObject pooledGO)
        {
            pooledGO.SetActive(true);
        }

        private void OnReleaseToPool(PooledGameObject releaseGO)
        {
            releaseGO.transform.SetParent(transform);
            releaseGO.SetActive(false);
        }

        private void OnDestroyPoolObject(PooledGameObject destroyedGO)
        {
            Destroy(destroyedGO.gameObject);
        }
    }
}