using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable , IPointerEnterHandler, IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image cover;

    [SerializeField]
    private Text stackSize;

    public BagScript MyBag { get; set; }

    public int MyIndex { get; set; }
    public bool IsEmpty
    {
        get { return MyItems.Count == 0; }
    }

    public bool IsFull
    {
        get
            {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
        
    }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }
    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }

            return null;
        }
    }

    public Image MyIcon 
    { 
        get
        {
            return icon;
        }
        set 
        {
            icon = value;
        } 
    }

    public int MyCount 
    {
        get
        {
            return MyItems.Count; 
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public ObservableStack<Item> MyItems { get => items;}
    public Image MyCover { get => cover;}

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        MyCover.enabled = false;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return true;
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.m_instance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(InventoryScript.m_instance.MyfromSlot == null && !IsEmpty)
            {
                if (HandScript.m_instance.MyMovable != null)
                {
                    if (HandScript.m_instance.MyMovable is Bag)
                    {
                        if (MyItem is Bag)
                        {
                            InventoryScript.m_instance.SwapBags(HandScript.m_instance.MyMovable as Bag, MyItem as Bag);
                        }
                    }
                    else if (HandScript.m_instance.MyMovable is Armor)
                    {
                        if (MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.m_instance.MyMovable as Armor).MyArmorType)
                        {
                            (MyItem as Armor).Equip();
                            HandScript.m_instance.Drop();
                        }
                    }
                  
                }
                else
                {
                    HandScript.m_instance.TakeMovable(MyItem as IMovable);
                    InventoryScript.m_instance.MyfromSlot = this;
                }
            }
            else if (InventoryScript.m_instance.MyfromSlot == null && IsEmpty)
            {
                if (HandScript.m_instance.MyMovable is Bag)
                {
                    Bag bag = (Bag)HandScript.m_instance.MyMovable;

                    if (bag.MyBagScript != MyBag && (InventoryScript.m_instance.MyEmptySlotCount - bag.MySlotCount) > 0)
                    {
                        AddItem(bag);
                        bag.MyBagButton.RemoveBag();
                        HandScript.m_instance.Drop();
                    }
                }
                else if (HandScript.m_instance.MyMovable is Armor)
                {
                    Armor armor = (Armor)HandScript.m_instance.MyMovable;                    
                    CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
                    AddItem(armor);
                    HandScript.m_instance.Drop();
                }
               
                
            }
            else if (InventoryScript.m_instance.MyfromSlot != null)
            {
                if (PutItemBack() || MargeItems(InventoryScript.m_instance.MyfromSlot) || SwapItems(InventoryScript.m_instance.MyfromSlot) || AddItems(InventoryScript.m_instance.MyfromSlot.MyItems))
                {
                    HandScript.m_instance.Drop();
                    InventoryScript.m_instance.MyfromSlot = null;
                }
            }
            
        }
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.m_instance.MyMovable == null)
        {
            UseItem();
        }
    }

    public void Clear()
    {
        int initialCount = MyItems.Count;
        MyCover.enabled = false;
        if (initialCount > 0)
        {
            for (int i = 0; i < initialCount; i++)
            {
                InventoryScript.m_instance.OnItemCountChanged(MyItems.Pop());
            }           
        }
    }

    public void UseItem()
    {
        if(MyItem is IUsable)
        {
            (MyItem as IUsable).Use();
        }
        else if (MyItem is Armor)
        {
            (MyItem as Armor).Equip();
        }
    }

    private bool MargeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            int free = MyItem.MyStackSize - MyCount;
            for (int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }
            return true;
        }
        return false;
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {

            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    public bool PutItemBack()
    {
        MyCover.enabled = false;
        if (InventoryScript.m_instance.MyfromSlot == this)
        {
            InventoryScript.m_instance.MyfromSlot.enabled = true;
            return true;
        }

        return false;
    }

    private bool SwapItems( SlotScript from)
    {
        from.MyCover.enabled = false;
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            from.MyItems.Clear();
            from.AddItems(MyItems);

            MyItems.Clear();
            AddItems(tmpFrom);

            return true;
        }
        return false;
    }

    public void UpdateSlot()
    {
        UIManager.m_instance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UIManager.m_instance.ShowTooltip(new Vector2(1, 0),transform.position, MyItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.m_instance.HideTooltip();
    }
}
