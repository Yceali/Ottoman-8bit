using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyBottle", menuName = "Items/EmptyBottle", order = 4)]
public class EmptyBottle : Item
{
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nİlaç üretiminde kullanılır");
    }
}
