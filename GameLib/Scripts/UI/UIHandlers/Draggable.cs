using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    public static GameObject CurrentDrag;
    Vector3 startPosition;
    Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        CurrentDrag = gameObject;
        startPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        startParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CurrentDrag = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if(transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }
	
}
