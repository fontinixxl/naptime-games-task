using System;
using UnityEngine;
using System.Collections.Generic;
using ObjectPool;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager>
{
    public event Action OnGameOver;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PoolableItemDefinition shooterPoolableItem;
    [SerializeField] private float padding = 0.2f;

    // Stores all the objects that have been hit and need to be respawn after 2 seconds
    private List<Shooter> _objectsOnCoolDown;
    private readonly List<Vector2Int> _availablePositions = new();
    private int _objectsAliveCount;
    // Grid data
    private float _cellHeight;
    private float _cellWidth;
    private int _rows;
    private int _columns;
    private int _numObjectSpawn;

    private void Start()
    {
        _objectsOnCoolDown = new List<Shooter>();
    }

    public void StartGame(int numObjectsSpawn)
    {
        PoolManager.Instance.InstantiatePool(); // Allocate memory and create pooled GameObjects
        _numObjectSpawn = numObjectsSpawn;
        _objectsAliveCount = numObjectsSpawn;
        CalculateGridSize();
        InitializeAvailablePositions();
        SpawnObjects();
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
        _objectsOnCoolDown.Clear();
        PoolManager.Instance.CleanupPool();
    }

    private void Update()
    {
        if (_objectsOnCoolDown.Count == 0) return;
        RespawnObjectsOnCoolDown();
    }

    private void RespawnObjectsOnCoolDown()
    {
        for (var i = _objectsOnCoolDown.Count - 1; i >= 0; i--)
        {
            var objectOnCoolDown = _objectsOnCoolDown[i];
            objectOnCoolDown.RespawnElapsedTime -= Time.deltaTime;
            if (objectOnCoolDown.RespawnElapsedTime <= 0)
            {
                var newPosition = MoveObjectToAvailableRandomPosition(objectOnCoolDown.gameObject);
                objectOnCoolDown.GridPosition = newPosition;
                objectOnCoolDown.gameObject.SetActive(true);
                _objectsOnCoolDown.RemoveAt(i);
            }
        }
    }

    private void SpawnObjects()
    {
        for (var i = 0; i < _numObjectSpawn; i++)
        {
            var pooledGameObject = PoolManager.Instance.Spawn(shooterPoolableItem.Name).gameObject;
            var position = MoveObjectToAvailableRandomPosition(pooledGameObject);
            var shooterObject = pooledGameObject.GetComponent<Shooter>();

            shooterObject.GridPosition = position;
            shooterObject.OnHit += OnObjectHit;
        }
    }

    private void OnObjectHit(Shooter hitObject)
    {
        // Disable GameObject
        hitObject.gameObject.SetActive(false);

        if (hitObject.Lives > 0)
        {
            // Add object's current grid position as a available position
            _availablePositions.Add(hitObject.GridPosition);
            hitObject.RespawnElapsedTime = 2f;
            _objectsOnCoolDown.Add(hitObject);
        }
        else
        {
            // It's dead! lol
            _objectsAliveCount--;
            if (_objectsAliveCount <= Mathf.CeilToInt(_numObjectSpawn * 0.1f))
            {
                GameOver();
            }
        }
    }

    private Vector2Int MoveObjectToAvailableRandomPosition(GameObject spawnedObject)
    {
        var randomIndex = Random.Range(0, _availablePositions.Count);
        var spawnCell = _availablePositions[randomIndex];
        // Calculate World position
        var xPos = -mainCamera.aspect * mainCamera.orthographicSize + spawnCell.x * (_cellWidth + padding) +
                   _cellWidth / 2;
        var yPos = -mainCamera.orthographicSize + spawnCell.y * (_cellHeight + padding) + _cellHeight / 2;
        var worldPosition = new Vector3(xPos, 0, yPos);

        spawnedObject.transform.position = worldPosition;
        _availablePositions.RemoveAt(randomIndex);

        return spawnCell;
    }

    private void InitializeAvailablePositions()
    {
        for (var row = 0; row < _rows; row++)
        {
            for (var column = 0; column < _columns; column++)
            {
                _availablePositions.Add(new Vector2Int(column, row));
            }
        }
    }

    private void CalculateGridSize()
    {
        // Calculate aspect ratio
        var aspectRatio = (float)Screen.width / Screen.height;

        if (aspectRatio >= 1) // Landscape
        {
            _columns = Mathf.FloorToInt(Mathf.Sqrt(_numObjectSpawn * aspectRatio));
            _rows = _numObjectSpawn / _columns;
            if (_numObjectSpawn % _columns > 0) _rows++;
        }
        else // Portrait
        {
            _rows = Mathf.FloorToInt(Mathf.Sqrt(_numObjectSpawn / aspectRatio));
            _columns = _numObjectSpawn / _rows;
            if (_numObjectSpawn % _rows > 0) _columns++;
        }

        _cellWidth = (mainCamera.aspect * mainCamera.orthographicSize * 2 - (_columns - 1) * padding) / _columns;
        _cellHeight = (mainCamera.orthographicSize * 2 - (_rows - 1) * padding) / _rows;
    }
}