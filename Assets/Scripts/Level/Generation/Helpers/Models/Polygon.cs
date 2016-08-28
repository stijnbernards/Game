using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Polygon
{

    public Site MidPoint;
    
    //public List<Vector2> Vertices = new List<Vector2>();
    
    public HashSet<Vector2> Points = new HashSet<Vector2>();
    
    public bool faulty = false;

    public float PerlinNoise = 0.0F;

    public Polygon(Site midpoint, HashSet<Vector2> points)
    {
        MidPoint = midpoint;
        Points = points;
    }

    public static bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        float pointX = point.x, pointY = point.y;
        float startX, startY, endX, endY;
        Vector2 endPoint = polygon[polygonLength - 1];

        endX = endPoint.x;
        endY = endPoint.y;

        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.x; endY = endPoint.y;
            inside ^= (endY > pointY ^ startY > pointY) 
                      && 
                      ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }
        return inside;
    }
}