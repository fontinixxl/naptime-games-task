using System;
using System.Collections;
using ObjectPool;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Shooter : MonoBehaviour
{
    public event Action<Shooter> OnHit;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PoolableItemDefinition projectileItemDef;

    private Material _material;
    private WaitForSeconds _waitOneSecond;
    private int _lives;

    public int Lives => _lives;
    public float CoolDownTime { get; set; }
    public Vector2Int GridPosition { get; set; }

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _lives = 3;
        _waitOneSecond = new WaitForSeconds(1.0f);
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        ApplyColorBasedOnLives();
#endif
        StartCoroutine(SpawnProjectiles());
    }

    private IEnumerator SpawnProjectiles()
    {
        while (gameObject.activeSelf)
        {
            var pooledTransform = PoolManager.Instance.Spawn(projectileItemDef.Name).transform;
            pooledTransform.position = spawnPoint.position;
            pooledTransform.rotation = transform.rotation;
            yield return _waitOneSecond;
        }
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
        if (!other.TryGetComponent(out Projectile projectile)) return;
        projectile.Deactivate();

        _lives--;
        OnHit?.Invoke(this);
    }
}