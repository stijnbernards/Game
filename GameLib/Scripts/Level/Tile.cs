using UnityEngine;
using System.Collections;

public class Tile{

    public delegate void TileAction();

    public int TileNumber = 1;
    public bool Seen = false;
    public string Name = "normal";

    public TileAction Action;

    public virtual void Execute()
    {
        if (this.Action != null)
        {
            Action.Invoke();
        }
    }
}
