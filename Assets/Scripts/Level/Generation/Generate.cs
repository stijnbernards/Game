using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GenerateBase {

    public int width = 100;
    public int height = 100;
    [Range(0, 100)]
    public int randomFillPercent = 50;
    public float Hardness;
    public float Level;
    public string ID;
    public Vector2 LastPos;

    public string seed;

    public GameObject startSprite = Resources.Load("StartPoint") as GameObject;
    public GameObject endSprite = Resources.Load("EndPoint") as GameObject;

    public List<GameObject> entities = new List<GameObject>();

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

/// <summary>
/// Abstract class to enforce derived classes to implement the abstract function GenerateLevel
/// </summary>
public abstract class Generate : GenerateBase {

    public abstract override void GenerateLevel();

    public override Generate StartGen(float level, string id)
    {
        this.ID = id;
        GameState.Instance.Map = this;
        this.Hardness = level;
        this.Level = level;
        UIMain.SetLevel();
        GenerateLevel();
        BeginPoint();
        EndPoint();
        BuildLevel();
        //GameState.Instance.Character.Player.transform.position = new Vector2(GameState.Instance.Map.startPoint.x, GameState.Instance.Map.startPoint.y);       
        return this;
    }

    //int x, y;
    //FindRandomEmpty(out x, out y);
    //map[x, y].TileNumber = 2;
    //this.startPoint = new Point(x, y);
    public abstract void BeginPoint();

    //int x, y;
    //FindRandomEmpty(out x, out y);
    //map[x, y].TileNumber = 3;
    //this.endPoint = new Point(x, y);
    public abstract void EndPoint();

    public void ContinueLevel(bool start)
    {
        BuildLevel();

        GameState.Instance.Map = this;

        if (start)
        {
            GameState.Instance.Character.Player.transform.position = LastPos;
            GameState.Instance.Character.Behaviour.CheckTilesVisible();

            return;
        }
        else
        {
            GameState.Instance.Character.Player.transform.position = new Vector2(this.startPoint.x, this.startPoint.y);
            GameState.Instance.Character.Behaviour.CheckTilesVisible();
            
            return;
        }
    }
}