using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

//Separated from Entity's bcuz stuff
public class Character : CharacterStats
{
    public Dictionary<int, Item> Equipment = new Dictionary<int, Item>();

    public CharacterBehaviour Behaviour
    {
        get
        {
            return Player.GetComponent<CharacterBehaviour>();
        }
    }

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

    public void SetEquipment(int slot, Item item)
    {
        if (!Equipment.ContainsKey(slot))
        {
            Equipment.Add(slot, item);
        }
        else
        {
            Equipment[slot].UnEquip();
            Equipment[slot] = item;
            
            item.Equip();
        }
    }


    private Class charClass;
    private Race charRace;

    private GameObject camera;
    private GameObject player;
    private GameObject miniMap;

    public Character(Class cClass, Race cRace)
    {
        GameState.Instance.Character = this;
        //Assign character race + class
        charClass = cClass;
        charRace = cRace;

        //Create the player object
        this.Player = (GameObject)GameObject.Instantiate(Resources.Load("Player"));
        //Create the camera object
        this.camera = (GameObject)GameObject.Instantiate(Resources.Load("Camera"));
        //Create minimap camera object
        this.miniMap = (GameObject)GameObject.Instantiate(Resources.Load("MiniMapCam"));
        
        Cam cam = this.camera.AddComponent<Cam>();
        cam.target = this.Player;

        Cam camMini = this.miniMap.AddComponent<Cam>();
        camMini.target = this.Player;
        
        Health = MaxHealth;
        
        UIMain.SetCharLevel();
        UIMain.SetExp();
        UIMain.SetHealth();
        UIMain.SetLevel();
        UIMain.SetCharLevel();

        this.CharClass.Init();
    }

    public void Hit(float amount, string name)
    {
#if DEBUG
        //Debug.Log("Hit player for " + amount + " DMG HP LEFT:" + this.health);
#endif
        this.Health -= amount - this.Armour;
        UIMain.SetChat("You've been hit for: " + amount + " by: " + name);
    }

    public void CharUpdate()
    {
        this.Health += HealthRegen;
        this.Mana += ManaRegen;
    }
}