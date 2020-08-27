using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ArmorType { Head, Shoulders, Chest, Cloak, Legs, Belt, Hands, Bracers, Ring, Neck, Feets, MainHand, Throwables, OffHand}

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{

    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private int agility;
    [SerializeField]
    private int strenght;
    [SerializeField]
    private int stamina;

    internal ArmorType MyArmorType { get => armorType; }

   

    public override string GetDescription()
    {
        string stats = string.Empty;

        if(strenght > 0)
        {
            stats += string.Format("\n +{0} güç", strenght);
        }
        else if(strenght < 0)
        {
            stats += string.Format("\n {0} güç", strenght);
        }
        if (agility > 0)
        {
            stats += string.Format("\n +{0} çeviklik", agility);
        }
        else if (agility < 0)
        {
            stats += string.Format("\n {0} çeviklik", agility);
        }
        if (stamina > 0)
        {
            stats += string.Format("\n +{0} dayanıklılık", stamina);
        }
        else if (stamina < 0)
        {
            stats += string.Format("\n {0} dayanıklılık", stamina);
        }
        return base.GetDescription() + stats;
    }

    public void Equip()
    {
        CharacterPanel.MyInstance.EquipArmor(this);
    }
}
