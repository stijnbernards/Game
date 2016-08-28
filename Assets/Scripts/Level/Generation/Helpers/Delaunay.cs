using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

class Delaunay
{
    public static List<Triangle> Triangulate(List<Vector2> vertices)
    {
        int vertex_count = vertices.Count;
        
        if(vertex_count < 3)
        {
            throw new ArgumentException("Not enought vertices");
        }

        int max_triangles = 4 * vertex_count;

        float xmin = vertices[0].x;
        float ymin = vertices[0].y;
        float xmax = xmin;
        float ymax = ymin;

        for (int i = 1; i < vertex_count; i++)
        {
            if (vertices[i].x < xmin)
            {
                xmin = vertices[i].x;
            }

            if (vertices[i].x > xmax)
            {
                xmax = vertices[i].x;
            }

            if (vertices[i].y < ymin)
            {
                ymin = vertices[i].y;
            }

            if (vertices[i].y > ymax)
            {
                ymax = vertices[i].y;
            }
        }

        float dx = xmax - xmin;
        float dy = ymax - ymin;
        float dmax = (dx > dy) ? dx : dy;

        float xmid = (xmax + xmin) * 0.5F;
        float ymid = (ymax + ymin) * 0.5F;

        vertices.Add(new Vector2(xmid - 2 * dmax, ymid - dmax));
        vertices.Add(new Vector2(xmid, ymid + 2 * dmax));
        vertices.Add(new Vector2(xmid + 2 * dmax, ymid - dmax));

        List<Triangle> triangles = new List<Triangle>();
        triangles.Add(new Triangle(vertex_count, vertex_count + 1, vertex_count + 2));

        for (int i = 0; i < vertex_count; i++)
        {
            List<Side> sides = new List<Side>();

            for (int x = 0; x < triangles.Count; x++)
            {
                if(InCircle(vertices[i], vertices[(int) (triangles[x].Points.x)], vertices[(int) (triangles[x].Points.y)], vertices[(int) (triangles[x].Points.z)]))
                {
                    sides.Add(new Side(triangles[x].Points.x, triangles[x].Points.y));
                    sides.Add(new Side(triangles[x].Points.y, triangles[x].Points.z));
                    sides.Add(new Side(triangles[x].Points.z, triangles[x].Points.x));

                    triangles.RemoveAt(x);
                    x--;
                }
            }

            if (i >= vertex_count)
            {
                continue;
            }

            for (int x = sides.Count - 2; x >= 0; x--)
            {
                for (int y = sides.Count - 1; y >= x + 1; y--)
                {
                    if (sides[x].Equals(sides[y]))
                    {
                        sides.RemoveAt(y);
                        sides.RemoveAt(x);
                        y--;
                        continue;
                    }
                }
            }

            for (int x = 0; x < sides.Count; x++)
            {
                if (triangles.Count >= max_triangles)
                {
                    throw new ApplicationException("Too edgy");
                }

                triangles.Add(new Triangle(sides[x].points.x, sides[x].points.y, i));
            }
            sides.Clear();
            sides = null;
        }

        for (int i = triangles.Count - 1; i >= 0; i--)
        {
            if (triangles[i].Points.x >= vertex_count || triangles[i].Points.y >= vertex_count || triangles[i].Points.z >= vertex_count)
            {
                triangles.RemoveAt(i);
            }
        }

        vertices.RemoveAt(vertices.Count - 1);
        vertices.RemoveAt(vertices.Count - 1);
        vertices.RemoveAt(vertices.Count - 1);
        triangles.TrimExcess();

        return triangles;
    }

