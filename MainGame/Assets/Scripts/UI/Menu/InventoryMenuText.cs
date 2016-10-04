using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class InventoryMenuText : MonoBehaviour, IPointerClickHandler
{
    private string text;
    private Action<GameObject, GameObject> action;
    private GameObject item, parent;

    public void SetData(string text, Action<GameObject, GameObject> action, GameObject item, GameObject parent)
    {
        this.text = text;
        this.action = action;
        this.item = item;
        this.parent = parent;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        action.Invoke(item, parent);
        transform.parent.gameObject.GetComponent<InventoryMenu>().OnPointerExit(null);
    }
}
