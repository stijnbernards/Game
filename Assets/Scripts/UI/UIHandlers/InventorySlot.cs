using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public static GameObject GetEmptySlot()
    {
        if (inventorySlots.Count == 0)
        {
            inventorySlots = GameObject.FindGameObjectsWithTag("Slot").ToList<GameObject>();
        }

        foreach (GameObject go in inventorySlots)
        {
            if (go.transform.childCount == 0)
            {
                return go;
            }
        }

        return null;
    }

    private static List<GameObject> inventorySlots = new List<GameObject>();

    public GameObject ItemInSlot
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
             
            return null;
        }
    }

    private bool enabled = true;

    public void OnDrop(PointerEventData eventData)
    {
        if (!ItemInSlot && enabled)
        {
            if (Draggable.CurrentDrag.GetComponent<Item>().Equipped)
            {
                Draggable.CurrentDrag.GetComponent<Item>().UnEquip();
                Draggable.CurrentDrag.GetComponent<Item>().Actions.Add("Equip", (item, parent) =>
                {
                    Item.EquipItem(item, parent);
                });

                Draggable.CurrentDrag.GetComponent<Item>().Actions.Remove("Unequip");
            }

            Draggable.CurrentDrag.transform.SetParent(transform);
        }
        else if (enabled)
        {
            if (Draggable.CurrentDrag.transform.parent.GetComponent<EquipmentSlot>() != null && ItemInSlot.GetComponent<Item>().CanEquip() && Draggable.CurrentDrag.transform.parent.GetComponent<EquipmentSlot>().SlotType == ItemInSlot.GetComponent<Item>().EquipmentSlotType)
            {
                if (Draggable.CurrentDrag.GetComponent<Item>().Equipped)
                {
                    Draggable.CurrentDrag.GetComponent<Item>().UnEquip();
                    Draggable.CurrentDrag.GetComponent<Item>().Actions.Add("Equip", (item, parent) =>
                    {
                        Item.EquipItem(item, parent);
                    });

                    Draggable.CurrentDrag.GetComponent<Item>().Actions.Remove("Unequip");
                }

                if (ItemInSlot.GetComponent<Item>().CanEquip())
                {
                    ItemInSlot.GetComponent<Item>().Equip();
                    ItemInSlot.GetComponent<Item>().Actions.Add("Unequip", (item, parent) =>
                    {
                        Item.EquipItem(item, parent);
                    });

                    ItemInSlot.GetComponent<Item>().Actions.Remove("Equip");
                }
                else
                {
                    Draggable.CurrentDrag.GetComponent<Item>().Equip();
                    return;
                }
            }
            else if (Draggable.CurrentDrag.transform.parent.GetComponent<EquipmentSlot>() == null)
            {
                ItemInSlot.transform.SetParent(Draggable.CurrentDrag.transform.parent);
                Draggable.CurrentDrag.transform.SetParent(transform);
                return;
            }
            else
            {
                return;
            }

            ItemInSlot.transform.SetParent(Draggable.CurrentDrag.transform.parent);

            Draggable.CurrentDrag.transform.SetParent(transform);
        }
    }
}
