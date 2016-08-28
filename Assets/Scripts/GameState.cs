using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{

    #region Properties
    public Generate Map;
    public MapRenderer MapRenderer;

    public EntityRegistry EntityRegistry;
    public LevelRegistry LevelRegistry;

    public float Level = 1;

    public double Turn
    {
        get
        {
            return this.turn;
        }
        set
        {
            this.turn = value;
            if (this.turn <= 0)
            {
                //TODO: NPC handler stuff...
                NPCHandler();
                GameState.Instance.Character.CharUpdate();
                this.turn += 100;
            }

        }
    }
    public Character Character
    {
        get
        {
            return this.character;
        }
        set
        {
            this.character = value;
        }
    }
    public static GameState Instance{
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameState").AddComponent<GameState>();
            }
            return instance;
        }
    }
    #endregion

    private Character character;
    private static GameState instance;
    private double turn = 100.0;

    public void OnApplicationQuit()
    {
        instance = null;
    }

    public void StartState()
    {
        EntityRegistry = new EntityRegistry();
        LevelRegistry = new LevelRegistry();

        EntityRegistry.RegisterEntity("Spoder", Resources.Load("Spoder") as GameObject);
        new Character(new Rogue(), new Halfling());
        MapRenderer = new MapRenderer();
        //Disabled for debugging purposes
        //Application.LoadLevel("Game");
    }

    private void NPCHandler()
    {
        foreach (GameObject ent in GameState.Instance.Map.entities)
        {
            if (ent != null)
            {
                Entity mob = (Entity)ent.GetComponent(typeof(Entity));
                mob.Action();
            }
        }
    }

    public void GetLevel<T>(float Hardness, int level, string identifier, bool end) where T : Generate, new()
    {
        Destroy(GameObject.Find("Map"));

        if (GameState.instance.Map != null)
        {
            foreach (GameObject ent in GameState.instance.Map.entities)
            {
                Destroy(ent);
            }
        }

        if (LevelRegistry.LevelExists(identifier))
        {
            LevelRegistry.GetMap(identifier, level).ContinueLevel(end);
        }
        else
        {
            LevelRegistry.AddLevels<T>(identifier, level);
            LevelRegistry.GetMap(identifier, 0).ContinueLevel(end);
        }


        //UIMain.GameLog("Entered level " + Hardness);
        //UIMain.SetLevel() ;
        //new T().StartGen(Hardness);
    }

    //public void EnterLevel<T>(float Hardness, float levels, string identifier) where T : GenerateBase, new()
    //{
    //    Destroy(GameObject.Find("Map"));

    //    foreach (GameObject ent in GameState.instance.Map.entities)
    //    {
    //        Destroy(ent);
    //    }

    //    UIMain.GameLog("Entered level " + identifier);
    //    new T().StartGen(Hardness, levels);
    //}

    public void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
}
