using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject hoverMenu;

    private List<string> text = new List<string>()
    {
        "hey maatje"
    };

    public void SetText(List<string> text)
    {
        this.text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverMenu = (GameObject) GameObject.Instantiate(Resources.Load("Prefabs/UI/Inventory/HoverMenu"));
        hoverMenu.transform.SetParent(GameObject.Find("Inventory").transform);
        hoverMenu.transform.position = transform.position;
        
        foreach(string txt in text)
        {
            GameObject textLine = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/Inventory/HoverMenuText"));

            textLine.GetComponent<Text>().text = txt;
            textLine.transform.SetParent(hoverMenu.transform);
        }   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(hoverMenu);
    }
}