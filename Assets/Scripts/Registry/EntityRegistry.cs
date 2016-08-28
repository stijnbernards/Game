using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityRegistry
{
    private Dictionary<string, GameObject> entities = new Dictionary<string, GameObject>();

    public void RegisterEntity(string name, GameObject entity)
    {
        entities.Add(name, entity);
    }

    public GameObject GetEntity(string name)
    {
        return entities[name];
    }
}