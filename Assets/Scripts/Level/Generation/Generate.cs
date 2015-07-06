using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateBase : MonoBehaviour {

    public int width;
    public int height;
    [Range(0, 100)]
    public int randomFillPercent;

    public string seed;

    public GameObject startSprite;
    public GameObject endSprite;

    [HideInInspector]
    public List<Entity> entitys = new List<Entity>();
    [HideInInspector]
    public Tile[,] map;
    [HideInInspector]
    public Point startPoint;
    [HideInInspector]
    public Point endPoint;
    [HideInInspector]
    public List<int> Obstacles = new List<int>();

    void Start()
    {
        int x, y;
        GenerateLevel();
        FindRandomEmpty(out x, out y);
        map[x, y].TileNumber = 2;
        this.startPoint = new Point(x, y);
        FindRandomEmpty(out x, out y);
        map[x, y].TileNumber = 3;
        this.endPoint = new Point(x, y);
        GameState.Instance.Character.Player.transform.position = new Vector2(GameState.Instance.Map.startPoint.x, GameState.Instance.Map.startPoint.y);
        BuildLevel();
    }

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
}

/// <summary>
/// Abstract class to enforce derived classes to implement the abstract function GenerateLevel
/// </summary>
public abstract class Generate : GenerateBase {
    public abstract override void GenerateLevel();
}
