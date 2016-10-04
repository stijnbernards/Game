using UnityEngine;
using System.Collections.Generic;

public class Entities
{
    public const string ENTITY_SPIDER = "SPIDER";

    public static void RegisterEntities()
    {
        //Maak Static function
        GameState.Instance.EntityRegistry.RegisterEntity(ENTITY_SPIDER, Resources.Load("Spoder") as GameObject);
    }
}