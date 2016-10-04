using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class InventoryMenu : MonoBehaviour, IPointerExitHandler
{
    public List<GameObject> ClickObjects = new List<GameObject>();

    public static GameObject RightClickMenu(Dictionary<string, Action<GameObject, GameObject>> actions, GameObject item)
    {
        GameObject menu = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/Inventory/RightClickMenu"));
        menu.GetComponent<InventoryMenu>().SetClickActions(actions, item);

        return menu;
    }

    private void Start()
    {
        transform.position = Input.mousePosition;
        transform.position = new Vector2(transform.position.x + -5, transform.position.y + 5);
        transform.SetParent(GameObject.Find("Inventory").transform);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Slot"))
        {
            go.GetComponent<Image>().raycastTarget = false;
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EquipmentSlot"))
        {
            go.GetComponent<Image>().raycastTarget = false;
        }
    }

    public void SetClickActions(Dictionary<string, Action<GameObject, GameObject>> actions, GameObject item)
    {
        foreach (KeyValuePair<string, Action<GameObject, GameObject>> kv in actions)
        {
            GameObject clickObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/Inventory/RightClickMenuText"));
            
            clickObject.GetComponent<Text>().text = kv.Key;
            clickObject.GetComponent<InventoryMenuText>().SetData(kv.Key, kv.Value, item, item.transform.parent.gameObject);
            clickObject.transform.SetParent(transform);
            ClickObjects.Add(clickObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Slot"))
        {
            go.GetComponent<Image>().raycastTarget = true;
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EquipmentSlot"))
        {
            go.GetComponent<Image>().raycastTarget = true;
        }

        Destroy(gameObject);
    }
}