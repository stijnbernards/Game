using UnityEngine;

class Spikes : Entity
{
    public Spikes() 
        : base()
    {
        this.Physical = false;
    }

    public override void Action() { }

    public override void Step()
    {
        GameState.Instance.Character.Hit(10F, "Spikes");
    }
}

