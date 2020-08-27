﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour , IInteractible
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite openSprite, closeSprite;

    private bool isOpen;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private List<Item> items;

    [SerializeField]
    private BagScript bag;

    public List<Item> MyItems { get => items; set => items = value; }
    public BagScript MyBag { get => bag; set => bag = value; }

    private void Awake()
    {
        items = new List<Item>();
    }

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            AddItems();
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        if (isOpen)
        {
            StoreItems();
            MyBag.Clear();
            isOpen = false;
            spriteRenderer.sprite = closeSprite;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void AddItems()
    {
        if (MyItems != null)
        {
            foreach (Item item in MyItems)
            {
                item.MySlot.AddItem(item);
            }
        }
    }

    public void StoreItems()
    {
        MyItems = MyBag.GetItems();
    }
}
