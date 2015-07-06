using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterBehaviour : MonoBehaviour {

    private Vector2 pos;
    private bool moving = false;

    public void Update()
    {
        CheckInput();
        Entity ent = (from Entity e in GameState.Instance.Map.entitys where (Vector2)e.entity.transform.position == pos select e).First();
        Debug.Log(ent.entity.transform.position);
        if(ent != null)
        {
            Debug.Log("Godverju");
        }
        else if (moving && GameState.Instance.Map.Obstacles.IndexOf(GameState.Instance.Map.map[(int)pos.x, (int)pos.y].TileNumber) == -1)
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
