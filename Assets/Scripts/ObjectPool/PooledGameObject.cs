using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    /// <summary>
    /// Component that will be automatically added to any GO that is Poolable
    /// </summary>
    public class PooledGameObject : MonoBehaviour
    {
        private IObjectPool<PooledGameObject> _pool;
        private PoolManager _poolManager;

        public string Id { get; private set; }
        public bool IsRelease { get; private set; }

        public void Initialize(PoolManager poolMgr, string id)
        {
            _pool = poolMgr.Pool[id];
            _poolManager = poolMgr;
            Id = id;
            _poolManager.OnCleanup += OnCleanup;
            _poolManager.OnRelease += OnRelease;
        }

        public void SetActive(bool active)
        {
            // Sets gameObject ready to used
            gameObject.SetActive(active);
            IsRelease = !active;
        }

        public void OnRelease()
        {
            if (IsRelease)
            {
                return;
            }

            // Return object to pool
            _pool.Release(this);
        }

        private void OnDestroy()
        {
            // gameObject gets destroyed unregister from pool
            if (_poolManager)
            {
                _poolManager.OnCleanup -= OnCleanup;
                _poolManager.OnRelease -= OnRelease;
            }
        }

        private void OnCleanup()
        {
            // Called when pool manager gets cleanup
            OnRelease();
            Destroy(gameObject);
        }
    }
}