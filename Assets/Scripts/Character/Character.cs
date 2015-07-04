using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character{

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
        Camera cam = this.camera.AddComponent<Camera>();
        cam.target = this.player;
    }
}
