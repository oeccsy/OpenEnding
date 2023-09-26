using System;
using System.Collections;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public static readonly float planetRadius = 8f;
    public float orbitRadius;

    [SerializeField]
    private float _theta;
    public float Theta { get => _theta; set => _theta = value % 360; }
    
    public Vector3 UpAxis => transform.position.normalized;
    public Vector3 ForwardAxis => Vector3.Cross(Vector3.right, UpAxis).normalized; // TODO : 0벡터 대응
    public Vector3 RightAxis => Vector3.Cross(UpAxis, ForwardAxis).normalized;
    public Vector3 Pos => new Vector3(transform.position.x, orbitRadius * Mathf.Cos(Theta * Mathf.Deg2Rad), orbitRadius * Mathf.Sin(Theta * Mathf.Deg2Rad));
    public Quaternion Rot => Quaternion.LookRotation(ForwardAxis, UpAxis);
    
    private void Awake()
    {
        orbitRadius = Mathf.Sqrt(planetRadius * planetRadius - transform.position.x * transform.position.x);
    }

    public void Update()
    {
        transform.position = Pos;
        transform.rotation = Rot;
    }
}
