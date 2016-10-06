using System;

public struct EntityItem
{
    public string Ent;
    public int Amount;

    public EntityItem(string ent, int amount)
    {
        Ent = ent;
        Amount = amount;
    }
}