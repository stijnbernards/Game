using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Separated from Entity's bcuz stuff
public class Character
{

    #region Properties
    public GameObject Player
    {
        get
        {
            return this.player;
        }
        set
        {
            this.player = value;
        }
    }

    public Race CharRace
    {
        get
        {
            return this.charRace;
        }
    }

    public Class CharClass
    {
        get
        {
            return this.charClass;
        }
    }
    public float MoveSpeed = 1f;
    public float AttackSpeed = 1f;
    public float HealthRegen = 100f;
    public float MaxHealth = 1000f;
    public float Damage = 200f;
    public float Health
    {
        get
        {
            return this.health;
        }
        set
        {
            this.health = value;
            UIMain.SetHealth();
            if (this.health <= 0)
            {
                CharacterBehaviour Char = (CharacterBehaviour)(this.player.GetComponent(typeof(CharacterBehaviour)));
                Char.Kill();
            }
            if (this.health > this.MaxHealth)
                this.Health = MaxHealth;
        }
    }
    #endregion

    private float health;

    private Class charClass;
    private Race charRace;

    private GameObject player;
    private GameObject camera;

    public Character(Class cClass, Race cRace)
    {
        GameState.Instance.Character = this;
        this.Health = 1000f;
        //Assign character race + class
        this.charClass = cClass;
        this.charRace = cRace;

        //Create the player object
        this.Player = (GameObject)GameObject.Instantiate(Resources.Load("Player"));
        //Create the camera object
        this.camera = (GameObject)GameObject.Instantiate(Resources.Load("Camera"));
        Cam cam = this.camera.AddComponent<Cam>();
        cam.target = this.player;
    }

    public void Hit(float amount)
    {
#if DEBUG
        //Debug.Log("Hit player for " + amount + " DMG HP LEFT:" + this.health);
#endif
        this.Health -= amount;
    }

    public void CharUpdate()
    {
        this.Health += HealthRegen;
    }
}
