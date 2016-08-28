using System;
using System.Collections.Generic;
using UnityEngine;

//TODO:: Use this for generation
public class BaseNpc : Entity
{
    public GameObject textBalloon = GameObject.Find("ChatPopup");

    public override void Action()
    {
        return;
    }

    public override void Hit(float dmg)
    {
        return;
    }

    public override void Interact()
    {
        return;
    }
}