using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Tui
{
    public override int CanMoveTo(Vector2 dir)
    {
        return 2;

    }
}
