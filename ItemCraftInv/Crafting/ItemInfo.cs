﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text stack;

    private int count;

    public Item MyItem { get => item; set => item = value; }

    public void Initialize(Item item, int count)
    {
        MyItem = item;
        image.sprite = item.MyIcon;
        title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[item.MyQuality], item.MyTitle);
        this.count = count;

        if (count > 1)
        {
            stack.enabled = true;
            stack.text = InventoryScript.m_instance.GetItemCount(item.MyTitle).ToString() +"/" + count.ToString();
        }
    }

    public void UpdateStackCount()
    {
        stack.text = InventoryScript.m_instance.GetItemCount(MyItem.MyTitle) + "/" + count.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.m_instance.ShowTooltip(new Vector2(0, 0), transform.position, MyItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.m_instance.HideTooltip();
    }
}
