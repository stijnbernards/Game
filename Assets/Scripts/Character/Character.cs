using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Separated from Entity's bcuz stuff
public class Character : CharacterStats
{
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


    private Class charClass;
    private Race charRace;

    private GameObject camera;
    private GameObject player;

    private float health;

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
        Cam cam = this.camera.AddComponent<Cam>();
        cam.target = this.Player;
        this.health = MaxHealth;
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
        this.Health -= amount;
        UIMain.SetChat("You've been hit for: " + amount + " by: " + name);
    }

    public void CharUpdate()
    {
        this.Health += HealthRegen;
    }
}

//Character stats are defined here
public class CharacterStats
{
    #region Properties
    //Levels
    public float SkillPoints
    {
        get { return this.skillPoints; }
        set 
        {
            this.skillPoints = value;
        }
    }

    public float Strength
    {
        get
        {
            return this.strength;
        }
        set
        {
            this.strength = value;
            SkillPoints--;
        }
    }
    public float Dexterity
    {
        get
        {
            return this.dexterity;
        }
        set
        {
            this.dexterity = value;
            SkillPoints--;
        }
    }
    public float Constitution
    {
        get
        {
            return this.constitution;
        }
        set
        {
            this.constitution = value;
            SkillPoints--;
        }
    }
    public float Magic
    {
        get
        {
            return this.magic;
        }
        set
        {
            this.magic = value;
            SkillPoints--;
        }
    }

    public float ExpToLevel
    {
        get
        {
            return this.expToLevel;
        }
    }
    public float Level
    {
        get
        {
            return this.level;
        }
        set
        {
            this.level = value;
            LevelUp();
        }
    }
    public float Exp
    {
        get
        {
            return this.exp / (this.expToLevel / 100);
        }
        set
        {
            this.exp = value;
            UIMain.SetExp();
            if (this.exp >= this.expToLevel)
                this.Level++;
        }
    }

    //Stats
    public float MoveSpeed
    {
        get
        {
            return (this.Dexterity / 100f) + 1f;
        }
    }

    public float AttackSpeed
    {
        get
        {
            return (this.Dexterity / 100f) + 1f;
        }
    }
    public float HealthRegen
    {
        get
        {
            return this.Constitution * 0.1f + 0.1f;
        }
    }
    public float MaxHealth
    {
        get
        {
            return this.Constitution * 3.5f + 100;
        }
    }
    public float Damage
    {
        get
        {
            return this.Strength + 10;
        }
    }

    public float LOS
    {
        get
        {
            return this.los;
        }
    }
    #endregion

    private float los = 5;
    private float exp;
    private float level = 1;
    private float expToLevel
    {
        get { return CalcExpLevel(); }
    }
    private float skillPoints, strength, dexterity, constitution, magic;

    public float CalcExpLevel()
    {
        return (this.level * level - 1) + 100f;
    }

    public void LevelUp()
    {
        this.Exp = 0;
        this.skillPoints += 3;
        UIMain.SetCharLevel();
        UIMain.SetExp();
    }

#if DEBUG
    public void DebugLog()
    {
        Debug.Log("/----------------------------------/");
        Debug.Log("ExpToLevel: " + ExpToLevel);
        Debug.Log("Level: " + Level);
        Debug.Log("Exp: " + Exp);
        Debug.Log("MoveSpeed: " + MoveSpeed);
        Debug.Log("Health Regen: " + HealthRegen);
        Debug.Log("Max health: " + MaxHealth);
        Debug.Log("Damage: " + Damage);
        Debug.Log("Attack speed: " + AttackSpeed);
        Debug.Log("Dexerity: " + Dexterity);
        Debug.Log("Constitution: " + Constitution);
        Debug.Log("Magic: " + Magic);
        Debug.Log("Strength: " + Strength);
        Debug.Log("/----------------------------------/");
    }
#endif
}