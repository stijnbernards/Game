using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRegistry : IEnumerable<KeyValuePair<string, List<Generate>>>
{
    public LevelRegistry GetLevelRegistry()
    {
        if (GameState.Instance != null && GameState.Instance.LevelRegistry != null)
        {
            return GameState.Instance.LevelRegistry;
        }
        else
        {
            return null;
        }
    }

    public List<Generate> this[string i]
    {
        get
        {
            return levels[i];
        }
    }

    private Dictionary<string, List<Generate>> levels = new Dictionary<string, List<Generate>>();

    public void AddLevels<T>(string name, int amount, float difficulty) where T : Generate, new()
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
                levels[name][i].StartGen(difficulty, name);
            }
        }
    }

    public void AddLevel(string name, Generate level)
    {
        if (levels.ContainsKey(name))
        {
            levels[name].Add(level);
        }
        else
        {
            levels.Add(name, new List<Generate>() { level });
        }
    }

    public Generate GetMap(string name, int level)
    {
        if (levels.ContainsKey(name) && levels[name].Count > level)
        {
            if (levels[name][level].map == null)
            {
                levels[name][level].StartGen(0, name);
            }

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

    public IEnumerator<KeyValuePair<string, List<Generate>>> GetEnumerator()
    {
        return levels.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return levels.GetEnumerator();
    }
}