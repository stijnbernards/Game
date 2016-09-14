using System;
using UnityEngine.UI;
using UnityEngine;

public partial class Item
{
    public static GameObject Default
    {
        get
        {
            GameObject go = new GameObject();
            //Destroy(go);
            go.name = "DefaultItem";
            go.AddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Stone");
            go.AddComponent<CanvasGroup>();
            go.AddComponent<Draggable>();
            return go;
        }
    }

    public static void EquipItem(GameObject itemToEquip, GameObject slot)
    {
        GameObject equipmentSlot = EquipmentSlot.GetSlotByType((int)itemToEquip.GetComponent<Item>().EquipmentSlotType);

        Draggable.CurrentDrag = itemToEquip;
        equipmentSlot.GetComponent<EquipmentSlot>().OnDrop(null);
    }

    public static void UnequipItem(GameObject itemToEquip, GameObject slot)
    {
        GameObject inventorySlot = InventorySlot.GetEmptySlot();

        if(inventorySlot == null)
        {
            return;
        }

        Draggable.CurrentDrag = itemToEquip;

        inventorySlot.GetComponent<InventorySlot>().OnDrop(null);
    }

    public static void DropItem(GameObject item)
    {
        GameObject go = Entity.Default;
        go.AddComponent<ItemEntity>().Drop(GameState.Instance.Character.Behaviour.transform.position);
        go.AddComponent<Item>().GetCopyOf<Item>(item.GetComponent<Item>());

        Instantiate(go);

        Destroy(item);
    }
}