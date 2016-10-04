using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LloydRelaxation
{
    public static Voronoi Relax(Voronoi voronoi, Vector2 bounds1, Vector2 bounds2, int amount = 100)
    {
        Voronoi newVoronoi;
        HashSet<Vector2> sites = new HashSet<Vector2>();
        Vector2 centroidRemember;

        foreach (KeyValuePair<Vector2, Polygon> poly in voronoi.Polygons)
        {
            centroidRemember = PolygonCentroid(poly.Value);
            if (centroidRemember.x != -Mathf.Infinity && centroidRemember.y != -Mathf.Infinity && centroidRemember.x != Mathf.Infinity && centroidRemember.y != Mathf.Infinity && centroidRemember.x > bounds1.x && centroidRemember.y > bounds1.y && centroidRemember.x < bounds2.x && centroidRemember.y < bounds2.y)
            {
                sites.Add(centroidRemember);
            }
            else
            {
                sites.Add(poly.Value.MidPoint.Point);
            }
        }

        amount--;

        newVoronoi = Delaunay.DeriveVoronoi(sites.ToList(), Delaunay.Triangulate(sites.ToList()));

        if (amount <= 0)
        {
            return newVoronoi;
        }
        else
        {
            return Relax(newVoronoi, bounds1, bounds2, amount);
        }
    }

    public static Vector2 PolygonCentroid(Polygon poly)
    {
        if (!poly.MidPoint.Faulty)
        {   
            Vector2 centroid = new Vector2(0, 0);
            List<Vector2> points = new List<Vector2>();
            float signedArea = 0.0F;
            float x0 = 0.0F;
            float y0 = 0.0F;
            float x1 = 0.0F;
            float y1 = 0.0F;
            float a = 0.0F;

            for (int i = 0; i < poly.Points.Count; i++)
            {
                points = new List<Vector2>(poly.Points.ToList());

                x0 = points[i].x;
                y0 = points[i].y;
                x1 = points[(i + 1) % points.Count].x;
                y1 = points[(i + 1) % points.Count].y;
                a = x0 * y1 - x1 * y0;
                signedArea += a;
                centroid.x += (x0 + x1) * a;
                centroid.y += (y0 + y1) * a;
            }

            signedArea *= 0.5F;
            centroid.x /= (6.0F * signedArea);
            centroid.y /= (6.0F * signedArea);
            
            return centroid;
        }
        else
        {
            return poly.MidPoint.Point; 
        }
    }
}