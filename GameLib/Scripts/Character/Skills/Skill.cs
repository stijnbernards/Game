using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Dictionary<string, float> Modifiers = new Dictionary<string, float>();
    public Category SkillCategory
    {
        get
        {
            return GameState.Instance.CategoryRegistry.GetCategory(skillCategory);
        }
    }
    public bool Unlocked = false;
    public string Name;
    public float Cost = 1.0F;

    private string skillCategory;

    public Skill()
    {

    }

    public void GenerateSkill(string cat)
    {
        Category category = GameState.Instance.CategoryRegistry.GetCategory(cat);

        Name = "TEST";
        skillCategory = cat;

        float stats = Random.Range(1F, 3F);

        for (float i = 0; i < stats; i++)
        {
            string stat = category[Random.Range(0F, category.AmountOfStats - 1)];
            if(Modifiers.ContainsKey(stat))
            {
                continue;
            }
            float modifier = Random.Range(1F, 3F);

            Modifiers.Add(stat, modifier);
        }
    }
}