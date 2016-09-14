using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntity : Entity
{
    void Start()
    {
        Physical = false;
    }

    public override void Step()
    {
        GameObject go = InventorySlot.GetEmptySlot();

        if (go == null)
        {
            return;
        }

        GameObject item = Item.Default;
        gameObject.GetComponent<Item>().Actions = new Dictionary<string, Action<GameObject, GameObject>>();
        item.AddComponent<Item>().GetCopyOf<Item>(gameObject.GetComponent<Item>());
        item.transform.SetParent(go.transform);

        Destroy(gameObject);
    }

    public void Drop(Vector2 pos)
    {
        gameObject.transform.SetParent(null);
        gameObject.transform.position = pos;
    }
}