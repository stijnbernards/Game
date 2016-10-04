using System;
using UnityEngine;

class Side : IEquatable<Side>
{
    public Vector2 points;

    public Side(float p1, float p2)
    {
        points = new Vector2(p1, p2);
    }

    public Side()
        : this(0, 0)
    {

    }

    public bool Equals(Side other)
    {
        return
            ((this.points.x == other.points.y) && (this.points.y == other.points.x)) ||
            ((this.points.x == other.points.x) && (this.points.y == other.points.y));
    }
}
