using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GenHelpers;

public class CaveRooms : Generate
{
    public override void GenerateLevel()
    {

    }

    public Vector2[] RoomBounds(int size)
    {
        int[,] square = new int[size, size];

        int calc1 = Mathf.RoundToInt(size / 5);
        int calc2 = Mathf.RoundToInt(calc1 * 3);
        int calc3 = Mathf.RoundToInt(calc1 * 4);

        return null;
    }

}
