using UnityEngine;
using System.Collections;

public class Spoder : Entity {
    //Used for declaring of stuf
    public static string Name = "Spoder";

    public override void Instantiate()
    {
        base.Instantiate();
        float hard = GameState.Instance.Map.Hardness;
        this.Damage = Random.Range(hard, hard + 5);
        this.Health = Random.Range(hard + 10, hard + 40);
    }
}
