using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{

    #region Properties
    public Generate Map;

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
                this.turn = 100 - this.turn;
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
        this.character = new Character(new Rogue(), new Halfling());
        //Disabled for debugging purposes
        //Application.LoadLevel("Game");
    }

    private void NPCHandler()
    {
        foreach (GameObject ent in GameState.Instance.Map.entitys)
        {
            Entity mob = (Entity)ent.GetComponent(typeof(Entity));
            mob.Action();
        }
    }
}
