using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour{

    #region Stats 
    public float Movespeed;
    public float Attackspeed;
    public float Defence;
    public float Damage;
    public float Exp;
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

    private float health = 100;

    public virtual void Instantiate()
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
        //Debug.Log("Hit "+ this.name +" for "+ dmg +" DMG HP LEFT:"+ this.health);
#endif
    }

    public void Kill()
    {
        GameState.Instance.Character.Exp += Exp;
        Destroy(gameObject);
    }

    /// <summary>
    /// Basic AI function really beta ignores walls n stuff
    /// </summary>
    public virtual void Action()
    {
        Vector3 pPos = GameState.Instance.Character.Player.transform.position;
        Vector3 rad = pPos - transform.position;
        Vector3 dir = new Vector3(0f, 0f, 0f);;

        if ((rad.x < LOS && rad.x > -LOS) && (rad.y <= LOS && rad.y > -LOS))
        {
            if (rad.x > 0)
                dir += Vector3.right;
            else if (rad.x < 0)
                dir += Vector3.left;

            if (rad.y > 0)
                dir += Vector3.up;
            else if (rad.y < 0)
                dir += Vector3.down;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f);
            if (hit.collider != null)
                if (hit.collider.gameObject.GetComponent(typeof(CharacterBehaviour)) != null)
                    GameState.Instance.Character.Hit(Damage);
                else
                    FindSpot(dir);
            else
                transform.position += dir;
        }
    }

    private void FindSpot(Vector3 dir)
    {

    }
}

public struct EntityItem
{
    public string Ent { get; set; }
    public int Amount { get; set; }

    public EntityItem(string ent, int amount)
    {
        this.Ent = ent;
        this.Amount = amount;
    }
}