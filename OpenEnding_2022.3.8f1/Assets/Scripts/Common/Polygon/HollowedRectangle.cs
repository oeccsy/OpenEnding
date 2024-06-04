using System.Collections.Generic;
using UnityEngine;

namespace Common.Polygon
{
    public class HollowedRectangle : HollowedPolygon
    {
        [SerializeField]
        protected float _xOffset = 1f;
        [SerializeField]
        protected float _yOffset = 2f;
        
        public float XOffset
        {
            get => _xOffset;
            set
            {
                _xOffset = value;
                DrawPolygon(Sides, Radius);
            }
        }
        
        public float YOffset
        {
            get => _yOffset;
            set
            {
                _yOffset = value;
                DrawPolygon(Sides, Radius);
            }
        }
        
        public override void DrawPolygon(int sides, float radius)
        {
            if (sides != 4) return;
            
            _sides = sides;
            _radius = radius;

            List<Vector3> pointsList = new List<Vector3>();
            
            float outerRadius = radius;
            float innerRadius = radius - _thickness;
            List<Vector3> outerPoints = GetCircumferencePoints(sides, outerRadius);
            List<Vector3> innerPoints = GetCircumferencePoints(sides, innerRadius);
            ApplyRectangleOffset(ref outerPoints, ref innerPoints);
            
            pointsList.AddRange(outerPoints);
            pointsList.AddRange(innerPoints);

            _polygonPoints = pointsList.ToArray();
            _polygonTriangles = GetTriangles(_polygonPoints);
        
            _mesh.Clear();
            _mesh.vertices = _polygonPoints;
            _mesh.triangles = _polygonTriangles;
        }

        private void ApplyRectangleOffset(ref List<Vector3> outerPoints, ref List<Vector3> innerPoints)
        {
            for (int i = 0; i < 2; i++)
            {
                outerPoints[i] += new Vector3(0, _yOffset * 0.5f, 0);
                innerPoints[i] += new Vector3(0, _yOffset * 0.5f, 0);
            }
            
            for (int i = 2; i < 4; i++)
            {
                outerPoints[i] += new Vector3(0, -_yOffset * 0.5f, 0);
                innerPoints[i] += new Vector3(0, -_yOffset * 0.5f, 0);
            }

            for (int i = 0; i < 4; i++)
            {
                if (i % 3 == 0)
                {
                    outerPoints[i] += new Vector3(_xOffset * 0.5f, 0, 0);
                    innerPoints[i] += new Vector3(_xOffset * 0.5f, 0, 0);
                }
                else
                {
                    outerPoints[i] += new Vector3(-_xOffset * 0.5f, 0, 0);
                    innerPoints[i] += new Vector3(-_xOffset * 0.5f, 0, 0);
                }
            }
        }
    }
}