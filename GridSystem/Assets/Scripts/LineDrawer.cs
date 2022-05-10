using UnityEngine;

public struct LineDrawer
{
    private LineRenderer _lineRenderer;
    private float _lineSize;

    public LineDrawer(float lineSize = 0.2f)
    {
        var lineObj = new GameObject("LineObj");
        _lineRenderer = lineObj.AddComponent<LineRenderer>();
        //Particles/Additive
        _lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        _lineSize = lineSize;
    }

    private void Init(float lineSize = 0.2f)
    {
        if (_lineRenderer == null)
        {
            var lineObj = new GameObject("LineObj");
            _lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            _lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            _lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color, int positions, int startPos, int endPos, Vector3 offset)
    {
        if (_lineRenderer == null)
        {
            Init(0.2f);
        }

        //Set color
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;

        //Set width
        _lineRenderer.startWidth = _lineSize;
        _lineRenderer.endWidth = _lineSize;

        //Set line count which is 2
        _lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        _lineRenderer.positionCount = positions;
        _lineRenderer.SetPosition(startPos, start + offset);
        _lineRenderer.SetPosition(endPos, end + offset);
    }

    public void Destroy()
    {
        if (_lineRenderer != null)
        {
            UnityEngine.Object.Destroy(_lineRenderer.gameObject);
        }
    }
}
