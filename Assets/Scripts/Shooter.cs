using System.Collections;
using ObjectPool;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PoolableItemDefinition projectileItemDef;
    private WaitForSeconds _waitOneSecond;
    
    private void Awake()
    {
        _waitOneSecond = new WaitForSeconds(1.0f);
    }

    private IEnumerator Start()
    {
        while (true)
        {
            var pooledTransform = PoolManager.Instance.Spawn(projectileItemDef.Name).transform;
            pooledTransform.position = spawnPoint.position;
            pooledTransform.rotation = transform.rotation;
            yield return _waitOneSecond;
        }
    }
}
