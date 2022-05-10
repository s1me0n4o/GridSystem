public class PathNode
{
    public PathNode NodeCameFrom;

    public int gCost;
    public int hCost;
    public int fCost;

    private Grid<PathNode> _grid;
    private int _x;
    private int _y;
    private bool _isWallkable = true;

    public int X => _x;
    public int Y => _y;
    public bool IsWalkable => _isWallkable;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;
    }

    public override string ToString()
    {
        return _x + "," + _y;
    }

    public void CalcFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetWalkable(bool isWalkable)
    {
        _isWallkable = isWalkable;
        _grid.TriggerOnGridObjectChanged(_x, _y);
    }
}
