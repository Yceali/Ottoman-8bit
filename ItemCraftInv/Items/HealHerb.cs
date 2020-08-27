using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HealHerb", menuName ="Items/HealHerb", order =3)]
public class HealHerb : Item
{
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nŞifalı ilaç üretiminde kullanılır");
    }
}
