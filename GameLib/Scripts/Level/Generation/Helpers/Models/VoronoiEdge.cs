using System;
using UnityEngine;

public class VoronoiEdge : IEquatable<VoronoiEdge>
{
    public Vector2 site1;
    public Vector2 site2;
    public Site Midpoint;

    public bool Used { get; set; }

    public VoronoiEdge(Vector2 c1, Vector2 c2, Site midpoint)
    {
        site1 = c1;
        site2 = c2;
        Used = false;
        Midpoint = midpoint;
    }


    public bool Equals(VoronoiEdge other)
    {
        if (this.site1 == other.site1 && this.site2 == other.site2 && this.Midpoint.Point == other.Midpoint.Point)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}