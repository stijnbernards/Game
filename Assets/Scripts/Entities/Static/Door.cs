using System;
using System.Collections.Generic;
using UnityEngine;

class Door : Entity
{
    public override void Action() { }

    public override void Interact()
    {
        if (Physical == false)
        {
            Physical = true;
        }
        else
        {
            Physical = false;
        }
    }

}