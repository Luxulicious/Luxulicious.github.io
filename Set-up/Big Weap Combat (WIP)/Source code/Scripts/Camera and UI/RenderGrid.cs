using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[ExecuteAlways]
public class RenderGrid : MonoBehaviour
{
    private static Material _lineMaterial;
    private List<Vector3> _pointsToDrawLabelsAt = new List<Vector3>();
    private Vector3? _prevPosition = null;

    [SerializeField] private Vector2 cellSize = new Vector2(10, 10);
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(10, 10);
    [SerializeField] private bool _showMeasurements = true;
    [SerializeField] private Color _color = new Color(1, 1, 1);
    [SerializeField] private bool _renderInEditMode = false;
    [SerializeField] private Vector2 offSet = new Vector2(5, 5);
    [SerializeField] public GUIStyle _guiStyle = new GUIStyle();

    //TODO OnGUI is actually like old and badly optimized replace it with constant texts
    void OnGUI()
    {
        if (!(_showMeasurements && (Application.isPlaying || (Application.isEditor && _renderInEditMode)))) return;
        if (_prevPosition != this.transform.position)
            _pointsToDrawLabelsAt = GetPoints();
        _prevPosition = this.transform.position;

        _guiStyle.normal.textColor = _color;
        foreach (var point in _pointsToDrawLabelsAt)
        {
            var content = new GUIContent("x: " + point.x + " y:" + point.y);
            var labelSize = GUI.skin.label.CalcSize(content);
            var worldPoint = this.transform.position + point;
            var adjustedPoint = new Vector2(worldPoint.x, worldPoint.y);
            var canvasPos = Camera.main.WorldToScreenPoint(adjustedPoint);
            var adjustedCanvasPos = canvasPos + new Vector3(0, labelSize.y);
            adjustedCanvasPos = new Vector2(adjustedCanvasPos.x + offSet.x,
                Screen.height - adjustedCanvasPos.y - offSet.y);
            GUI.Label(new Rect(adjustedCanvasPos, labelSize), content, _guiStyle);
        }
    }

    private List<Vector3> GetPoints()
    {
        var points = new List<Vector3>();
        for (int x = 0; x < _gridSize.x; ++x)
        {
            for (int y = 0; y < _gridSize.y; ++y)
            {
                points.Add(new Vector2(x * cellSize.x, y * cellSize.y));
            }
        }

        return points;
    }

    // Update is called once per frame
    void OnRenderObject()
    {
        if (!(Application.isPlaying || (Application.isEditor && _renderInEditMode))) return;
        if (!_lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            _lineMaterial = new Material(shader);
            _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            _lineMaterial.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
            _lineMaterial.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            _lineMaterial.SetInt("_Cull", (int) UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            _lineMaterial.SetInt("_ZWrite", 0);
        }

        // Apply the line material
        _lineMaterial.SetPass(0);
        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(this.transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);
        GL.Color(_color);
        DrawGrid();
        GL.End();
        GL.PopMatrix();
    }

    private void DrawGrid()
    {
        for (int x = 0; x < _gridSize.x + 1; ++x)
        {
            var a = new Vector3(x * cellSize.x, 0);
            GL.Vertex3(a.x, a.y, a.z);
            var b = new Vector3(x * cellSize.x, _gridSize.y * cellSize.y);
            GL.Vertex3(b.x, b.y, b.z);
        }

        for (int y = 0; y < _gridSize.y + 1; ++y)
        {
            var a = new Vector3(0, y * cellSize.y);
            GL.Vertex3(a.x, a.y, a.z);
            var b = new Vector3(cellSize.x * _gridSize.x, y * cellSize.y);
            GL.Vertex3(b.x, b.y, b.z);
        }
    }
}