using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HealthPotion",menuName ="Items/Potion",order = 1)]
public class HealthPotion : Item, IUsable
{

    [SerializeField]
    private int health;

    public void Use()
    {
        if (Player.m_instance.MyHealth.MyCurrentValue < Player.m_instance.MyHealth.MyMaxValue)
        {
            Remove();
            Player.m_instance.GetHealth(health);
        }  
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nKullan: {0} Can yeniler",health);
    }
}
