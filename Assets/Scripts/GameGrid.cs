using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private int _height = 27;
    [SerializeField] private int _width = 10;
    private float _gridSpaceSize = 1f;

    


    [SerializeField] private GameObject _gridCellPrefab;
    private GameObject[,] _gameGrid;
    public Vector2Int GetPosition()
    {
        return new Vector2Int(_width,_height);
    }

    void Start()
    {

        CreateGrid();
        
    }

    private void CreateGrid()
    {
        _gameGrid = new GameObject[_width, _height];
        if(_gridCellPrefab == null)
        {
            Debug.LogError("ERROR: Grid Cell Prefab not assigned");
        }

        for(int y = 0; y <_height ; y++)
        {
            for(int x=0; x < _width; x++)
            {
                _gameGrid[x, y] = Instantiate(_gridCellPrefab, new Vector3(x * _gridSpaceSize, y * _gridSpaceSize), Quaternion.identity);
                _gameGrid[x, y].GetComponent<GridCell>().SetPosition(x, y);
                _gameGrid[x, y].transform.parent = transform;
                _gameGrid[x, y].gameObject.name = "Grid Space (X: " + x.ToString() + ", Y:" + y.ToString() + ")";
            }
        }
    }
    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / _gridSpaceSize);
        int y = Mathf.FloorToInt(worldPosition.z / _gridSpaceSize);

        x = Mathf.Clamp(x, 0, _width);
        y = Mathf.Clamp(x,0,_height);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        float x = gridPos.x * _gridSpaceSize;
        float y = gridPos.y * _gridSpaceSize;

        return new Vector3(x,0, y);

    }
}
