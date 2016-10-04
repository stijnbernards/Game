using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CategoryRegistry : IEnumerable<KeyValuePair<string, Category>>
{
    public static CategoryRegistry GetCategoryRegistry()
    {
        if (GameState.Instance != null || GameState.Instance.CategoryRegistry != null)
        {
            return GameState.Instance.CategoryRegistry;
        }
        else
        {
            return null;
        }
    }
    
    private Dictionary<string, Category> categories = new Dictionary<string, Category>();

    public Category this[string i]
    {
        get
        {
            return categories[i];
        }
    }

    public void RegisterCategory(string categoryIdentifier, Category cat)
    {
        if (categories.ContainsKey(categoryIdentifier))
        {
            //throw exeception
        }
        else
        {
            categories.Add(categoryIdentifier, cat);
        }
    }

    public Category GetCategory(string categoryIdentifier)
    {
        if (categories.ContainsKey(categoryIdentifier))
        {
            return categories[categoryIdentifier];
        }
        else
        {
            return null;
        }
    }

    public IEnumerator<KeyValuePair<string, Category>> GetEnumerator()
    {
        return categories.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return categories.GetEnumerator();
    }
}