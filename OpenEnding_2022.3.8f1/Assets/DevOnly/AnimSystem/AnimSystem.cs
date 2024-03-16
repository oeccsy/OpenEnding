using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSystem : MonoBehaviour
{
    public List<Vector3> test = new List<Vector3>();
    private void Awake()
    {
        //GetCircumferencePoints(6, 3);
        SpawnRegularPolygon(6, 1f);
        SpawnRegularMeshPolygon(6, 1f);
        //SpawnNoisedPolygon(100, 1);
    }
    
    public List<Vector3> GetCircumferencePoints(int sides, float radius)
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

    public Polygon SpawnRegularPolygon(int steps, float radius)
    {
        GameObject newObj = new GameObject("Polygon", typeof(Polygon));
        Polygon polygon = newObj.GetComponent<Polygon>();
        
        polygon.LineRenderer.positionCount = steps;
        polygon.LineRenderer.loop = true;

        for (int i = 0; i < steps; i++)
        {
            Vector3 point = Vector3.zero;
            point.x = Mathf.Cos((1f / steps) * (2 * Mathf.PI) * i) * radius;
            point.y = Mathf.Sin((1f / steps) * (2 * Mathf.PI) * i) * radius;
            point.z = 0;
            
            polygon.LineRenderer.SetPosition(i, point);
        }
        
        return polygon;
    }
    
    public Polygon SpawnNoisedPolygon(int steps, float radius)
    {
        Polygon polygon = SpawnRegularPolygon(steps, radius);

        for (int i = 0; i < steps; i++)
        {
            Vector3 point = polygon.LineRenderer.GetPosition(i);
            point.z = Random.Range(0f, 1f) * radius;
            
            polygon.LineRenderer.SetPosition(i, point);
        }
        
        return polygon;
    }

    public MeshPolygon SpawnRegularMeshPolygon(int steps, float radius)
    {
        GameObject newObj = new GameObject("MeshPolygon", typeof(MeshPolygon));
        MeshPolygon polygon = newObj.GetComponent<MeshPolygon>();

        polygon.Sides = steps;
        polygon.Radius = radius;
        return polygon;
    }
}
