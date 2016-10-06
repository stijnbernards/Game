using System;
using System.Collections.Generic;
using UnityEngine;


public class Site
{
    public Vector2 Point;
    public bool Faulty = false;

    public Site(Vector2 point, bool faulty = false)
    {
        Point = point;
        Faulty = faulty;
    }
}