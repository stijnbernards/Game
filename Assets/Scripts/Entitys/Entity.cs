using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity {

    #region Stats
    public float Movespeed;
    public float Attackspeed;
    public float Defence;
    public float Damage;
    #endregion

    public GameObject entity;

    public virtual Entity SpawnInWorld(Vector2 pos, GameObject sprite)
    {
        this.entity = GameObject.Instantiate(sprite, pos, Quaternion.identity) as GameObject;
        return this;
    }

    public void MoveTo(float x, float y)
    {
        entity.transform.position = new Vector2(x, y);
    }
}

public struct EntityRarity
{
    public Entity Ent;
    //Not yet implemented
    public int Rarity;
}