    private static bool InCircle(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
    {
        if (Mathf.Abs(point2.y - point3.y) < float.Epsilon && Mathf.Abs(point3.y - point4.y) < float.Epsilon)
        {
            return false;
        }

        float m1, m2,
              mx1, mx2,
              my1, my2,
              xc, yc;

        if (Mathf.Abs(point3.y - point2.y) < double.Epsilon)
        {
            m2 = -(point4.x - point3.x) / (point4.y - point3.y);
            mx2 = (point3.x + point4.x) * 0.5F;
            my2 = (point3.y + point4.y) * 0.5F;

            xc = (point3.x + point2.x) * 0.5F;
            yc = m2 * (xc - mx2) + my2;
        }
        else if (Mathf.Abs(point4.y - point3.y) < float.Epsilon)
        {
            m1 = -(point3.x - point2.x) / (point3.y - point2.y);
            mx1 = (point2.x + point3.x) * 0.5F;
            my1 = (point2.y + point3.y) * 0.5F;

            xc = (point4.x + point3.x) * 0.5F;
            yc = m1 * (xc - mx1) + my1;
        }
        else
        {
            m1 = -(point3.x - point2.x) / (point3.y - point2.y);
            m2 = -(point4.x - point3.x) / (point4.y - point3.y);
            mx1 = (point2.x + point3.x) * 0.5F;
            mx2 = (point3.x + point4.x) * 0.5F;
            my1 = (point2.y + point3.y) * 0.5F;
            my2 = (point3.y + point4.y) * 0.5F;

            xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
            yc = m1 * (xc - mx1) + my1;
        }

        float dx = point3.x - xc;
        float dy = point3.y - yc;
        float rsqr = dx * dx + dy * dy;
        dx = point1.x - xc;
        dy = point1.y - yc;
        float drsqr = dx * dx + dy * dy;

        return (drsqr <= rsqr);
    }

    public static Vector2 FindCircumCenter(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float f1 = (p2.x - p1.x) / (p1.y - p2.y);
        //center ab
        Vector2 m1 = new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
        float g1 = m1.y - f1 * m1.x;

        float f2 = (p3.x - p2.x) / (p2.y - p3.y);
        //center cb
        Vector2 m2 = new Vector2((p2.x + p3.x) / 2, (p2.y + p3.y) / 2);
        float g2 = m2.y - f2 * m2.x;

        if (f1 == f2)
        {
            return new Vector2(0, 0);
        }
        else if (p1.y == p2.y)
        {
            return new Vector2(m1.x, f2 * m1.x + g2);
        }
        else if (p2.y == p3.y)
        {
            return new Vector2(m2.x, f1 * m2.x + g1);
        }
            
        float x = (g2 - g1) / (f1 - f2);

        return new Vector2(x, f1 * x + g1);
    }

    public static Voronoi DeriveVoronoi(List<Vector2> startingPoints, List<Triangle> triangles)
    {
        List<VoronoiEdge> points = new List<VoronoiEdge>();
        Vector2 c1, c2;
        HashSet<Vector2> unique_vertices = new HashSet<Vector2>();
        List<Vector2> non_unique_vertices = new List<Vector2>();
        HashSet<Vector2> sites = new HashSet<Vector2>();
        Vector2 midpoint = new Vector2(0, 0);
        int i = 0;
        bool add = true;

        foreach (Triangle triangleX in triangles)
        {
            foreach (Triangle triangleY in triangles)
            {
                if (!triangleX.Equals(triangleY))
                {
                    unique_vertices.Add(startingPoints[(int)triangleX.Points.x]);
                    unique_vertices.Add(startingPoints[(int)triangleX.Points.y]);
                    unique_vertices.Add(startingPoints[(int)triangleX.Points.z]);
                    unique_vertices.Add(startingPoints[(int)triangleY.Points.x]);
                    unique_vertices.Add(startingPoints[(int)triangleY.Points.y]);
                    unique_vertices.Add(startingPoints[(int)triangleY.Points.z]);
                    non_unique_vertices.Add(startingPoints[(int)triangleX.Points.x]);
                    non_unique_vertices.Add(startingPoints[(int)triangleX.Points.y]);
                    non_unique_vertices.Add(startingPoints[(int)triangleX.Points.z]);
                    non_unique_vertices.Add(startingPoints[(int)triangleY.Points.x]);
                    non_unique_vertices.Add(startingPoints[(int)triangleY.Points.y]);
                    non_unique_vertices.Add(startingPoints[(int)triangleY.Points.z]);
                        
                    if (unique_vertices.Count == 4)
                    {
                        
                        c1 = FindCircumCenter(
                            new Vector2(startingPoints[(int)triangleX.Points.x].x, startingPoints[(int)triangleX.Points.x].y),
                            new Vector2(startingPoints[(int)triangleX.Points.y].x, startingPoints[(int)triangleX.Points.y].y),
                            new Vector2(startingPoints[(int)triangleX.Points.z].x, startingPoints[(int)triangleX.Points.z].y)
                            );

                        c2 = FindCircumCenter(
                            new Vector2(startingPoints[(int)triangleY.Points.x].x, startingPoints[(int)triangleY.Points.x].y),
                            new Vector2(startingPoints[(int)triangleY.Points.y].x, startingPoints[(int)triangleY.Points.y].y),
                            new Vector2(startingPoints[(int)triangleY.Points.z].x, startingPoints[(int)triangleY.Points.z].y)
                            );

                        List<Vector2> test = non_unique_vertices.GroupBy(x => x)
                            .Where(g => g.Count() == 2)
                            .Select(g => g.Key)
                            .ToList();

                        i = 0;
                        add = true;

                        while(true)
                        {
                            add = true;
                            if (i >= test.Count)
                            {
                                add = false;
                                break;
                            }

                            if (triangleX.Taken.Contains(test.ToList()[i]))
                            {
                                i++;
                                continue;
                            }
                            else
                            {
                                foreach(VoronoiEdge triangEdge in triangleX.VoronoiEdges)
                                {
                                    foreach (VoronoiEdge triangEdgeY in triangleY.VoronoiEdges)
                                    {
                                        if (triangEdge.Equals(new VoronoiEdge(c1, c2, new Site(test[i]))) || triangEdge.Equals(new VoronoiEdge(c2, c1, new Site(test[i]))) || triangEdgeY.Equals(new VoronoiEdge(c1, c2, new Site(test[i]))) || triangEdgeY.Equals(new VoronoiEdge(c2, c1, new Site(test[i]))))
                                        {
                                            add = false;
                                            break;
                                        }
                                    }

                                    if (!add)
                                    {
                                        break;
                                    }
                                }

                                foreach (VoronoiEdge pointEdge in points)
                                {
                                    if (pointEdge.Equals(new VoronoiEdge(c1, c2, new Site(test[i]))) || pointEdge.Equals(new VoronoiEdge(c2, c1, new Site(test[i]))))
                                    {
                                        add = false;
                                        break;
                                    }
                                }

                                if (add)
                                {
                                    triangleX.Taken.Add(test.ToList()[i]);
                                    midpoint = test.ToList()[i];
                                    break;
                                }
                                i++;
                            }
                        }

                        if (add)
                        {
                            triangleX.VoronoiEdges.Add(new VoronoiEdge(c1, c2, new Site(midpoint)));

                            points.Add(new VoronoiEdge(c1, c2, new Site(midpoint)));
                            sites.Add(c1);
                            sites.Add(c2);
                        }

                        midpoint = new Vector2(0, 0);
                    }

                    unique_vertices.Clear();
                    non_unique_vertices.Clear();
                }

                //if (triangleX.Taken.Count == 3)
                //{
                //    break;
                //}
            }
        }

        foreach (Triangle triangle in triangles)
        {
            if (triangle.Taken.Count < 3)
            {
                foreach (VoronoiEdge vEdge in triangle.VoronoiEdges)
                {
                    points.Find(x => x.Equals(vEdge)).Midpoint.Faulty = true;
                }
            }
        }

        //First, find the center of the bounding box that contains all of your vertices. We'll call this point C.

//Sort your list of vertices based on each point's angle with respect to C. You can use atan2(point.y - C.y, point.x - C.x) to find the angle. If two or more vertices have the same angle, the one closer to C should come first.

//Then, draw your points in the order they appear in the list. You will end up with a starburst pattern that is non-intersecting and probably non-convex. Example:

        return new Voronoi(points.ToList(), startingPoints);
    }
}

