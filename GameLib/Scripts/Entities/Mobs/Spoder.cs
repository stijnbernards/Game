using UnityEngine;
using System.Collections;

public class Spoder : Entity {

    public override void Instantiate()
    {
        base.Instantiate();
        float hard = GameState.Instance.Map.Difficulty;
        this.Damage = Random.Range(hard, hard + 5);
        this.Health = Random.Range(hard + 10, hard + 40);
        this.Exp = Random.Range(hard + 20, hard + 40);
    }
}
