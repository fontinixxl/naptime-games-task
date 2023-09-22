using ObjectPool;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Speed at which the projectile moves
    [SerializeField] private float lifeTime = 2f; // Time after which the projectile will be destroyed

    private float _activationTime;
    
    private void OnEnable()
    {
        // Return object to the pool after lifeTime
        _activationTime = Time.time;
    }
    
    void Update()
    {
        if (HasLifeTimeElapsed())
        {
            DeactivateProjectile();
        }
        
        // Move the projectile in the forward direction
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void DeactivateProjectile()
    {
        if (TryGetComponent<PooledGameObject>(out var pooledGameObject))
        {
            pooledGameObject.OnRelease();
        }
    }

    private bool HasLifeTimeElapsed() => Time.time - _activationTime >= lifeTime;
}