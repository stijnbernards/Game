using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class to enforce derived classes to implement the abstract function GenerateLevel
/// </summary>
public abstract class Generate : GenerateBase {

    public abstract override void GenerateLevel();

    public override Generate StartGen(float level, string id)
    {
        this.ID = id;
        GameState.Instance.Map = this;
        this.Difficulty = level;
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