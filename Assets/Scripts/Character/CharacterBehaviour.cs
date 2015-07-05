using UnityEngine;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour {

    private Vector2 pos;
    private bool moving = false;

    public void Update()
    {
        CheckInput();
        if (moving && GameState.Instance.Map.Obstacles.IndexOf(GameState.Instance.Map.map[(int)pos.x, (int)pos.y].TileNumber) == -1)
        {
            transform.position = pos;
            moving = false;
        }
        else
        {
            moving = false;
        }
        this.pos = GameState.Instance.Character.Player.transform.position;
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            pos += Vector2.right;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pos -= Vector2.right;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            pos += Vector2.up;
            moving = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            pos -= Vector2.up;
            moving = true;
        }
    }
}
