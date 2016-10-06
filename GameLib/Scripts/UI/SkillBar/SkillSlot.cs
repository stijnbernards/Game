using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IDropHandler
{
    public KeyCode KeyCode;
    public SkillItem SkillItem;

    private GameObject skill;

    public void OnDrop(PointerEventData eventData)
    {
        if (Draggable.CurrentDrag.GetComponent<SkillItem>() != null)
        {
            Destroy(skill);

            Draggable.CurrentDrag.transform.SetParent(gameObject.transform);
            
            skill = Draggable.CurrentDrag;
            skill.GetComponent<SkillItem>().Execute();
            this.SkillItem = skill.GetComponent<SkillItem>();
        }
    }
}