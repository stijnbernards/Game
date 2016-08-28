using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Generate
{

    public GameObject groundSprite = Resources.Load("Ground") as GameObject;
    public GameObject wallSprite = Resources.Load("Stone") as GameObject;

    public override void BeginPoint() { }
    public override void EndPoint() { }

    public override void GenerateLevel()
    {
        List<Rectangle> Rectlanges = new List<Rectangle>();

        while (true)
        {

        }
    }

    public class Rectangle
    {
        public Vector2 Position;
        public Vector2 Size;

        public Rectangle(Vector2 pos, Vector2 size)
        {
            Position = pos;
            Size = size;
        }

        public Rectangle Split(float at, bool dir)
        {
            if (dir)
            {
                Size.x -= at;
                Position.x -= at / 2;

                return new Rectangle(new Vector2(at / 2, Position.y), new Vector2(at, Size.y));
            }
            else
            {
                Size.y -= at;
                Position.y -= at / 2;

                return new Rectangle(new Vector2(Position.x, at / 2), new Vector2(Size.x, at));
            }
        }
    }
}
