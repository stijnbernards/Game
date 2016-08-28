using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CharacterBehaviour : MonoBehaviour {

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

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            dir = Vector2.right;
            moving = true;
            CheckTilesVisible();
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

    #region ShadowCasting

    public void CheckTilesVisible()
    {
        float los = GameState.Instance.Character.LOS;
        List<Vector2[]> lines = new List<Vector2[]>();

        lines.AddRange(CheckSideVisible(1, los));
        lines.AddRange(CheckSideVisible(2, los));
        lines.AddRange(CheckSideVisible(3, los));
        lines.AddRange(CheckSideVisible(4, los));
        lines.AddRange(CheckSideVisible(5, los));
        lines.AddRange(CheckSideVisible(6, los));
        lines.AddRange(CheckSideVisible(7, los));
        lines.AddRange(CheckSideVisible(8, los));

        foreach(Vector2[] v in lines){
            foreach (Vector2 v2 in v)
            {
                GameObject.Instantiate(Resources.Load("Ground"), v2, Quaternion.identity);
            }
        }
    }

    public List<Vector2[]> CheckSideVisible(int side, float los)
    {
        List<Vector2[]> lines = new List<Vector2[]>();

        switch (side)
        {
            case 1:
                for (int i = 0; i < los / 2; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x + i,
                        GameState.Instance.Character.Behaviour.transform.position.y + los,
                        false
                    ));
                }
                break;
            case 2:
                for (int i = 0; i < los / 2; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x - i,
                        GameState.Instance.Character.Behaviour.transform.position.y + los,
                        false
                    ));
                }
                break;
            case 3:
                for (int i = 0; i < los / 2; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x + i,
                        GameState.Instance.Character.Behaviour.transform.position.y - los,
                        false
                    ));
                }
                break;
            case 4:
                for (int i = 0; i < los / 2; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x - i,
                        GameState.Instance.Character.Behaviour.transform.position.y - los,
                        false
                    ));
                }
                break;
            case 5:
                for (int i = 0; i < los / 2; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x + los,
                        GameState.Instance.Character.Behaviour.transform.position.y + i,
                        false
                    ));
                }
                break;
            case 6:
                for (int i = 0; i < los / 2; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x + los,
                        GameState.Instance.Character.Behaviour.transform.position.y - i,
                        false
                    ));
                }
                break;
            case 7:
                for (int i = 0; i < los / 2; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x - los,
                        GameState.Instance.Character.Behaviour.transform.position.y + i,
                        false
                    ));
                }
                break;
            case 8:
                for (int i = 0; i < los / 2; i++)
                {
                    lines.Add(GenHelpers.PlotLine(
                        GameState.Instance.Character.Behaviour.transform.position.x,
                        GameState.Instance.Character.Behaviour.transform.position.y,
                        GameState.Instance.Character.Behaviour.transform.position.x - los,
                        GameState.Instance.Character.Behaviour.transform.position.y - i,
                        false
                    ));
                }
                break;
        }

        return lines;
    }

    #endregion

    public void Kill()
    {
        Destroy(gameObject);
#if DEBUG
        Debug.Log("Oh dear you died.");
#endif
        Application.Quit();
    }
}
