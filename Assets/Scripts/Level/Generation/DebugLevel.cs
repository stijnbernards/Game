using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugLevel : Generate
{
    public override void BeginPoint() 
    {
        this.startPoint = new Point(0, 0);
        this.endPoint = new Point(0, 0);
    }
    public override void EndPoint() { }

    public override void GenerateLevel()
    {
        GameObject door = Entity.Default;
        door.AddComponent<Door>();

        GameObject spikes = Entity.Default;
        spikes.AddComponent<Spikes>();

        GameState.Instance.EntityRegistry.RegisterEntity("door", door);
        GameState.Instance.EntityRegistry.RegisterEntity("spikes", spikes);

        int x, y;

        map = new Tile[10, 10];
        width = 10;
        height = 10;

        for (int z = 0; z < 10; z++)
        {
            for (int q = 0; q < 10; q++)
            {
                map[z, q] = new Tile()
                {
                    TileNumber = 0
                };
            }
        }
        
        SpawnEntitys();

        FindRandomEmpty(out x, out y);

        map[x, y].Action = new Tile.TileAction(() =>
        {
            UIMain.YesNoDialog("Do you want to continue to the Caves?", new System.Action(() => { 
                if(!GameState.Instance.LevelRegistry.LevelExists("CAVES")){
                    GameState.Instance.GetLevel<Caves>(Hardness, 4, "CAVES", false); 
                }else{
                    GameState.Instance.GetLevel<Caves>(Hardness, 0, "CAVES", false); 
                }
            }));
        });

        map[x, y].TileNumber = 3;

        FindRandomEmpty(out x, out y);

        map[x, y].Action = new Tile.TileAction(() =>
        {
            UIMain.YesNoDialog("Do you want to continue to the weird chambers?", new System.Action(() =>
            {
                if (!GameState.Instance.LevelRegistry.LevelExists("CAVES_ROOMS"))
                {
                    GameState.Instance.GetLevel<CaveRooms>(Hardness, 8, "CAVES_ROOMS", false);
                }
                else
                {
                    GameState.Instance.GetLevel<CaveRooms>(Hardness, 0, "CAVES_ROOMS", false);
                }
            }));
        });

        map[x, y].TileNumber = 3;
    }

    public override void BuildLevel()
    {
        GameState.Instance.MapRenderer.DispatchMap(map);
    }

    public override void SpawnEntitys()
    {
        Entity.SpawnInWorld(new Vector2(9, 9), GameState.Instance.EntityRegistry.GetEntity("door"));
        Entity.SpawnInWorld(new Vector2(5, 5), GameState.Instance.EntityRegistry.GetEntity("spikes"));
    }
}