using System;
using UnityEngine;

using Anvil.Attributes;

[Mod(TestMod.MOD_ID)]
public class TestMod
{
    public const string MOD_ID = "TEST_MOD";

    public void PreInit()
    {
        Debug.Log(MOD_ID + " PreInit Called");
    }

    public void Load()
    {
        Debug.Log(MOD_ID + " Load Called");
    }

    public void PostInit()
    {
        Debug.Log(MOD_ID + " PostInit Called");
    }
}