using UnityEngine;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour {

    private Vector2 pos;
    private bool moving = false;

    public void Update()
    {
        CheckInput();
        if (moving && GameState.Instance.Map[(int)pos.x, (int)pos.y] == 0)
        {
            transform.position = pos;
            moving = false;
        }
        else
        {
            moving = false;
        }
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
