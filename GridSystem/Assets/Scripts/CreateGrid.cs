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
    private Grid<Node> _grid;

    private void Start()
    {
        _camera = Camera.main;
        _cameraZ = Mathf.Abs(_camera.transform.position.z);
        _grid = new Grid<Node>(_gridX, _gridY, _cellSize, Vector3.zero, _parent, (Grid<Node> grid, int x, int y) => new Node(grid, x, y));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = GetMouseWorldPosition();
            Node gridNode = _grid.GetGridObject(mousePos);
            if (gridNode != null)
            {
                _grid.SetGridObject(mousePos, gridNode);
                gridNode.SetIsWalkable(!gridNode.IsWalkable());
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
