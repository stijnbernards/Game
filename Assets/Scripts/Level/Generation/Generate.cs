using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateBase {

    public int width = 100;
    public int height = 100;
    [Range(0, 100)]
    public int randomFillPercent = 50;
    public float Hardness;

    public string seed;

    public GameObject startSprite = Resources.Load("StartPoint") as GameObject;
    public GameObject endSprite = Resources.Load("EndPoint") as GameObject;

    public List<GameObject> entitys = new List<GameObject>();

    public Tile[,] map;

    public Point startPoint;
    public Point endPoint;

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
    public virtual void BuildLevel() { }
    public virtual void SpawnEntitys() { }
    public virtual void StartGen(float Level) { }
}

/// <summary>
/// Abstract class to enforce derived classes to implement the abstract function GenerateLevel
/// </summary>
public abstract class Generate : GenerateBase {
    public abstract override void GenerateLevel();

    public override void StartGen(float level)
    {
        GameState.Instance.Map = this;
        this.Hardness = level;
        UIMain.SetLevel();
        int x, y;
        GenerateLevel();
        FindRandomEmpty(out x, out y);
        map[x, y].TileNumber = 2;
        this.startPoint = new Point(x, y);
        FindRandomEmpty(out x, out y);
        map[x, y].TileNumber = 3;
        this.endPoint = new Point(x, y);
        BuildLevel();
        GameState.Instance.Character.Player.transform.position = new Vector2(GameState.Instance.Map.startPoint.x, GameState.Instance.Map.startPoint.y);
    }
}