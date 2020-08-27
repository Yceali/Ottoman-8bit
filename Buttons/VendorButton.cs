using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text price;

    [SerializeField]
    private Text quantity;

    private VendorItem vendorItem;
    public void AddItem(VendorItem vendorItem)
    {
        this.vendorItem = vendorItem;

        if (vendorItem.MyQuantity > 0 || (vendorItem.MyQuantity == 0 && vendorItem.Unlimited))
        {
            icon.sprite = vendorItem.Item.MyIcon;
            title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[vendorItem.Item.MyQuality], vendorItem.Item.MyTitle);
            

            if (!vendorItem.Unlimited)
            {
                quantity.text = vendorItem.MyQuantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }

            if (vendorItem.Item.MyPrice > 0)
            {
                price.text = vendorItem.Item.MyPrice.ToString() + " Sikke";
            }
            else
            {
                price.text = string.Empty;
            }
            gameObject.SetActive(true);
        }
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Player.m_instance.MyGold >= vendorItem.Item.MyPrice) && InventoryScript.m_instance.AddItem(Instantiate(vendorItem.Item)))
        {
            SellItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.m_instance.ShowTooltip(new Vector2(0, 1), transform.position, vendorItem.Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.m_instance.HideTooltip();
    }

    private void SellItem()
    {
        Player.m_instance.MyGold -= vendorItem.Item.MyPrice;
        Player.m_instance.UpdateGoldText();

        if (!vendorItem.Unlimited)
        {
            vendorItem.MyQuantity--;
            quantity.text = vendorItem.MyQuantity.ToString();

            if (vendorItem.MyQuantity == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
