using ObjectPool;
using UnityEngine;

[RequireComponent(typeof(PooledGameObject))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Speed at which the projectile moves
    [SerializeField] private float lifeTime = 2f; // Time after which the projectile will be destroyed

    private PooledGameObject _pooledComponent;
    private float _activationTime;

    private void Awake()
    {
        _pooledComponent = GetComponent<PooledGameObject>();
    }

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

        // Move the 'dummy' parent projectile in the forward directionS
        transform.parent.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    public void Deactivate()
    {
        _pooledComponent.OnRelease();
    }

    private bool HasLifeTimeElapsed() => Time.time - _activationTime >= lifeTime;
}