using System.Collections.Generic;
using UnityEngine;

namespace DevOnly.AnimSystem
{
    public abstract class Polygon : MonoBehaviour
    {
        protected MeshRenderer _meshRenderer;
        protected MeshFilter _meshFilter;
    
        protected Mesh _mesh;
        protected Vector3[] _polygonPoints;
        protected int[] _polygonTriangles;

        private int _sides = 3;
        private float _radius = 1f;

        public int Sides
        {
            get => _sides;
            set
            {
                _sides = value;
                DrawPolygon(_sides, _radius);
            }
        }

        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                DrawPolygon(_sides, _radius);
            }
        }

        protected virtual void Awake()
        {
            Radius = 0f;
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshFilter.mesh = _mesh = new Mesh();
        }

        public abstract void DrawPolygon(int sides, float radius);
        
        protected List<Vector3> GetCircumferencePoints(int sides, float radius)
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

        protected abstract int[] GetTriangles(Vector3[] points);
    }
}