using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu(fileName = "PoolableItem", menuName = "Pool/Item", order = 0)]
    public class PoolableItemDefinition : ScriptableObject
    {
        [Tooltip("Name is important and will be used to retrieve items from the Pool")]
        public string Name;
        public GameObject Prefab;
        public int PoolCount = PoolManager.DEFAULT_POOL_COUNT;
    }
}