using System;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : IEquatable<Triangle>
{
    public Vector3 Points;

    public List<VoronoiEdge> VoronoiEdges;
    public List<Vector2> Taken = new List<Vector2>();

    public Triangle(float p1, float p2, float p3)
    {
        Points = new Vector3(p1, p2, p3);
        VoronoiEdges = new List<VoronoiEdge>();
    }

    public bool Equals(Triangle other)
    {
        if (other.Points == Points)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Intercepts(Triangle other)
    {
        if (other.Points.x == Points.x && other.Points.y == Points.y && other.Points.z == Points.z)
        {
            return false;
        }
        if (other.Points.x == Points.x || other.Points.y == Points.y || other.Points.z == Points.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Intercepts(Vector3 other)
    {
        if (other.x == Points.x || other.y == Points.y || other.z == Points.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Vector2> Extract(List<Vector2> points)
    {
        return new List<Vector2>()
        {
            new Vector2(points[(int)Points.x].x, points[(int)Points.x].y),
            new Vector2(points[(int)Points.y].x, points[(int)Points.y].y),
            new Vector2(points[(int)Points.z].x, points[(int)Points.z].y),
        };
    }
}