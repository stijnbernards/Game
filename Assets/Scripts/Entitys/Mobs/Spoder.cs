using UnityEngine;
using System.Collections;

public class Spoder : Entity {
    //Used for declaring of stuf
    public static string Name = "Spoder";

    public override void Instantiate()
    {
        base.Instantiate();

        this.Damage = Random.Range(1, 3);
        this.Health = Random.Range(10, 50);
    }
}
