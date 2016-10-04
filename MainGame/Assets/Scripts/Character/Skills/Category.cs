using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Category 
{
    public const string CATEGORY_PHYSICAL = "physical";
    public const string CATEGORY_MAGICAL = "magical";
    public const string CATEGORY_STEALTH = "stealth";
    public const string CATEGORY_SHADOWS = "shadows";
    public const string CATEGORY_DEFENSIVE = "defensive";

    public string Name;
    public float AmountOfStats
    {
        get
        {
            return associatedStats.Count;
        }
    }

    private List<string> associatedStats = new List<string>();

    public string this[string i]
    {
        get
        {
            return associatedStats.Find(x => x == i);
        }
    }

    public string this[float i]
    {
        get
        {
            return associatedStats[(int)i];
        }
    }

    public Category(string name)
    {
        this.Name = name;
    }

    public void AddStat(string stat)
    {
        if (associatedStats.Contains(stat))
        {
            return;
        }
        else
        {
            associatedStats.Add(stat);
        }
    }

    public string GetStat(string stat)
    {
        if (associatedStats.Contains(stat))
        {
            return stat;
        }
        else
        {
            return null;
        }
    }


}
