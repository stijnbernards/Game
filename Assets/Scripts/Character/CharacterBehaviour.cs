using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterBehaviour : MonoBehaviour {

    private Vector3 dir;
    private bool moving = false;

    public void Update()
    {
        if (CheckInput())
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f);
            if (hit.collider != null)
            {
                Entity entity = (Entity)hit.collider.gameObject.GetComponent(typeof(Entity));
                entity.Hit(GameState.Instance.Character.Damage);
                GameState.Instance.Turn -= 100;
            }
            else if (moving && GameState.Instance.Map.Obstacles.IndexOf(GameState.Instance.Map.map[(int)(transform.position.x + dir.x), (int)(transform.position.y + dir.y)].TileNumber) == -1)
            {
                transform.position += dir;
                moving = false;
                GameState.Instance.Turn -= 100;
            }
            else
            {
                moving = false;
            }
            transform.position = GameState.Instance.Character.Player.transform.position;
        }
    }

    bool CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            dir = Vector2.right;
            moving = true;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dir = Vector2.left;
            moving = true;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            dir = Vector2.up;
            moving = true;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            dir = Vector2.down;
            moving = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
#if DEBUG
        Debug.Log("Oh dear you died.");
#endif
    }
}
