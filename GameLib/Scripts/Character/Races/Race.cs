using UnityEngine;
using System.Collections;

public abstract class Race {

    public string RaceName
    {
        get
        {
            return this.raceName;
        }
    }

    private string raceName;

    protected Race(string race)
    {
        this.raceName = race;
    }
}
