using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelRegistry
{
    private Dictionary<string, List<Generate>> levels = new Dictionary<string, List<Generate>>();

    public void AddLevels<T>(string name, int amount) where T : Generate, new()
    {
        if (levels.ContainsKey(name))
        {
            throw new Exception();
        }
        else
        {
            levels.Add(name, new List<Generate>());

            for (int i = 0; i < amount; i++)
            {
                levels[name].Add(new T());
            }

            for (int i = 0; i < amount; i++)
            {
                levels[name][i].StartGen(i, name);
            }
        }
    }

    public Generate GetMap(string name, int level)
    {
        if (levels.ContainsKey(name) && levels[name].Count > level)
        {
            return levels[name][level];
        }
        else
        {
            return null;
        }
    }

    public bool LevelExists(string name)
    {
        if (levels.ContainsKey(name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int LevelCount(string name)
    {
        if (levels.ContainsKey(name))
        {
            return levels[name].Count;
        }
        else
        {
            return 0;
        }
    }
}