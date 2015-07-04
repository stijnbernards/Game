using UnityEngine;
using System.Collections;

public abstract class Class {
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
}
