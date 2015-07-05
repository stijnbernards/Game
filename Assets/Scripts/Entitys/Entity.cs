using UnityEngine;
using System.Collections;

public class Entity {

    #region Stats
    public float Movespeed;
    public float Attackspeed;
    public float Defence;
    public float Damage;
    #endregion

    public GameObject EntitySprite;

    public Entity(float movespeed, float attackspeed, float defence, float damage)
    {
        this.Movespeed = movespeed;
        this.Attackspeed = attackspeed;
        this.Defence = defence;
        this.Damage = damage;
    }

    //Should be implemented to set the entity sprite
    public virtual void SetSprite() { }
}
