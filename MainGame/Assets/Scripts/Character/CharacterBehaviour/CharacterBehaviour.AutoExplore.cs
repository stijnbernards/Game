using System;
using UnityEngine;

public partial class CharacterBehaviour
{
    public void AutoExplore()
    {
        while (true)
        {
            Vector2 transform = GameState.Instance.Character.Behaviour.transform.position;
        }
    }
}