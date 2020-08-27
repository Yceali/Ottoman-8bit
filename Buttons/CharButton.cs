using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private ArmorType armorType;

    private Armor equipedArmor;

    [SerializeField]
    private Image icon;

    public Armor MyEquipedArmor { get => equipedArmor; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.m_instance.MyMovable is Armor)
            {
                Armor tmp = (Armor)HandScript.m_instance.MyMovable;

                if (tmp.MyArmorType == armorType)
                {
                    EquipArmor(tmp);
                }

                UIManager.m_instance.RefreshTooltip(tmp);
            }
            else if(HandScript.m_instance.MyMovable == null && MyEquipedArmor != null)
            {
                HandScript.m_instance.TakeMovable(MyEquipedArmor);
                CharacterPanel.MyInstance.MySelectedButton = this;
                icon.color = Color.gray;
            }
        }
    }

    public void EquipArmor(Armor armor)
    {
        armor.Remove();

        if (MyEquipedArmor != null)
        {
            if (MyEquipedArmor != armor)
            {
                armor.MySlot.AddItem(MyEquipedArmor);
                UIManager.m_instance.RefreshTooltip(MyEquipedArmor);
            }          
        }
        else
        {
            UIManager.m_instance.HideTooltip();
        }

        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        icon.color = Color.white;
        this.equipedArmor = armor;
        this.MyEquipedArmor.MyCharButton = this;

        if(HandScript.m_instance.MyMovable == (armor as IMovable))
        {
            HandScript.m_instance.Drop();
        }
        
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;

        MyEquipedArmor.MyCharButton = null;
        equipedArmor = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyEquipedArmor != null)
        {
            UIManager.m_instance.ShowTooltip(new Vector2(0, 0),transform.position, MyEquipedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.m_instance.HideTooltip();
    }
}
