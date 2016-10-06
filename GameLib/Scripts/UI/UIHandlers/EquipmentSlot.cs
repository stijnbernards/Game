using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    public static GameObject GetSlotByType(int type)
    {
        if (equipmentSlots.Count > 0)
        {
            return equipmentSlots[type];
        }
        else
        {
            equipmentSlots = GameObject.FindGameObjectsWithTag("EquipmentSlot").OrderBy(x => x.GetComponent<EquipmentSlot>().SlotType).ToList<GameObject>();
            
            return equipmentSlots[type];
        }
    }

    private static List<GameObject> equipmentSlots = new List<GameObject>();

    public int SlotType;

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

    private bool slotEnabled = true;

    public void OnDrop(PointerEventData eventData)
    {
        if (!ItemInSlot && slotEnabled && Draggable.CurrentDrag.GetComponent<Item>().EquipmentSlotType == SlotType && Draggable.CurrentDrag.GetComponent<Item>().CanEquip())
        {
            Draggable.CurrentDrag.transform.SetParent(transform);

            Draggable.CurrentDrag.GetComponent<Item>().Actions.Add("Unequip", (item, parent) =>
            {
                Item.UnequipItem(item, parent);
            });

            Draggable.CurrentDrag.GetComponent<Item>().Actions.Remove("Equip");

            Draggable.CurrentDrag.GetComponent<Item>().Equip();
        }
        else if (slotEnabled && Draggable.CurrentDrag.GetComponent<Item>().EquipmentSlotType == SlotType && Draggable.CurrentDrag.GetComponent<Item>().CanEquip())
        {
            ItemInSlot.GetComponent<Item>().UnEquip();

            if (Draggable.CurrentDrag.GetComponent<Item>().CanEquip())
            {
                ItemInSlot.GetComponent<Item>().Actions.Add("Equip", (item, parent) =>
                {
                    Item.EquipItem(item, parent);
                });

                ItemInSlot.GetComponent<Item>().Actions.Remove("Unequip");

                ItemInSlot.transform.SetParent(Draggable.CurrentDrag.transform.parent);

                Draggable.CurrentDrag.transform.SetParent(transform);
                Draggable.CurrentDrag.GetComponent<Item>().Actions.Add("Unequip", (item, parent) =>
                {
                    Item.UnequipItem(item, parent);
                });

                Draggable.CurrentDrag.GetComponent<Item>().Actions.Remove("Equip");
                Draggable.CurrentDrag.GetComponent<Item>().Equip();
            }
            else
            {
                ItemInSlot.GetComponent<Item>().Equip();
            }
        }
    }
}
