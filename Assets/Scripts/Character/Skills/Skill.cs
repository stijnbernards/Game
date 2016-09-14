using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public Dictionary<string, float> Stats = new Dictionary<string, float>();
    public Category.SkillCategory SkillCategory;

    public Skill(Category.SkillCategory cat)
    {
        SkillCategory = cat;
    }

}