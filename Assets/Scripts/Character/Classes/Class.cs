using UnityEngine;
using System.Collections;

public class Class {
    public float SkillPoints;
    public string ClassName
    {
        get
        {
            return this.className;
        }
    }

    private string className;

    protected Class(string nClass)
    {
        this.className = nClass;
    }

    //Basic class stuff, setting character stats etc...
    public void Init()
    {
        GameState.Instance.Character.SkillPoints = SkillPoints;
    }
}
