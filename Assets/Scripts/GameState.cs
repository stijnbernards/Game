using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

    //The game instance
    private static GameState instance;

    public int[,] Map;

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

    private Character character;


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
}
