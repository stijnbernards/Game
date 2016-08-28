using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Voronoi
{
    public List<VoronoiEdge> Edges = new List<VoronoiEdge>();
    public Dictionary<Vector2, Polygon> Polygons = new Dictionary<Vector2, Polygon>();
    public List<Vector2> Sites = new List<Vector2>();

    public Voronoi(List<VoronoiEdge> edges, List<Vector2> sites)
    {
        Edges = edges;
        Sites = sites;
        Polygons = GetPolygons();
    }

    public HashSet<Vector2> Visualize()
    {
        HashSet<Vector2> points = new HashSet<Vector2>();

        foreach (VoronoiEdge edge in Edges)
        {
            points.UnionWith(GenHelpers.PlotLine((int)edge.site1.x, (int)edge.site1.y, (int)edge.site2.x, (int)edge.site2.y, false));
        }

        return points;
    }

    public Dictionary<Vector2, Polygon> GetPolygons()
    {
        Dictionary<Vector2, Polygon> polyDict = new Dictionary<Vector2, Polygon>();

        foreach (VoronoiEdge vEdge in Edges)
        {
            if (polyDict.ContainsKey(vEdge.Midpoint.Point))
            {
                polyDict[vEdge.Midpoint.Point].Points.Add(vEdge.site1);
                polyDict[vEdge.Midpoint.Point].Points.Add(vEdge.site2);
            }
            else
            {
                polyDict.Add(vEdge.Midpoint.Point, new Polygon(vEdge.Midpoint, new HashSet<Vector2>(){vEdge.site1, vEdge.site2}));
            }
        }

        return polyDict;
    }

    public void AssignPerlin()
    {
        foreach (Polygon poly in Polygons.Values)
        {
            poly.PerlinNoise = (Mathf.PerlinNoise(poly.MidPoint.Point.x, poly.MidPoint.Point.x) - 0.5F) * 800.0F;
        }
    }
}