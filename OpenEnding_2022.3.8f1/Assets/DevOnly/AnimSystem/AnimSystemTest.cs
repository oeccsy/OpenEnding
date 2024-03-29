using System.Collections;
using System.Collections.Generic;
using Common.Polygon;
using Unity.VisualScripting;
using UnityEngine;

public class AnimSystemTest : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var a = AddFilled(3, 2);
        var b = AddFilled(4, 2);
        // AddFilled(5, 2);
        //var b =AddFilled(6, 2);
        // AddFilled(7, 2);
        // AddFilled(8, 2);
        // AddFilled(9, 2);
        var c = AddFilled(10, 2);
        var e =  AddHollowed(5, 3);
        // AddLine();
        // AddNoise();

        var d = AddHollowed(3, 2);
        var f = AddHollowed(4, 2);
        
        yield return new WaitForSeconds(5f);

        Debug.Log("Start");
        
        yield return a.MorphTo(b, 3f);
        yield return c.MorphTo(a, 3f);
        yield return d.MorphTo(e, 3f);
    }

    private Polygon AddFilled(int sides, float radius)
    {
        GameObject newObj = new GameObject("FPolygon", typeof(FilledPolygon));
        var polygon = newObj.GetComponent<FilledPolygon>();
        polygon.Sides = sides;
        polygon.Radius = radius;

        return polygon;
    }
    
    private Polygon AddHollowed(int sides, float radius)
    {
        GameObject newObj = new GameObject("HPolygon", typeof(HollowedPolygon));
        var polygon = newObj.GetComponent<HollowedPolygon>();
        polygon.Sides = sides;
        polygon.Radius = radius;
        
        return polygon;
    }
    
    private void AddLine()
    {
        GameObject newObj = new GameObject("LPolygon", typeof(LinePolygon));
        var polygon = newObj.GetComponent<LinePolygon>();
    }
    
    private void AddNoise()
    {
        GameObject newObj = new GameObject("NPolygon", typeof(NoisePolygon));
        var polygon = newObj.GetComponent<NoisePolygon>();
    }
}
