﻿using UnityEngine;

namespace DevOnly.AnimSystem
{
    public class NoisePolygon : Polygon
    {
        private LineRenderer _lineRenderer;
        protected override void Awake()
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        public override void DrawPolygon(int sides, float radius)
        {
            _lineRenderer.positionCount = sides;
            _lineRenderer.loop = true;
        
            for (int i = 0; i < sides; i++)
            {
                Vector3 point = Vector3.zero;
                point.x = Mathf.Cos((1f / sides) * (2 * Mathf.PI) * i) * radius;
                point.y = Mathf.Sin((1f / sides) * (2 * Mathf.PI) * i) * radius;
                point.z = Random.Range(0f, 1f) * radius;
            
                _lineRenderer.SetPosition(i, point);
            }
        }

        protected override int[] GetTriangles(Vector3[] points)
        {
            throw new System.NotImplementedException();
        }
    }
}