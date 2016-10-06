using System.Collections.Generic;
using UnityEngine;

public class DungeonRectangle
{
    private static float MIN_SIZE = 10;

    public Vector2 Position;
    public Vector2 Size;
    public DungeonRectangle ChildOne;
    public DungeonRectangle ChildTwo;
    public List<DungeonRectangle> DungeonRooms = new List<DungeonRectangle>();

    public DungeonRectangle(Vector2 pos, Vector2 size)
    {
        Position = pos;
        Size = size;
    }

    public void StartSplitting(float amountRectangles)
    {
        List<DungeonRectangle> rectangles = new List<DungeonRectangle>();

        rectangles.Add(this);

        while (rectangles.Count < amountRectangles)
        {
            DungeonRectangle rect = rectangles[Random.Range(0, rectangles.Count)];

            if (rect.Split())
            {
                rectangles.Add(rect.ChildOne);
                rectangles.Add(rect.ChildTwo);
            }
        }
    }

    public bool Split()
    {
        if (ChildOne != null)
        {
            return false;
        }

        bool dir = (Random.value > 0.5 ? false : true);

        float max = (dir ? Size.y : Size.x) - MIN_SIZE;

        if (max <= MIN_SIZE)
        {
            return false;
        }

        float split = Random.Range(0, max);

        if (split < MIN_SIZE)
        {
            split = MIN_SIZE;
        }

        if (dir)
        {
            ChildOne = new DungeonRectangle(Position, new Vector2(Size.x, split));
            ChildTwo = new DungeonRectangle(new Vector2(Position.x, Position.y - split), new Vector2(Size.x, Size.y - split));
        }
        else
        {
            ChildOne = new DungeonRectangle(Position, new Vector2(split, Size.y));
            ChildTwo = new DungeonRectangle(new Vector2(Position.x + split, Position.y), new Vector2(Size.x - split, Size.y));
        }

        return true;
    }

    public Vector2[] Visualize()
    {
        List<Vector2> returnVectors = new List<Vector2>();

        if (ChildOne != null)
        {
            returnVectors.AddRange(ChildOne.Visualize());
            returnVectors.AddRange(ChildTwo.Visualize());

            foreach (Vector2 vect in ChildOne.ConnectTo(ChildTwo))
            {
                returnVectors.Add(vect);
            }
        }
        else
        {
            float height = Mathf.Max(Random.Range(0, Size.y), MIN_SIZE);
            float width = Mathf.Max(Random.Range(0, Size.x), MIN_SIZE);
            float left = Position.x + (Size.x / 2) - (width / 2);
            float top = Position.y - (Size.y / 2) + (height / 2);

            for (float x = left; x < left + width; x++)
            {
                for (float y = top - height; y < top; y++)
                {
                    returnVectors.Add(new Vector2(x, y));
                }
            }
        }

        return returnVectors.ToArray();
    }

    public Vector2[] ConnectTo(DungeonRectangle connector)
    {
        return GenHelpers.PlotLine(Position.x + Size.x / 2, Position.y - Size.y / 2, connector.Position.x + connector.Size.x / 2, connector.Position.y - connector.Size.y / 2, true);
    }
}
