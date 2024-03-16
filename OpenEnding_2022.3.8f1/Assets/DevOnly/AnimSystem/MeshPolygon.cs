using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshPolygon : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    
    private Mesh _mesh;
    private Vector3[] _polygonPoints;
    private int[] _polygonTriangles;
    
    public PolygonType Type { get; set; }
    
    public enum PolygonType
    {
        Filled,
        Hollowed
    }
    
    public int Sides { get; set; }
    public float Radius { get; set; }
    
    public float InnerRadius { get; set; }
    
    private void Awake()
    {
        Sides = 0;
        Radius = 0f;
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.mesh = _mesh = new Mesh();
    }

    private void Start()
    {
        DrawPolygon(Sides, Radius);
    }

    public void Update()
    {
        if (Sides < 3 || Radius <= 0) return;
    }

    public void DrawPolygon(int sides, float radius)
    {
        Sides = sides;
        Radius = radius;

        switch (Type)
        {
            case PolygonType.Filled:
                DrawFilledPolygon(Sides, Radius);
                break;
            case PolygonType.Hollowed:
                DrawHollowedPolygon(Sides, Radius, InnerRadius);
                break;
        }
    }

    public void DrawFilledPolygon(int sides, float radius)
    {
        _polygonPoints = GetCircumferencePoints(sides, radius).ToArray();
        _polygonTriangles = GetFilledPolygonTriangles(_polygonPoints);
        
        _mesh.Clear();
        _mesh.vertices = _polygonPoints;
        _mesh.triangles = _polygonTriangles;
    }

    public void DrawHollowedPolygon(int sides, float outerRadius, float innerRadius)
    {
        List<Vector3> pointsList = new List<Vector3>();
        List<Vector3> outerPoints = GetCircumferencePoints(sides, outerRadius);
        pointsList.AddRange(outerPoints);
        List<Vector3> innerPoints = GetCircumferencePoints(sides, innerRadius);
        pointsList.AddRange(innerPoints);

        _polygonPoints = pointsList.ToArray();
        _polygonTriangles = GetHollowedPolygonTriangles(_polygonPoints);
        
        _mesh.Clear();
        _mesh.vertices = _polygonPoints;
        _mesh.triangles = _polygonTriangles;
    }
    
    private List<Vector3> GetCircumferencePoints(int sides, float radius)
    {
        if (sides < 3) return null;
        
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < sides; i++)
        {
            Vector3 point = Vector3.zero;
            point.x = Mathf.Cos((1f / sides) * (2 * Mathf.PI) * i) * radius;
            point.y = Mathf.Sin((1f / sides) * (2 * Mathf.PI) * i) * radius;
            point.z = 0;
            
            points.Add(point);
        }
        
        return points;
    }

    private int[] GetFilledPolygonTriangles(Vector3[] points)
    {
        int triangleAmount = points.Length - 2;
        
        List<int> triangles = new List<int>();
        for (int i = 0; i < triangleAmount; i++)
        {
            // Clockwise
            triangles.Add(0);
            triangles.Add(i+2);
            triangles.Add(i+1);
        }

        return triangles.ToArray();
    }

    private int[] GetHollowedPolygonTriangles(Vector3[] points)
    {
        int triangleAmount = points.Length;
        int sides = points.Length / 2;

        List<int> triangles = new List<int>();
        for (int i = 0; i < triangleAmount / 2; i++)
        {
            int curOuterIndex = i;
            int nextOuterIndex = (i + 1) % sides;
            
            int curInnerIndex = curOuterIndex + sides;
            int nextInnerIndex = nextOuterIndex + sides;

            // Clockwise
            triangles.Add(curOuterIndex);
            triangles.Add(curInnerIndex);
            triangles.Add(nextOuterIndex);

            // Clockwise
            triangles.Add(curOuterIndex);
            triangles.Add(nextInnerIndex);
            triangles.Add(curInnerIndex);
        }

        return triangles.ToArray();
    }

    private void Draw()
    {
        _mesh.Clear();
        _mesh.vertices = _polygonPoints;
        _mesh.triangles = _polygonTriangles;
    }
}