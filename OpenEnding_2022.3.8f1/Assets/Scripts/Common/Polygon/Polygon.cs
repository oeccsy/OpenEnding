using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Common.Polygon
{
    public abstract class Polygon : MonoBehaviour
    {
        protected MeshRenderer _meshRenderer;
        protected MeshFilter _meshFilter;
    
        protected Mesh _mesh;
        protected Vector3[] _polygonPoints;
        protected int[] _polygonTriangles;

        [SerializeField]
        protected int _sides = 3;
        [SerializeField]
        protected float _radius = 1f;

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
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshRenderer.material = Resources.Load<Material>("Materials/Polygon");
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshFilter.mesh = _mesh = new Mesh();
        }

        protected virtual void Start()
        {
            DrawPolygon(Sides, Radius);
        }

        public abstract void DrawPolygon(int sides, float radius);
        
        protected List<Vector3> GetCircumferencePoints(int sides, float radius)
        {
            if (sides < 3) return null;
        
            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i < sides; i++)
            {
                Vector3 point = Vector3.zero;
                float theta;

                if (sides % 2 == 1)
                {
                    theta = (1f / sides) * (2 * Mathf.PI);
                    point.x = Mathf.Cos(theta * i + Mathf.PI/2) * radius;
                    point.y = Mathf.Sin(theta * i + Mathf.PI/2) * radius;
                    point.z = 0;
                }
                else if (sides == 4)
                {
                    theta = (1f / sides) * (2 * Mathf.PI);
                    point.x = Mathf.Cos(theta * i + Mathf.PI/4) * radius;
                    point.y = Mathf.Sin(theta * i + Mathf.PI/4) * radius;
                    point.z = 0;
                }
                else
                {
                    theta = (1f / sides) * (2 * Mathf.PI);
                    point.x = Mathf.Cos(theta * i) * radius;
                    point.y = Mathf.Sin(theta * i) * radius;
                    point.z = 0;
                }
                
                points.Add(point);
            }
        
            return points;
        }

        protected abstract int[] GetTriangles(Vector3[] points);

        public List<Vector3> GetVertexPositions()
        {
            return _polygonPoints.ToList();
        }
        
        public IEnumerator MorphTo(Polygon targetPolygon, float time)
        {
            List<Vector3> startingPoints = GetVertexPositions();
            List<Vector3> finishingPoints = targetPolygon.GetVertexPositions();
                
            int difference = finishingPoints.Count - startingPoints.Count;
            if (difference > 0) AddFillerPoints(ref startingPoints, difference);
            if (difference < 0) AddFillerPoints(ref finishingPoints, -difference);

            _mesh.vertices = _polygonPoints = startingPoints.ToArray();
            _mesh.triangles = _polygonTriangles = GetTriangles(_polygonPoints);
            
            float timer = 0f;
            while (timer < time)
            {
                timer += Time.deltaTime;
                
                for (int i = 0; i < startingPoints.Count; i++)
                {
                    _polygonPoints[i] = Vector3.Lerp(startingPoints[i], finishingPoints[i], timer / time);
                }

                _mesh.vertices = _polygonPoints;
                yield return null;
            }
            
            _polygonPoints = finishingPoints.ToArray();
        }

        protected void AddFillerPoints(ref List<Vector3> points, int amount)
        {
            if (points.Count < 2) return;
            
            List<Vector3> newPoints = new List<Vector3>();
            int totalSections = points.Count;
            
            for (int section = 0; section < totalSections; section++)
            {
                newPoints.Add(points[section]);
                
                int countToAdd = amount / totalSections;
                if (section < amount % totalSections) countToAdd++;

                float gap = (float)1 / (countToAdd+1);
                
                for (int i = 1; i <= countToAdd; i++)
                {
                    Vector3 pos = Vector3.Lerp(points[section], points[(section + 1) % totalSections], gap * i);
                    newPoints.Add(pos);
                }
            }

            points = newPoints;
        }

        public void SetColor(Color color, float duration = 1f)
        {
            _meshRenderer.material.DOColor(color, duration);
        }
    }
}