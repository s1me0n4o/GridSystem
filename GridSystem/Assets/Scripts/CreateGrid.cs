using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [field: SerializeField] private Transform _parent;
    [field: SerializeField] private int _cellSize;
    [field: SerializeField] private int _gridX;
    [field: SerializeField] private int _gridY;
    private Camera _camera;
    private float _cameraZ;
    private Grid<VisualGridObject> _grid;

    private void Start()
    {
        _camera = Camera.main;
        _cameraZ = Mathf.Abs(_camera.transform.position.z);
        _grid = new Grid<VisualGridObject>(_gridX, _gridY, _cellSize, Vector3.zero, _parent, 
            (Grid<VisualGridObject> grid, int x, int y) 
                => new VisualGridObject(grid, x, y));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = GetMouseWorldPosition();
            var obj = _grid.GetGridObject(mousePos);
            if (obj != null)
            {
                obj.AddValue(5);
            }
        }

    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cameraZ));
        Debug.Log(worldPos);
        return worldPos;
    }

}

public class VisualGridObject
{
    public int Value;

    private const int MIN = 0;
    private const int MAX = 100;
    private Grid<VisualGridObject> _grid;
    private int _x;
    private int _y;

    public VisualGridObject(Grid<VisualGridObject> grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;
    }

    public void AddValue(int v)
    {
        Value += v;
        Mathf.Clamp(Value, MIN, MAX);
        _grid.TriggerOnGridObjectChanged(_x, _y);
    }

    public float GetNormalizedValue()
    {
        return (float)Value / MAX;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
