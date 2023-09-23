using ObjectPool;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Speed at which the projectile moves
    [SerializeField] private float lifeTime = 2f; // Time after which the projectile will be destroyed

    private float _activationTime;

    private void OnEnable()
    {
        _activationTime = Time.time;
    }

    private void Update()
    {
        if (HasLifeTimeElapsed())
        {
            Deactivate();
        }

        // Move the projectile in the forward direction
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    public void Deactivate()
    {
        if (TryGetComponent<PooledGameObject>(out var pooledGameObject))
        {
            pooledGameObject.OnRelease();
        }
    }

    private bool HasLifeTimeElapsed() => Time.time - _activationTime >= lifeTime;
}