using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButtons : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    
    public IUsable MyUsable { get; set; }

    [SerializeField]
    private Text stackSize;

    private Stack<IUsable> usables = new Stack<IUsable>();

    private int count;
    public Button MyButton {get; private set;}
    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount
    {
        get
        {
            return count;
        }
    }

    public Text MyStackText
    {
        get 
        { 
            return stackSize;
        }
    }

    public Stack<IUsable> MyUsables 
    {
        get
        {
            return usables;
        }
        set
        {
            if (value.Count > 0)
            {
                MyUsable = value.Peek();
            }
            else
            {
                MyUsable = null;
            }
            usables = value;
        }
    }

    [SerializeField]
    private Image icon;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.m_instance.MyMovable != null && HandScript.m_instance.MyMovable is IUsable)
            {
                SetUsable(HandScript.m_instance.MyMovable as IUsable);
            }
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);        
    }

    void Start()
    {
        InventoryScript.m_instance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (HandScript.m_instance.MyMovable == null)
        {
            if (MyUsable != null)
            {
                MyUsable.Use();
            }
            
            else if (MyUsables != null && MyUsables.Count > 0)
            {
                MyUsables.Peek().Use();
            }
        }
       
    }

    public void SetUsable(IUsable usable)
    {
        if (usable is Item)
        {
            MyUsables = InventoryScript.m_instance.GetUsables(usable);
            if(InventoryScript.m_instance.MyfromSlot != null)
            {
                InventoryScript.m_instance.MyfromSlot.MyCover.enabled = false;
                InventoryScript.m_instance.MyfromSlot.enabled = true;
                InventoryScript.m_instance.MyfromSlot = null;
            }
            
        }
        else
        {
            MyUsables.Clear();
            this.MyUsable = usable;
        }

        count = MyUsables.Count;
        UpdateVisual(usable as IMovable);
        UIManager.m_instance.RefreshTooltip(MyUsable as IDescribable);
    }

    public void UpdateVisual(IMovable movable)
    {
        if (HandScript.m_instance.MyMovable != null)
        {
            HandScript.m_instance.Drop();
        }
        MyIcon.sprite = movable.MyIcon;
        MyIcon.enabled = true;

        if (count > 1)
        {
            UIManager.m_instance.UpdateStackSize(this);
        }
        else if (MyUsable is Throwables)
        {
            UIManager.m_instance.ClearStackCount(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        if (item is IUsable && MyUsables.Count > 0)
        {
            if (MyUsables.Peek().GetType() == item.GetType())
            {
                MyUsables = InventoryScript.m_instance.GetUsables(item as IUsable);

                count = MyUsables.Count;
                UIManager.m_instance.UpdateStackSize(this);

            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;

        if (MyUsable !=null && MyUsable is IDescribable)
        {
            tmp = (IDescribable)MyUsable;
            //UIManager.m_instance.ShowTooltip(transform.position);
        }
        else if (MyUsables.Count>0)
        {
            //UIManager.m_instance.ShowTooltip(transform.position);
        }

        if(tmp != null)
        {
            UIManager.m_instance.ShowTooltip(new Vector2(1,0), transform.position, tmp);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.m_instance.HideTooltip();
    }
}
