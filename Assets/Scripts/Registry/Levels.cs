using UnityEngine;
using System;
using System.Collections.Generic;

public class Levels
{
    public const string LEVEL_TEST1 = "TEST1";

    public static void RegisterLevels()
    {
        Caves test1 = new Caves();
        test1.Difficulty = 10;
        test1.height = 50;
        test1.width = 50;

        test1.EntityList.Add(new EntityItem(Entities.ENTITY_SPIDER, 10));

        GameState.Instance.LevelRegistry.AddLevel(LEVEL_TEST1, test1);
    }
}