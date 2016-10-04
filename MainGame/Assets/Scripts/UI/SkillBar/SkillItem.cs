using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    public static GameObject Default
    {
        get
        {
            GameObject go = new GameObject();
            //Destroy(go);
            go.name = "DefaultSkill";
            go.AddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Stone");
            go.AddComponent<CanvasGroup>();
            go.AddComponent<Draggable>();
            go.AddComponent<SkillItem>();

            return go;
        }
    }

    private Action skill;
    private Dictionary<string, float> requirements = new Dictionary<string, float>();
    private Dictionary<string, float> modifiers = new Dictionary<string, float>();

    public void SetRequirements(Dictionary<string, float> requirements)
    {
        this.requirements = requirements;
    }

    public void SetModifiers(Dictionary<string, float> modifiers)
    {
        this.modifiers = modifiers;
    }

    public void SetAction(Action action)
    {
        skill = action;
    }

    public void Execute()
    {
        foreach (KeyValuePair<string, float> kv in requirements)
        {
            if (kv.Value < GameState.Instance.Character.GetStatByString(kv.Key))
            {
                return;
            }
        }

        foreach (KeyValuePair<string, float> kv in requirements)
        {
            GameState.Instance.Character.SetStatByString(kv.Key, kv.Value);
        }

        skill.Invoke();
    }
}