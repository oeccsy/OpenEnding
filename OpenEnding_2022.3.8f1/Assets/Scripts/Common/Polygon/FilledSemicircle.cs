using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.Polygon
{
    public class FilledSemiCircle : FilledPolygon
    {
        public override void DrawPolygon(int sides, float radius)
        {
            if (sides % 2 == 1) return;
            
            _sides = sides;
            _radius = radius;
            
            _polygonPoints = GetPoints(sides, radius);
            _polygonTriangles = GetTriangles(_polygonPoints);
        
            _mesh.Clear();
            _mesh.vertices = _polygonPoints;
            _mesh.triangles = _polygonTriangles;
        }

        protected Vector3[] GetPoints(int sides, float radius)
        {
            var points = GetCircumferencePoints(sides, radius);
            points.RemoveRange(sides/2 + 1, sides/2 - 1);

            return points.ToArray();
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
