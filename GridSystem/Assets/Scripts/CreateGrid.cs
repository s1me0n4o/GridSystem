using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField] private Transform PathQuad;
    [SerializeField] private Transform WallQuad;
    [SerializeField] private Vector3 Offset = new Vector3(0.5f, 0.5f);

    [field: SerializeField] private Transform _parent;
    [field: SerializeField] private int _cellSize;
    [field: SerializeField] private int _gridX;
    [field: SerializeField] private int _gridY;
    private Camera _camera;
    private float _cameraZ;
    private LineDrawer _lineDrawer;
    private PathFinding _pf;

    private List<Transform> _pathGameObjects = new List<Transform>();
    private Dictionary<string, Transform> _walls = new Dictionary<string, Transform>();
    private bool _isDestroying = false;

    //private Grid<VisualGridObject> _grid;

    private void Start()
    {
        _camera = Camera.main;
        _cameraZ = Mathf.Abs(_camera.transform.position.z);

        _lineDrawer = new LineDrawer();

        //_grid = new Grid<VisualGridObject>(_gridX, _gridY, _cellSize, Vector3.zero, _parent, 
        //    (Grid<VisualGridObject> grid, int x, int y) 
        //        => new VisualGridObject(grid, x, y));

        _pf = new PathFinding(_gridX, _gridY, _cellSize, _parent);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isDestroying)
        {
            var mousePos = GetMouseWorldPosition();
            _pf.GetGrid().GetXandY(mousePos, out int x, out int y);
            var path = _pf.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    //Debug.DrawLine(new Vector3(path[i].X, path[i].Y) * 10f + Vector3.one * 5f,
                    //    new Vector3(path[i+1].X, path[i+1].Y) * 10f + Vector3.one * 5f, 
                    //    Color.green);

                    //_lineDrawer.DrawLineInGameView(new Vector3(path[i].X, path[i].Y), 
                    //    new Vector3(path[i + 1].X, path[i + 1].Y), Color.green, 
                    //        path.Count, i, i+1,
                    //        new Vector3(0.5f, 0.5f));

                    var go = Instantiate(PathQuad, this.gameObject.transform);
                    go.transform.position = new Vector3(path[i].X, path[i].Y) + Offset;
                    _pathGameObjects.Add(go);
                    Debug.Log($"{path[i]}");
                }
            }

            StartCoroutine(DestroyPath());
        }

        if (Input.GetMouseButtonDown(1))
        {
            var mousePos = GetMouseWorldPosition();
            GetNode(mousePos, out int x, out int y, out PathNode node);
            
            if (!node.IsWalkable)
            {
                _walls.TryGetValue($"{x},{y}", out var obj);
                if (obj != null)
                {
                    Destroy(obj.gameObject);
                    _walls.Remove($"{x},{y}");
                }
            }
            else
            {
                var go = Instantiate(WallQuad, this.gameObject.transform);
                go.transform.position = new Vector3(x, y) + Offset;
                _walls.Add($"{x},{y}", go.transform);
            }
            node.SetWalkable(!node.IsWalkable);

        }
    }

    private void GetNode(Vector3 mousePos, out int x, out int y, out PathNode node)
    {
        var grid = _pf.GetGrid();
        grid.GetXandY(mousePos, out x, out y);
        node = grid.GetGridObject(x, y);
    }

    private IEnumerator DestroyPath()
    {
        _isDestroying = true;
        yield return new WaitForSeconds(2f);

        foreach (var item in _pathGameObjects)
        {
            Destroy(item.gameObject);
            yield return new WaitForSeconds(.3f);
        }
        _pathGameObjects.Clear();
        _isDestroying = false;
    }

    /// <summary>
    /// Uncomment if you want to change the values of in the grid objects
    /// </summary>
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        var mousePos = GetMouseWorldPosition();
    //        var obj = _grid.GetGridObject(mousePos);
    //        if (obj != null)
    //        {
    //            obj.AddValue(5);
    //        }
    //    }

    //}

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cameraZ));
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
