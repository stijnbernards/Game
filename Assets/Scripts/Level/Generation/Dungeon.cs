using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Generate
{

    public GameObject groundSprite = Resources.Load("Ground") as GameObject;
    public GameObject wallSprite = Resources.Load("Stone") as GameObject;

    public float Rooms = 19F;

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
                GameState.Instance.GetLevel<Caves>(Difficulty, (int)Level - 1, this.ID, true);
            });

            this.startPoint = new Vector2(x, y);
        }
        else
        {
            FindRandomEmpty(out x, out y);

            map[x, y].TileNumber = 8;
            map[x, y].Name = "starttile";
            map[x, y].Action = new Tile.TileAction(() =>
            {
                GameState.Instance.Character.Behaviour.SetMoving(false);
                GameState.Instance.GetLevel<Caves>(Difficulty, 0, "DEBUG_LEVEL", true);
            });

            this.startPoint = new Vector2(x, y);
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
                GameState.Instance.GetLevel<Caves>(Difficulty, (int)Level + 1, this.ID, false);
            });

            map[x, y].TileNumber = 9;
            map[x, y].Name = "endtile";

            this.endPoint = new Vector2(x, y);
        }
    }

    public override void GenerateLevel()
    {
        DungeonRectangle rect = new DungeonRectangle(new Vector2(0, 0), new Vector2(width, height));

        rect.StartSplitting(Rooms);

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
}
