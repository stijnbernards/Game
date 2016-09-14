using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterBehaviour : MonoBehaviour {

    public delegate void KeydownAction();

    private Vector3 dir;
    private bool moving = false;
    private Dictionary<KeyCode, KeydownAction> actions = new Dictionary<KeyCode, KeydownAction>();

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }

    public bool AddKeyBind(KeyCode keycode, KeydownAction action)
    {
        if (actions.ContainsKey(keycode))
        {
            return false;
        }
        else
        {
            actions.Add(keycode, action);

            return true;
        }
    }

    public bool RemoveKeyBind(KeyCode keycode)
    {
        actions.Remove(keycode);

        return true;
    }

    public void Update()
    {
        if (CheckInput())
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f);
            if (hit.collider != null)
            {
                Entity entity = (Entity)hit.collider.gameObject.GetComponent(typeof(Entity));
                if (entity != null && entity.Physical)
                {
                    entity.Hit(GameState.Instance.Character.Damage);
                    GameState.Instance.Turn -= 100f / GameState.Instance.Character.AttackSpeed;
                
                    return;
                }
                else
                {
                    entity.Step();
                }
            }

            GameState.Instance.Map.LastPos = new Vector2((int)(transform.position.x + dir.x), (int)(transform.position.y + dir.y));
            GameState.Instance.Map.map[(int)(transform.position.x + dir.x), (int)(transform.position.y + dir.y)].Execute();

            if (moving && GameState.Instance.Map.Obstacles.IndexOf(GameState.Instance.Map.map[(int)(transform.position.x + dir.x), (int)(transform.position.y + dir.y)].TileNumber) == -1)
            {
                transform.position += dir;
                moving = false;
                GameState.Instance.Turn -= 100f / GameState.Instance.Character.MoveSpeed;
            }
            else
            {
                moving = false;
            }

            transform.position = GameState.Instance.Character.Player.transform.position;
            CheckTilesVisible();
        }
    }

    bool CheckInput()
    {
        foreach (KeyCode key in actions.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                actions[key].Invoke();

                return false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            //GameState.Instance.Character.DebugLog();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f);

            if (hit.collider != null)
            {
                Entity entity = (Entity)hit.collider.gameObject.GetComponent(typeof(Entity));
                
                if (entity != null)
                {
                    entity.Interact();
                    GameState.Instance.Turn -= 100f / GameState.Instance.Character.AttackSpeed;
                }
            }

            return false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject UI = GameObject.Find("LevelUI") as GameObject;
            if (UI != null)
            {
                Destroy(UI);
            }
            else
            {
                GameObject go = (GameObject)(GameObject.Instantiate(Resources.Load("LevelUI"), new Vector3(0, 0, 0), Quaternion.identity));
                go.transform.SetParent(GameObject.Find("Canvas").transform, false);
                go.name = "LevelUI";
            }
            return false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject Inventory = GameObject.Find("Inventory") as GameObject;

            if (Inventory.GetComponent<CanvasGroup>().alpha == 1)
            {
                Inventory.GetComponent<CanvasGroup>().alpha = 0;
                Inventory.GetComponent<CanvasGroup>().interactable = false;
                Inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                Inventory.GetComponent<CanvasGroup>().alpha = 1;
                Inventory.GetComponent<CanvasGroup>().interactable = true;
                Inventory.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

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
        Application.Quit();
    }
}
