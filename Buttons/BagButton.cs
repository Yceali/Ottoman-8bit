using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{

    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    [SerializeField]
    private int bagIndex;

    public Bag MyBag
    {
        get
        {
            return bag;
        }
        set
        {
            if(value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }

    public int MyBagIndex { get => bagIndex; set => bagIndex = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.m_instance.MyfromSlot != null && HandScript.m_instance.MyMovable != null && HandScript.m_instance.MyMovable is Bag)
            {
                if (MyBag != null)
                {
                    InventoryScript.m_instance.SwapBags(MyBag, HandScript.m_instance.MyMovable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)HandScript.m_instance.MyMovable;
                    tmp.MyBagButton = this;
                    tmp.Use();
                    MyBag = tmp;
                    HandScript.m_instance.Drop();
                    InventoryScript.m_instance.MyfromSlot = null;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                HandScript.m_instance.TakeMovable(MyBag);
            }
            else if (bag != null)
            {
                bag.MyBagScript.OpenClose();
            }
        }
        
    }

    public void RemoveBag()
    {
        InventoryScript.m_instance.RemoveBag(MyBag);
        MyBag.MyBagButton = null;

        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            InventoryScript.m_instance.AddItem(item);
        }

        MyBag = null;
    }
}
