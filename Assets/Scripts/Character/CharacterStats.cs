using UnityEngine;
using System;
using System.Collections.Generic;

//Character stats are defined here
public class CharacterStats
{
    public const string STATS_MAGIC = "MAGIC";
    public const string STATS_STRENGTH = "STRENGTH";
    public const string STATS_ARMOUR = "ARMOUR";
    public const string STATS_CONSTITUTION = "CONSTITUTION";
    public const string STATS_DEXTERITY = "DEXTERITY";
    public const string STATS_DAMAGE = "DAMAGE";
    public const string STATS_HEALTHREGEN = "HEALTHREGEN";
    public const string STATS_MANA = "MANA";
    public const string STATS_MAXMANA = "MAXMANA";
    public const string STATS_MANAREGEN = "MANAREGEN";
    public const string STATS_HEALTH = "HEALTH";
    public const string STATS_MAXHEALTH = "MAXHEALTH";
    public const string STATS_ATTACKSPEED = "ATTACKSPEED";
    public const string STATS_MOVEMENTSPEED = "MOVESPEED";
    public const string STATS_LOS = "LOS";

    #region Properties
    //Levels
    public float SkillPoints
    {
        get 
        { 
            return this.skillPoints; 
        }
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
            return ((this.Dexterity / 100f) + 1f) * moveSpeed;
        }
        set
        {
            this.moveSpeed = value;
        }
    }

    public float AttackSpeed
    {
        get
        {
            return ((this.Dexterity / 100f) + 1f) * attackSpeed;
        }
        set
        {
            this.attackSpeed = value;
        }
    }

    public float HealthRegen
    {
        get
        {
            return this.Constitution * 0.1f + 0.1f + healthRegen;
        }
        set
        {
            this.healthRegen = value;
        }
    }

    public float MaxHealth
    {
        get
        {
            return this.Constitution * 3.5f + 100 + maxHealth;
        }
        set
        {
            this.maxHealth = value;
        }
    }

    public float Damage
    {
        get
        {
            return this.Strength + 10 + damage;
        }
        set
        {
            this.damage = value;
        }
    }

    public float LOS
    {
        get
        {
            return this.los;
        }
        set
        {
            this.los = value;
        }
    }

    public float Armour
    {
        get
        {
            return this.armour;
        }
        set
        {
            this.armour = value;
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
                CharacterBehaviour Char = GameState.Instance.Character.Behaviour;
                Char.Kill();
            }
            if (this.health > this.MaxHealth)
            {
                this.Health = MaxHealth;
            }
        }
    }

    public float Mana
    {
        get
        {
            return mana;
        }
        set
        {
            mana = value;

            if (mana > maxMana)
            {
                mana = maxMana;
            }
        }
    }

    public float MaxMana
    {
        get
        {
            return maxMana;
        }
        set
        {
            maxMana = value;
        }
    }

    public float ManaRegen
    {
        get
        {
            return manaRegen;
        }
        set
        {
            manaRegen = value;
        }
    }

    #endregion

    private float health;
    private float los = 10;
    private float exp;
    private float level = 1;
    private float armour = 0;
    private float mana = 0;
    private float expToLevel
    {
        get { return CalcExpLevel(); }
    }
    private float skillPoints, strength, dexterity, constitution, magic, healthRegen, maxHealth, damage, attackSpeed, moveSpeed, maxMana, manaRegen;

    #region StatDictionary
    private Dictionary<string, VariableReference> statsDictionary = new Dictionary<string, VariableReference>()
    {
        {
            STATS_DEXTERITY, 
            new VariableReference(
                () => GameState.Instance.Character.Dexterity,
                v => { GameState.Instance.Character.Dexterity = (float) v; }
            )
        },
        {
            STATS_STRENGTH,
            new VariableReference(
                () => GameState.Instance.Character.Strength,
                v => {GameState.Instance.Character.Strength = (float) v; }
            )
        },
        {
            STATS_CONSTITUTION,
            new VariableReference(
                () => GameState.Instance.Character.Constitution,
                v => {GameState.Instance.Character.Constitution = (float) v; }
            )
        },
        {
            STATS_MAGIC,
            new VariableReference(
                () => GameState.Instance.Character.Magic,
                v => {GameState.Instance.Character.Magic = (float) v; }
            )
        },
        {
            STATS_ARMOUR,
            new VariableReference(
                () => GameState.Instance.Character.Armour,
                v => {GameState.Instance.Character.Armour = (float) v; }
            )
        },
        {
            STATS_HEALTHREGEN,
            new VariableReference(
                () => GameState.Instance.Character.HealthRegen,
                v => {GameState.Instance.Character.HealthRegen = (float) v; }
            )
        },
        {
            STATS_MAXHEALTH,
            new VariableReference(
                () => GameState.Instance.Character.MaxHealth,
                v => {GameState.Instance.Character.MaxHealth = (float) v; }
            )
        },
        {
            STATS_DAMAGE,
            new VariableReference(
                () => GameState.Instance.Character.Damage,
                v => {GameState.Instance.Character.Damage = (float) v; }
            )
        },
        {
            STATS_ATTACKSPEED,
            new VariableReference(
                () => GameState.Instance.Character.AttackSpeed,
                v => {GameState.Instance.Character.AttackSpeed = (float) v; }
            )
        },
        {
            STATS_MOVEMENTSPEED,
            new VariableReference(
                () => GameState.Instance.Character.MoveSpeed,
                v => {GameState.Instance.Character.MoveSpeed = (float) v; }
            )
        },
        {
            STATS_LOS,
            new VariableReference(
                () => GameState.Instance.Character.LOS,
                v => {GameState.Instance.Character.LOS = (float) v; }
            )
        },
        {
            STATS_MAXMANA,
            new VariableReference(
                () => GameState.Instance.Character.MaxMana,
                v => {GameState.Instance.Character.MaxMana = (float) v; }
            )
        },        
        {
            STATS_MANAREGEN,
            new VariableReference(
                () => GameState.Instance.Character.ManaRegen,
                v => {GameState.Instance.Character.ManaRegen = (float) v; }
            )
        },
        {
            STATS_MANA,
            new VariableReference(
                () => GameState.Instance.Character.Mana,
                v => {GameState.Instance.Character.Mana = (float) v; }
            )
        },
        {
            STATS_HEALTH,
            new VariableReference(
                () => GameState.Instance.Character.Health,
                v => {GameState.Instance.Character.Health = (float) v; }
            )
        }
    };

    #endregion StatDictionary

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

    public float GetStatByString(string statName)
    {
        if (statsDictionary.ContainsKey(statName))
        {
            return (float)statsDictionary[statName].Get();
        }
        else
        {
            return -1F;
        }
    }

    public bool SetStatByString(string statName, float value)
    {
        if (statsDictionary.ContainsKey(statName))
        {
            statsDictionary[statName].Set(value);
            return true;
        }
        else
        {
            return false;
        }
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

public sealed class VariableReference
{
    public Func<object> Get { get; private set; }
    public Action<object> Set { get; private set; }

    public VariableReference(Func<object> getter, Action<object> setter)
    {
        Get = getter;
        Set = setter;
    }
}