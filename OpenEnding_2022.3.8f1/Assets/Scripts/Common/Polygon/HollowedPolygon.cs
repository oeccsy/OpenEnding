using System.Collections.Generic;
using UnityEngine;

namespace Common.Polygon
{
    public class HollowedPolygon : Polygon
    {
        [SerializeField]
        protected float _thickness = 1f;

        public float Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                DrawPolygon(Sides, Radius);
            }
        }
        
        public override void DrawPolygon(int sides, float radius)
        {
            _sides = sides;
            _radius = radius;

            List<Vector3> pointsList = new List<Vector3>();
            
            float outerRadius = radius;
            float innerRadius = radius - _thickness;
            List<Vector3> outerPoints = GetCircumferencePoints(sides, outerRadius);
            List<Vector3> innerPoints = GetCircumferencePoints(sides, innerRadius);
            pointsList.AddRange(outerPoints);
            pointsList.AddRange(innerPoints);

            _polygonPoints = pointsList.ToArray();
            _polygonTriangles = GetTriangles(_polygonPoints);
        
            _mesh.Clear();
            _mesh.vertices = _polygonPoints;
            _mesh.triangles = _polygonTriangles;
        }

        protected override int[] GetTriangles(Vector3[] points)
        {
            int triangleAmount = points.Length;
            int sides = points.Length / 2;

            List<int> triangles = new List<int>();
            for (int i = 0; i < triangleAmount / 2; i++)
            {
                int curOuterIndex = i;
                int curInnerIndex = curOuterIndex + sides;
                
                int nextOuterIndex = (i + 1) % sides;
                int prevInnerIndex = ((sides + i - 1) % sides) + sides;

                // Clockwise
                triangles.Add(curOuterIndex);
                triangles.Add(curInnerIndex);
                triangles.Add(nextOuterIndex);

                // Clockwise
                triangles.Add(curOuterIndex);
                triangles.Add(prevInnerIndex);
                triangles.Add(curInnerIndex);
            }

            return triangles.ToArray();
        }
    }
}