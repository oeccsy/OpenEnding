using System.Collections.Generic;
using UnityEngine;

namespace DevOnly.AnimSystem
{
    public class FilledPolygon : Polygon
    {
        public override void DrawPolygon(int sides, float radius)
        {
            _sides = sides;
            _radius = radius;
            
            _polygonPoints = GetCircumferencePoints(sides, radius).ToArray();
            _polygonTriangles = GetTriangles(_polygonPoints);
        
            _mesh.Clear();
            _mesh.vertices = _polygonPoints;
            _mesh.triangles = _polygonTriangles;
        }

        protected override int[] GetTriangles(Vector3[] points)
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
    }
}