using System;
using ObjectPool;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Shooter : MonoBehaviour
{
    public event Action<Shooter> OnHit;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PoolableItemDefinition projectileItemDef;
    [SerializeField] private float fireCoolDownTime = 1.0f;

    private Material _material;
    private int _lives;
    private float _elapsedTimeFire; // since last projectile was fired

    public int Lives => _lives;
    public float RespawnElapsedTime { get; set; }
    public Vector2Int GridPosition { get; set; }

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _lives = 3;
        _elapsedTimeFire = fireCoolDownTime * .5f;
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        ApplyColorBasedOnLives();
#endif
    }

    private void Update()
    {
        _elapsedTimeFire += Time.deltaTime;
        if (_elapsedTimeFire >= fireCoolDownTime)
        {
            SpawnProjectile();
            _elapsedTimeFire = 0;
        }
    }

    private void SpawnProjectile()
    {
        var pooledTransform = PoolManager.Instance.Spawn(projectileItemDef.Name).transform;
        pooledTransform.position = spawnPoint.position;
        pooledTransform.rotation = transform.rotation;
    }

    private void ApplyColorBasedOnLives()
    {
        _material.color = _lives switch
        {
            3 => Color.green,
            2 => Color.yellow,
            1 => Color.red,
            _ => _material.color
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        // If for whatever reason is not a projectile, exit
        if (!other.TryGetComponent(out Projectile projectile))
        {
            Debug.LogWarning("Collider does not have a Projectile Component!", other);
            return;
        }

        projectile.Deactivate();

        _lives--;
        OnHit?.Invoke(this);
    }
}