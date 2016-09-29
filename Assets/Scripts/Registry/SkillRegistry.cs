using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SkillRegistry : IEnumerable<KeyValuePair<string, GameObject>>
{
    public static SkillRegistry GetSkillRegistry()
    {
        if (GameState.Instance != null && GameState.Instance.SkillRegistry != null)
        {
            return GameState.Instance.SkillRegistry;
        }
        else
        {
            return null;
        }
    }

    public GameObject this[string i]
    {
        get
        {
            return skills[i];
        }
    }

    private Dictionary<string, GameObject> skills = new Dictionary<string, GameObject>();

    public void RegisterSkill(string identifier, GameObject skill)
    {
        if (skills.ContainsKey(identifier))
        {
            //Throw exeception
        }
        else
        {
            skills.Add(identifier, skill);
        }
    }

    public GameObject GetSkill(string identifier)
    {
        if (skills.ContainsKey(identifier))
        {
            return skills[identifier];
        }
        else
        {
            return null;
        }
    }

    public IEnumerator<KeyValuePair<string, GameObject>> GetEnumerator()
    {
        return skills.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return skills.GetEnumerator();
    }
}