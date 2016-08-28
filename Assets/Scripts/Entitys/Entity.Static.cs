using System;
using UnityEngine;

public partial class Entity
{
    private static Sprite defaultSprite
    {
        get { return Resources.Load<Sprite>("Sprites/Stone"); }
    }

    public static GameObject SpawnInWorld(Vector2 pos, GameObject sprite)
    {
        GameObject go = GameObject.Instantiate(sprite, pos, Quaternion.identity) as GameObject;
        Entity entity = (Entity)go.GetComponent(typeof(Entity));
        entity.Instantiate();

        return go;
    }

    public static GameObject Default
    {
        get
        {
            GameObject go = new GameObject();
            Destroy(go);
            go.name = "DefaultEntity";
            go.AddComponent<BoxCollider2D>();
            go.AddComponent<SpriteRenderer>().sprite = defaultSprite;
            
            return go;
        }
    }
}

