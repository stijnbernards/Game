using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Generate
{

    public GameObject groundSprite = Resources.Load("Ground") as GameObject;
    public GameObject wallSprite = Resources.Load("Stone") as GameObject;

    public override void BeginPoint()
    {
        int x, y;

        if (this.Level != 0)
        {
            FindRandomEmpty(out x, out y);
            map[x, y].TileNumber = 8;
            map[x, y].Name = "starttile";
            map[x, y].Action = new Tile.TileAction(() =>
            {
                GameState.Instance.Character.Behaviour.SetMoving(false);
                GameState.Instance.GetLevel<Caves>(Hardness, (int)Level - 1, this.ID, true);
            });
            this.startPoint = new Point(x, y);
        }
        else
        {
            FindRandomEmpty(out x, out y);

            map[x, y].TileNumber = 8;
            map[x, y].Name = "starttile";
            map[x, y].Action = new Tile.TileAction(() =>
            {
                GameState.Instance.Character.Behaviour.SetMoving(false);
                GameState.Instance.GetLevel<Caves>(Hardness, 0, "DEBUG_LEVEL", true);
            });

            this.startPoint = new Point(x, y);
        }
    }

    public override void EndPoint()
    {
        int x, y;

        if (this.Level + 1 < GameState.Instance.LevelRegistry.LevelCount(this.ID))
        {
            FindRandomEmpty(out x, out y);

            map[x, y].Action = new Tile.TileAction(() =>
            {
                GameState.Instance.Character.Behaviour.SetMoving(false);
                GameState.Instance.GetLevel<Caves>(Hardness, (int)Level + 1, this.ID, false);
            });

            map[x, y].TileNumber = 9;
            map[x, y].Name = "endtile";

            this.endPoint = new Point(x, y);
        }
    }

    public override void GenerateLevel()
    {
        DungeonRectangle rect = new DungeonRectangle(new Vector2(0, 0), new Vector2(100, 100));

        rect.StartSplitting();

        Tile[,] map = new Tile[(int)rect.Size.x, (int)rect.Size.y];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = new Tile()
                {
                    TileNumber = 1
                };
            }
        }

        foreach (Vector2 vect in rect.Visualize())
        {
            map[(int)Mathf.Abs(vect.x), (int)Mathf.Abs(vect.y)].TileNumber = 0;
        }


        this.map = map;

        this.Obstacles.Add(1);
    }

    public override void BuildLevel()
    {

        GameState.Instance.MapRenderer.DispatchMap(map);
    }

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

        public void StartSplitting()
        {
            List<DungeonRectangle> rectangles = new List<DungeonRectangle>();

            rectangles.Add(this);

            while (rectangles.Count < 19)
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
                float left =  Position.x + (Size.x / 2) - (width / 2);
                float top = Position.y - (Size.y / 2) + (height / 2);

                Vector2 topLeft = new Vector2(Position.x, Position.y);
                Vector2 topRight = new Vector2(Position.x + Size.x, Position.y);
                Vector2 bottomLeft = new Vector2(Position.x, Position.y - Size.y);
                Vector2 bottomRight = new Vector2(Position.x + Size.x, Position.y - Size.y);

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
            //1 == east; 2 == west; 3 == north; 4 == south; 
            //this fucking means that this object is east etc of the connector!!!
            //probably not needed lol fail
            //float orientation = (Position.x != connector.Position.x ? (Position.x > connector.Position.x ? 1 : 2 ) : (Position.y > connector.Position.y ?  3 : 4));
            return GenHelpers.PlotLine(Position.x + Size.x / 2, Position.y - Size.y / 2, connector.Position.x + connector.Size.x / 2, connector.Position.y  - connector.Size.y / 2, true);
        }
    }
}
