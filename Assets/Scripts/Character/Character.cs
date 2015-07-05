using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Separated from Entity's bcuz stuff
public class Character {

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
        this.player = (GameObject)GameObject.Instantiate(Resources.Load("Player"));
        this.player.AddComponent<CharacterBehaviour>();
        //Create the camera object
        this.camera = (GameObject)GameObject.Instantiate(Resources.Load("Camera"));
        Cam cam = this.camera.AddComponent<Cam>();
        cam.target = this.player;
    }
}
