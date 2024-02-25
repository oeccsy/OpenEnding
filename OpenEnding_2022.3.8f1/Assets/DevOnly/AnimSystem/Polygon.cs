using UnityEngine;

public class Polygon : MonoBehaviour
{
    public int Sides { get; set; }
    public float Radius { get; set; }
    
    public LineRenderer LineRenderer { get; private set; }
    private void Awake()
    {
        Sides = 0;
        Radius = 0f;
        LineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    public void Update()
    {
        if (Sides < 3 || Radius <= 0) return;
    }
}