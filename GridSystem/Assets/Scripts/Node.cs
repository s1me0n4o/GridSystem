using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    private Grid<Node> _grid;
    private int _x;
    private int _y;
    private bool _isWalkable;

    public Node(Grid<Node> grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;
        _isWalkable = true;
    }

    public bool IsWalkable()
    {
        return _isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        _isWalkable = isWalkable;
        _grid.TriggerOnGridObjectChanged(_x, _y);
    }
}
