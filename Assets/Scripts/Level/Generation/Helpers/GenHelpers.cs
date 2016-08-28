using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GenHelpers
{
    private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

    public static Vector2 GenerateBetween(Vector2 p1, Vector2 p2)
    {
        float x, y;
        x = Random.Range(p1.x, p2.x);
        y = Random.Range(p1.y, p2.y);

        return new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }

    public static List<Vector2> GenrateAmountBetween(Vector2 p1, Vector2 p2, int amount)
    {
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < amount; i++)
        {
            points.Add(GenerateBetween(p1, p2));
        }

        return points;
    }

    public static Vector2[] PlotLine(float x0, float y0, float x1, float y1, bool thickness = true)
    {
        return PlotLine((int)x0, (int)y0, (int)x1, (int)y1, thickness);
    }

    public static Vector2[] PlotLine(int x0, int y0, int x1, int y1, bool thickness = true)
    {
        List<Vector2> points = new List<Vector2>();

        bool steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);

        if (steep) 
        { 
            Swap<int>(
                ref x0, 
                ref y0
                );
 
            Swap<int>(
                ref x1, 
                ref y1
                ); 
        }

        if (x0 > x1) 
        { 
            Swap<int>(
                ref x0, 
                ref x1
                ); 

            Swap<int>(
                ref y0, 
                ref y1
                ); 
        }

        int dX = (x1 - x0), 
            dY = Mathf.Abs(y1 - y0), 
            err = (dX / 2), 
            ystep = (y0 < y1 ? 1 : -1), 
            y = y0;

        for (int x = x0; x <= x1; ++x)
        {
            if (!steep)
            {
                points.Add(new Vector2(x, y));
                if (thickness)
                {
                    points.Add(new Vector2(x + 1, y));
                    points.Add(new Vector2(x, y + 1));
                    points.Add(new Vector2(x, y - 1));
                    points.Add(new Vector2(x - 1, y));
                }
            }
            else
            {
                points.Add(new Vector2(y, x));
                if (thickness)
                {
                    points.Add(new Vector2(y, x + 1));
                    points.Add(new Vector2(y, x - 1));
                    points.Add(new Vector2(y + 1, x));
                    points.Add(new Vector2(y - 1, x));
                }
            }

            err = err - dY;
            if (err < 0) { y += ystep; err += dX; }
        }

        return points.ToArray();
    }

    public static float Sign(float x, float y)
    {
        if (x - y > 0)
            return 1;
        else if (x - y == 0)
            return 0;
        else
            return -1;
    }
}

