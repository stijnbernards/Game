using UnityEngine;
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
    public float HealthRegen = 0.8f;
    public float MaxHealth = 10f;
    public float Damage = 15f;
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
            {
                CharacterBehaviour Char = (CharacterBehaviour)(this.player.GetComponent(typeof(CharacterBehaviour)));
                Char.Kill();
            }
            if (this.health > this.MaxHealth)
                this.Health = MaxHealth;
        }
    }
    #endregion

    private float health = 100;

    private Class charClass;
    private Race charRace;

    private GameObject player;
    private GameObject camera;

    public Character(Class cClass, Race cRace)
    {
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
        Debug.Log("Hit player for " + amount + " DMG HP LEFT:" + this.health);
#endif
        this.Health -= amount;
    }

    public void CharUpdate()
    {
        this.Health += HealthRegen;
#if DEBUG
        Debug.Log("Character hp: " + this.Health);
#endif
    }
}
