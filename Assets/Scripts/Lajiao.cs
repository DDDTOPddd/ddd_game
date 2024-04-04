using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lajiao : Banana
{
   public override int BeEat()
    {
        Destroy(gameObject);
        return 2;
    }
}
