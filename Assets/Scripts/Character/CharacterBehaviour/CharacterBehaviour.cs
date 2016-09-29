using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterBehaviour : MonoBehaviour {

    public delegate bool KeydownAction();

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

    public bool AddKeyBind(KeyCode[] keycode, KeydownAction action)
    {
        for (int i = 0; i < keycode.Length; i++)
        {
            if (!AddKeyBind(keycode[i], action))
            {
                return false;
            }
        }

        return true;
    }

    public bool RemoveKeyBind(KeyCode keycode)
    {
        actions.Remove(keycode);

        return true;
    }

    public void Start()
    {
        RegisterKeyBinds();
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
                return actions[key].Invoke();
            }
        }

        return false;
    }

    public void RegisterKeyBinds()
    {
        AddKeyBind(new KeyCode[] { KeyCode.Keypad8, KeyCode.W, KeyCode.UpArrow }, new KeydownAction(
            () =>
            {
                dir = Vector2.up;
                moving = true;
                return true;
            }
        ));

        AddKeyBind(new KeyCode[] { KeyCode.Keypad2, KeyCode.DownArrow, KeyCode.S }, new KeydownAction(
            () =>
            {
                dir = Vector2.down;
                moving = true;
                return true;
            }
        ));

        AddKeyBind(new KeyCode[] { KeyCode.Keypad4, KeyCode.A, KeyCode.LeftArrow }, new KeydownAction(
            () =>
            {
                dir = Vector2.left;
                moving = true;
                return true;
            }
        ));

        AddKeyBind(new KeyCode[] { KeyCode.Keypad6, KeyCode.D, KeyCode.RightArrow }, new KeydownAction(
            () =>
            {
                dir = Vector2.right;
                moving = true;
                return true;
            }
        ));

        AddKeyBind(KeyCode.Keypad9, new KeydownAction(
            () =>
            {
                dir = new Vector2(1, 1);
                moving = true;
                return true;
            }
        ));

        AddKeyBind(KeyCode.Keypad7, new KeydownAction(
            () =>
            {
                dir = new Vector2(-1, 1);
                moving = true;
                return true;
            }
        ));

        AddKeyBind(KeyCode.Keypad1, new KeydownAction(
            () =>
            {
                dir = new Vector2(-1, -1);
                moving = true;
                return true;
            }
        ));

        AddKeyBind(KeyCode.Keypad3, new KeydownAction(
            () =>
            {
                dir = new Vector2(1, -1);
                moving = true;
                return true;
            }
        ));

        AddKeyBind(KeyCode.Keypad5, new KeydownAction(
            () =>
            {
                dir = new Vector2(0, 0);
                moving = true;
                return true;
            }
        ));

        AddKeyBind(KeyCode.Space, new KeydownAction(
            () =>
            {
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
        ));

        AddKeyBind(KeyCode.P, new KeydownAction(
            () =>
            {
                GameObject skillTree = GameObject.Find("SkillTree") as GameObject;

                if (skillTree.GetComponent<CanvasGroup>().alpha == 1)
                {
                    skillTree.GetComponent<CanvasGroup>().alpha = 0;
                    skillTree.GetComponent<CanvasGroup>().interactable = false;
                    skillTree.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
                else
                {
                    skillTree.GetComponent<CanvasGroup>().alpha = 1;
                    skillTree.GetComponent<CanvasGroup>().interactable = true;
                    skillTree.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                return false;
            }
        ));

        AddKeyBind(KeyCode.E, new KeydownAction(
            () =>
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

                return false;
            }
        ));

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("SkillSlot"))
        {
            if (go.GetComponent<SkillSlot>() != null)
            {
                //Won't work without this line.
                GameObject go2 = go;

                AddKeyBind(go.GetComponent<SkillSlot>().KeyCode, new KeydownAction(
                    () => 
                    {
                        if (go2.GetComponent<SkillSlot>().SkillItem != null)
                        {
                            go2.GetComponent<SkillSlot>().SkillItem.Execute(); 
                        }
                        return false;
                    }
                ));
            }
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
