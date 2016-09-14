using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public partial class Item : MonoBehaviour, IPointerClickHandler
{
    public Dictionary<string, Action<GameObject, GameObject>> Actions = new Dictionary<string, Action<GameObject, GameObject>>();

    public Dictionary<string, float> Requirements = new Dictionary<string, float>();

    public Dictionary<string, float> Modifiers = new Dictionary<string, float>();
    
    public bool Equipped = false;

    public void Start()
    {
        Actions.Add("Equip", (item, parent) => 
        { 
            Item.EquipItem(item, parent); 
        });

        Actions.Add("Drop", (item, parent) =>
        {
            Actions = new Dictionary<string, Action<GameObject, GameObject>>();
            Item.DropItem(item);
        });
    }

    public float Value
    {
        get
        {
            return this.value;
        }
    }

    public float EquipmentSlotType
    {
        get
        {
            return this.equipmentSlot;
        }
        set
        {
            this.equipmentSlot = value;
        }
    }

    private float value = 0;
    private float equipmentSlot = 0;

    public bool CanEquip()
    {
        foreach (KeyValuePair<string, float> kv in Requirements)
        {
            if (GameState.Instance.Character.GetStatByString(kv.Key) < kv.Value)
            {
                return false;
            }
        }

        return true;
    }

    public void Equip()
    {
        foreach (KeyValuePair<string, float> kv in Modifiers)
        {
            if (!GameState.Instance.Character.SetStatByString(kv.Key, GameState.Instance.Character.GetStatByString(kv.Key) + kv.Value))
            {
                //Exception
            }
        }

        Equipped = true;
    }
    
    public void UnEquip()
    {
        foreach (KeyValuePair<string, float> kv in Modifiers)
        {
            if (kv.Value < 0)
            {
                if (!GameState.Instance.Character.SetStatByString(kv.Key, GameState.Instance.Character.GetStatByString(kv.Key) + Mathf.Abs(kv.Value)))
                {
                    //Exception
                }
            }
            else
            {
                if (!GameState.Instance.Character.SetStatByString(kv.Key, GameState.Instance.Character.GetStatByString(kv.Key) - kv.Value))
                {
                    //Exception
                }
            }
        }

        Equipped = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameObject rcm = InventoryMenu.RightClickMenu(Actions, gameObject);
        }
    }

    public GameObject ToItemEntity()
    {
        return gameObject;
    }
}