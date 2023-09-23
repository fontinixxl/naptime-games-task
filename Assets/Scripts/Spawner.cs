using UnityEngine;
using System.Collections.Generic;
using ObjectPool;

public class Spawner : MonoBehaviour
{
    public int objectCount = 50; // TODO: Remove once UI is done
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PoolableItemDefinition itemToSpawn;
    // [SerializeField] private int cellSize = 1;
    [SerializeField] private float padding = 0.2f;
    
    private List<Vector2Int> _availableCells = new();
    private float _cellHeight;
    private float _cellWidth;
    private int _rows;
    private int _columns;

    private void Start()
    {
        CalculateGridSize();
        InitializeAvailableCells();
        CalculateCellOffsets();
    }

    public void SpawnObjects()
    {
        for (var i = 0; i < objectCount; i++)
        {
            if (_availableCells.Count <= 0) return;
            
            var randomIndex = Random.Range(0, _availableCells.Count);
            var spawnCell = _availableCells[randomIndex];

            // Calculate position
            var xPos = -mainCamera.aspect * mainCamera.orthographicSize + spawnCell.x * (_cellWidth + padding) + _cellWidth / 2;
            var yPos = -mainCamera.orthographicSize + spawnCell.y * (_cellHeight + padding) + _cellHeight / 2;

            var pooledGameObject = PoolManager.Instance.Spawn(itemToSpawn.Name);
            var gridWorldPos = new Vector3(xPos, 0, yPos);
            pooledGameObject.transform.position = gridWorldPos;

            _availableCells.RemoveAt(randomIndex);
        }
    }
    
    private void CalculateCellOffsets()
    {
        _cellWidth = (mainCamera.aspect * mainCamera.orthographicSize * 2 - (_columns - 1) * padding) / _columns;
        _cellHeight = (mainCamera.orthographicSize * 2 - (_rows - 1) * padding) / _rows;
    }

    private void InitializeAvailableCells()
    {
        for (var row = 0; row < _rows; row++)
        {
            for (var column = 0; column < _columns; column++)
            {
                _availableCells.Add(new Vector2Int(column, row));
            }
        }
    }

    private void CalculateGridSize()
    {
        // Calculate aspect ratio
        var aspectRatio = (float)Screen.width / Screen.height;

        if (aspectRatio >= 1) // Landscape
        {
            _columns = Mathf.FloorToInt(Mathf.Sqrt(objectCount * aspectRatio));
            _rows = objectCount / _columns;
            if (objectCount % _columns > 0) _rows++;
        }
        else // Portrait
        {
            _rows = Mathf.FloorToInt(Mathf.Sqrt(objectCount / aspectRatio));
            _columns = objectCount / _rows;
            if (objectCount % _rows > 0) _columns++;
        }
    }

}
