using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRegistry : IEnumerable<KeyValuePair<string, GameObject>>
{
    public static EntityRegistry GetEntityRegistry()
    {
        if (GameState.Instance != null && GameState.Instance.EntityRegistry != null)
        {
            return GameState.Instance.EntityRegistry;
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
            return entities[i];
        }
    }

    private Dictionary<string, GameObject> entities = new Dictionary<string, GameObject>();

    public void RegisterEntity(string name, GameObject entity)
    {
        entities.Add(name, entity);
    }

    public GameObject GetEntity(string name)
    {
        return entities[name];
    }

    public IEnumerator<KeyValuePair<string, GameObject>> GetEnumerator()
    {
        return entities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return entities.GetEnumerator();
    }
}