using UnityEngine;
using System.Collections;

public partial class GameState : MonoBehaviour
{

    #region Properties
    public Generate Map;
    public MapRenderer MapRenderer;

    public EntityRegistry EntityRegistry;
    public LevelRegistry LevelRegistry;
    public SkillRegistry SkillRegistry;
    public CategoryRegistry CategoryRegistry;

    public Anvil Anvil;

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
        Register();
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

    public void GetLevel<T>(float difficulty, int level, string identifier, bool end) where T : Generate, new()
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
            LevelRegistry.AddLevels<T>(identifier, level, difficulty);
            LevelRegistry.GetMap(identifier, 0).ContinueLevel(end);
        }
    }

    public void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
}
