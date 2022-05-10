using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    private Grid<PathNode> _grid;
    private List<PathNode> _openList;
    private List<PathNode> _closedList;

    public PathFinding(int w, int h, int cs, Transform parent)
    {
        _grid = new Grid<PathNode>(w, h, cs, Vector3.zero, parent, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public Grid<PathNode> GetGrid()
    {
        return _grid;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        var startNode = _grid.GetGridObject(startX, startY);
        var endNode = _grid.GetGridObject(endX, endY);

        _openList = new List<PathNode>();
        _closedList = new List<PathNode>();

        _openList.Add(startNode);

        for (int x = 0; x < _grid.GetGridWidth(); x++)
        {
            for (int y = 0; y < _grid.GetGridHeight(); y++)
            {
                PathNode pathNode = _grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalcFCost();
                pathNode.NodeCameFrom = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalcDistanceHCost(startNode, endNode);
        startNode.CalcFCost();

        while (_openList.Count > 0)
        {
            var currentNode = GetLowestFCostNode(_openList);

            if (currentNode == endNode)
            {
                // Found final node
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            var neighbours = GetNeighbourNodes(currentNode);
            foreach (var neighbourNode in neighbours)
            {
                if (neighbourNode == null)
                    continue;

                if (_closedList.Contains(neighbourNode))
                    continue;

                if (!neighbourNode.IsWalkable)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

                var tentitiveGCost = currentNode.gCost + CalcDistanceHCost(currentNode, neighbourNode);
                if (tentitiveGCost < neighbourNode.gCost)
                {
                    neighbourNode.NodeCameFrom = currentNode;
                    neighbourNode.gCost = tentitiveGCost;
                    neighbourNode.hCost = CalcDistanceHCost(neighbourNode, endNode);
                    neighbourNode.CalcFCost();

                    if (!_openList.Contains(neighbourNode))
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }

        }

        // Out of nodes in open list
        return null;
    }

    private int CalcDistanceHCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        var lowestCostPathNode = pathNodes[0];

        for (int i = 1; i < pathNodes.Count; i++)
        {
            var tempPathNode = pathNodes[i];
            if (tempPathNode.fCost < lowestCostPathNode.fCost)
            {
                lowestCostPathNode = tempPathNode;
            }
        }
        return lowestCostPathNode;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {

        var path = new List<PathNode>();
        path.Add(endNode);
        var currentNode = endNode;
        while (currentNode.NodeCameFrom != null)
        {
            path.Add(currentNode.NodeCameFrom);
            currentNode = currentNode.NodeCameFrom;
        }

        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighbourNodes(PathNode currentNode)
    {
        var neighbours = new List<PathNode>();

        if (currentNode.X -1 >= 0)
        {
            // Left
            neighbours.Add(GetNode(currentNode.X - 1, currentNode.Y));
            // Left down
            if (currentNode.Y - 1 >= 0)
            {
                neighbours.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
            }
            // Left up
            if (currentNode.Y + 1 <= _grid.GetGridHeight())
            {
                neighbours.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
            }
        }

        if (currentNode.X + 1 <= _grid.GetGridWidth())
        {
            // Right
            neighbours.Add(GetNode(currentNode.X + 1, currentNode.Y));
            // Right down
            if (currentNode.Y - 1 >= 0)
            {
                neighbours.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
            }
            // Right up
            if (currentNode.Y + 1 <= _grid.GetGridHeight())
            {
                neighbours.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
            }
        }

        // Down
        if (currentNode.Y - 1 >= 0)
        {
            neighbours.Add(GetNode(currentNode.X, currentNode.Y - 1));
        }
        // Up
        if (currentNode.Y + 1 < _grid.GetGridHeight())
        {
            neighbours.Add(GetNode(currentNode.X, currentNode.Y + 1));
        }

        return neighbours;
    }

    private PathNode GetNode(int x, int y)
    {
        return _grid.GetGridObject(x, y);
    }
}
