using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour{

    #region Stats 
    public string Name;

    public float Movespeed;
    public float Attackspeed;
    public float Defence;
    public float Damage;
    public float LOS;
    public float Health
    {
        get
        {
            return this.health;
        }
        set
        {
            this.health = value;
            if (this.health <= 0)
                Kill();
        }
    }
    #endregion

    private float health;

    public void Instantiate()
    {
        this.LOS = 10;
    }

    public static GameObject SpawnInWorld(Vector2 pos, GameObject sprite)
    {
        GameObject go = GameObject.Instantiate(sprite, pos, Quaternion.identity) as GameObject;
        Entity entity = (Entity)go.GetComponent(typeof(Entity));
        entity.Instantiate();

        return go;
    }

    public void MoveTo(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

    public void Hit(float dmg)
    {
        this.Health -= dmg;
#if DEBUG
        Debug.Log("Hit "+ this.name +" for "+ dmg +" DMG HP LEFT:"+ this.health);
#endif
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Basic AI function really beta ignores walls n stuff
    /// </summary>
    public virtual void Action()
    {
        Vector3 pPos = GameState.Instance.Character.Player.transform.position;
        Vector3 rad = pPos - transform.position;

#if DEBUG
        Debug.Log(rad.x);
        Debug.Log(-LOS);
        Debug.Log(LOS);
#endif

        if((rad.x < LOS && rad.x > -LOS) && (rad.y <= LOS && rad.y > -LOS)){
            if (rad.x > 0)
                transform.position += Vector3.right;
            else if (rad.x < 0)
                transform.position += Vector3.left;

            if (rad.y > 0)
                transform.position += Vector3.up;
            else if (rad.y < 0)
                transform.position += Vector3.down;
        }
    }
}

public struct EntityRarity
{
    public Entity Ent;
    //Not yet implemented
    public int Rarity;
}