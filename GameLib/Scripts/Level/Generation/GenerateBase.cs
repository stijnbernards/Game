using System.Collections.Generic;
using UnityEngine;

public class GenerateBase
{

    public int width = 100;
    public int height = 100;
    [Range(0, 100)]
    public int randomFillPercent = 50;
    public float Difficulty;
    public float Level;
    public string ID;
    public Vector2 LastPos;

    public string seed;

    public GameObject startSprite = Resources.Load("StartPoint") as GameObject;
    public GameObject endSprite = Resources.Load("EndPoint") as GameObject;

    public List<GameObject> entities = new List<GameObject>();
    public List<EntityItem> EntityList = new List<EntityItem>();

    public Tile[,] map;

    public Vector2 startPoint;
    public Vector2 endPoint;

    public List<int> Obstacles = new List<int>();

    public void FindRandomEmpty(out int x, out int y)
    {
        while (true)
        {
            x = Random.Range(0, width);
            y = Random.Range(0, height);
            if (map[x, y].TileNumber == 0)
                return;
        }
    }

    public virtual void GenerateLevel() { }
    public virtual void BuildLevel()
    {
        GameState.Instance.MapRenderer.DispatchMap(map);
    }

    public virtual void SpawnEntitys()
    {
        int x, y;

        foreach (EntityItem ei in EntityList)
        {
            for (int i = 0; i < ei.Amount; i++)
            {
                FindRandomEmpty(out x, out y);
                this.entities.Add(Entity.SpawnInWorld(new Vector2(x, y), GameState.Instance.EntityRegistry.GetEntity(ei.Ent)));
            }
        }
    }

    public virtual Generate StartGen(float level, string id) { return null; }
    public Tile GetTileSafe(float x, float y)
    {
        if (x < map.GetLength(0) && x > 0 && y < map.GetLength(1) && y > 0)
        {
            return map[(int)x, (int)y];
        }
        else
        {
            return new Tile()
            {
                TileNumber = 1
            };
        }
    }
}